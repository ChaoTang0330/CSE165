using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacter : MonoBehaviour
{
    public Transform NaviArrow;
    public Transform Destination;
    public Text usedTime;

    public GameObject mapCanvas;
    public GameObject finishCanvas;
    public Text currRun;
    public Text bestRun;

    Vector3 startPos;
    Quaternion startOri;

    float shorestTime = float.PositiveInfinity;
    float timer = 0.0f;
    bool startTimer = false;
    bool finishFlag = false;

    float movementSpeed = 15.0f;
    float turningSpeed = 30.0f;
    Rigidbody rb;
    Vector3 mousePos = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);

    bool teleportFlag = false;
    int groundLayer = 1 << 9;
    Vector3 nextDir;
    float nextDis = 0.0f;
    float deltaDis;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        startOri = transform.rotation;
        deltaDis = movementSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        NaviArrow.LookAt(Destination, Vector3.up);

        if(finishFlag)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                finishFlag = false;
                timer = 0;
                transform.position = startPos;
                transform.rotation = startOri;

                usedTime.text = string.Format("{0:d2}:{1:d2}", 0, 0);

                mapCanvas.SetActive(true);
                finishCanvas.SetActive(false);
            }
        }

        if(startTimer)
        {
            timer += Time.deltaTime;
            int minutes = (int)timer / 60;
            int seconds = (int)timer % 60;
            usedTime.text = string.Format("{0:d2}:{1:d2}", minutes, seconds);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            teleportFlag ^= true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        if(!teleportFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                float turningAngle = turningSpeed * Time.fixedDeltaTime
                    * (Input.mousePosition.x - mousePos.x) / Screen.width;
                Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, turningAngle, 0));
                rb.MoveRotation(rb.rotation * deltaRotation);

                float movementDis = movementSpeed * Time.fixedDeltaTime
                    * (Input.mousePosition.y - mousePos.y) / Screen.height;

                rb.MovePosition(transform.position + transform.forward * movementDis);
                //transform.Translate(new Vector3(0, 0, movementDis), Space.Self);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                mousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                float turningAngle = turningSpeed * Time.fixedDeltaTime
                    * (Input.mousePosition.x - mousePos.x) / Screen.width;
                Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, turningAngle, 0));
                rb.MoveRotation(rb.rotation * deltaRotation);
            }

            if(Input.GetMouseButtonDown(0))
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, 
                        Input.mousePosition.y,
                        Camera.main.nearClipPlane));

                // cast ray
                RaycastHit hitInfo;
                bool hitFlag;
                Ray stareAt = new Ray(point, point - Camera.main.transform.position);
                hitFlag = Physics.Raycast(stareAt, out hitInfo, Mathf.Infinity, groundLayer);
                while(hitFlag && hitInfo.transform.tag != "Ground")
                {
                    stareAt.origin = hitInfo.point;
                    hitFlag = Physics.Raycast(stareAt, out hitInfo, Mathf.Infinity, groundLayer);
                }
                if(hitFlag)
                {
                    //Vector3 nextPos = hitInfo.point;
                    //nextPos.y = transform.position.y;
                    //rb.MovePosition(nextPos);
                    nextDir = hitInfo.point - transform.position;
                    nextDir.y = 0;
                    nextDis = nextDir.magnitude;
                    nextDir /= nextDis;
                }
            }

            if(nextDis > deltaDis)
            {
                nextDis -= deltaDis;
                rb.MovePosition(transform.position + nextDir * deltaDis);
            }
            else
            {
                nextDis = 0;
            }
        }
        
    }

    private void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.gameObject.tag == "Finish")
        {
            startTimer = false;
            if(timer < shorestTime)
            {
                shorestTime = timer;
            }
            currRun.text = string.Format("You used {0:d} seconds.", (int) timer);
            bestRun.text = string.Format("Best Run: {0:d} seconds", (int) shorestTime);
            finishFlag = true;
            timer = 5.0f;
            mapCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider otherObj)
    {
        if (otherObj.gameObject.tag == "Start")
        {
            startTimer = true;
        }
    }
}
