using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFurniture : MonoBehaviour
{
    public LineRenderer laser;
    public GameObject sphere;
    public Transform rightController;
    public Transform leftController;

    public GameObject deskPrefab;
    public GameObject chairPrefab;

    public Transform desks;
    public Transform chairs;
    public Transform classroom;
    
    public bool GoGoMode = true;

    public float maxScale = 3.0f;
    public float minScale = 0.3f;

    public Load load;
    public Save save;

    private bool isLoading = false;
    private bool isSaving = false;

    private OVRCameraRig CameraRig;
    private GameObject furnitureObj = null;
    private FurnitureBehaviour furnitureScr = null;

    private Quaternion prevControllerRot;
    private Vector3 prevControllerPos;

    private float timer = 0.0f;
    private bool isScaling = false;
    //private float currScale = 1.0f;
    private float prevScale = 1.0f;
    private float prevDis = 0.0f;
    private Vector3 posInRoom;

    private bool startControl = false;
    private bool isRaycastMode = true;
    private bool isSpawning = false;
    private bool isDesk = false;

    void Awake()
    {
        CameraRig = gameObject.GetComponentInChildren<OVRCameraRig>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (isRaycastMode)
        {
            CheckRaycastIntersection();
        }

        MoveFurniture();

        if (!startControl)
        {
            SpawnFurnitue();

            ScalingScene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!startControl && !isSpawning)
        {
            SwitchControlMode();
        }
        
    }

    private void SwitchControlMode()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (isRaycastMode)
            {
                isRaycastMode = false;
                laser.enabled = false;
                sphere.SetActive(true);
            }
            else
            {
                isRaycastMode = true;
                laser.enabled = true;
                sphere.SetActive(false);
            }

            if (furnitureObj != null)
            {
                furnitureScr.setStatusSilent();
                furnitureObj = null;
                furnitureScr = null;
            }

            if(isLoading)
            {
                load.SetSilent();
                isLoading = false;
            }

            if(isSaving)
            {
                save.SetSilent();
                isSaving = false;
            }
        }
    }

    private void SpawnFurnitue()
    {
        if (!isSpawning)
        {
            if(OVRInput.Get(OVRInput.Button.One))//chair
            {
                isSpawning = true;
                isDesk = false;
                Vector3 spawnPos = new Vector3(0.0f, -0.71f * classroom.transform.localScale.y, 2.0f);
                spawnPos = rightController.rotation * spawnPos + rightController.position;
                furnitureObj = Instantiate(chairPrefab, spawnPos, rightController.rotation, chairs);
            }
            else if(OVRInput.Get(OVRInput.Button.Two))//desk
            {
                isSpawning = true;
                isDesk = true;
                Vector3 spawnPos = new Vector3(0.0f, -0.75f * classroom.transform.localScale.y, 2.0f);
                spawnPos = rightController.rotation * spawnPos + rightController.position;
                furnitureObj = Instantiate(deskPrefab, spawnPos, rightController.rotation, desks);
            }

            if(isSpawning)
            {
                if (isLoading)
                {
                    load.SetSilent();
                    isLoading = false;
                }

                if (isSaving)
                {
                    save.SetSilent();
                    isSaving = false;
                }

                if (furnitureScr != null)
                {
                    furnitureScr.setStatusSilent();
                }
                furnitureScr = furnitureObj.GetComponent<FurnitureBehaviour>();
                furnitureScr.setStatusIntersected();

                furnitureScr.setStatusSelected();
                prevControllerRot = rightController.rotation;
                prevControllerPos = rightController.position;

                if(isRaycastMode)
                {
                    RaycastHit hitInfo;
                    Ray stareAt = new Ray(rightController.position, rightController.forward);
                    Physics.Raycast(stareAt, out hitInfo, 100f);
                    laser.SetPosition(1, new Vector3(0.0f, 0.0f, hitInfo.distance - 0.1f));
                }
            }
        }
        else
        {
            if((OVRInput.Get(OVRInput.Button.One) && !isDesk)
                ||(OVRInput.Get(OVRInput.Button.Two) && isDesk))
            {
                Quaternion currControllerRot = rightController.rotation;
                Vector3 offsetFromController = furnitureObj.transform.position - prevControllerPos;
                Vector3 currControllerPos = rightController.position;

                Quaternion currRot = currControllerRot * Quaternion.Inverse(prevControllerRot);

                furnitureObj.transform.rotation = currRot * furnitureObj.transform.rotation;
                furnitureObj.transform.position = currRot * offsetFromController + currControllerPos;

                prevControllerRot = currControllerRot;
                prevControllerPos = currControllerPos;
            }
            else
            {
                furnitureScr.setStatusRelease();
                furnitureScr.setStatusSilent();
                isSpawning = false;
                furnitureObj = null;
                furnitureScr = null;
            }
        }
    }

    private void CheckRaycastIntersection()
    {
        if (isSpawning || startControl) return;

        // cast ray
        RaycastHit hitInfo;
        bool hitFlag;
        Ray stareAt = new Ray(rightController.position, rightController.forward);
        hitFlag = Physics.Raycast(stareAt, out hitInfo, 100f);
        if (hitFlag)
        {
            //Vector3 laserEnd = new Vector3(0.0f, 0.0f, hitInfo.distance - 0.1f);
            laser.SetPosition(1, rightController.InverseTransformPoint(hitInfo.point));

            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                return;
            }

            if (hitInfo.transform.gameObject.tag == "Furniture")
            {
                if (furnitureObj == null)
                {
                    furnitureObj = hitInfo.transform.gameObject;
                    furnitureScr = furnitureObj.GetComponent<FurnitureBehaviour>();
                    furnitureScr.setStatusIntersected();
                }
                else if (furnitureObj != hitInfo.transform.gameObject)
                {
                    furnitureScr.setStatusSilent();

                    furnitureObj = hitInfo.transform.gameObject;
                    furnitureScr = furnitureObj.GetComponent<FurnitureBehaviour>();
                    furnitureScr.setStatusIntersected();
                }
            }
            else
            {
                if (furnitureObj != null)
                {
                    furnitureScr.setStatusSilent();
                    furnitureObj = null;
                    furnitureScr = null;
                }
            }

            if(hitInfo.transform == save.transform)
            {
                isSaving = true;
                save.SetIntersect();
            }
            else if (isSaving)
            {
                save.SetSilent();
                isSaving = false;
            }

            if (hitInfo.transform == load.transform)
            {
                isLoading = true;
                load.SetIntersect();
            }
            else if (isLoading)
            {
                load.SetSilent();
                isLoading = false;
            }
        }
        else
        {
            laser.SetPosition(1, new Vector3(0.0f, 0.0f, 5.0f));

            if (furnitureObj != null)
            {
                furnitureScr.setStatusSilent();
                furnitureObj = null;
                furnitureScr = null;
            }

            if (isSaving)
            {
                save.SetSilent();
                isSaving = false;
            }

            if (isLoading)
            {
                load.SetSilent();
                isLoading = false;
            }
        }
    }

    public void EnterSphereIntersection(Collider other)
    {
        if (startControl
            || isSpawning) //OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) ||
        {
            return;
        }

        if (other.gameObject.tag == "Furniture")
        {
            if (furnitureObj == null)
            {
                furnitureObj = other.gameObject;
                furnitureScr = furnitureObj.GetComponent<FurnitureBehaviour>();
                furnitureScr.setStatusIntersected();
            }
            else if (furnitureObj != other.gameObject)
            {
                furnitureScr.setStatusSilent();

                furnitureObj = other.gameObject;
                furnitureScr = furnitureObj.GetComponent<FurnitureBehaviour>();
                furnitureScr.setStatusIntersected();
            }
        }
        //else if(other.gameObject.tag == "Cube")
        //{
        //    load.SetIntersect();
        //}
        
        //if (other.gameObject.name == "Save")
        //{
        //    Debug.Log("Intersect Save");
        //    isSaving = true;
        //    save.SetIntersect();
        //    if(isLoading)
        //    {
        //        isLoading = false;
        //        load.SetSilent();
        //    }
        //}
        
        //if (other.gameObject.name == "Load")
        //{
        //    Debug.Log("Intersect Log");
        //    isLoading = true;
        //    load.SetIntersect();
        //    if(isSaving)
        //    {
        //        isSaving = false;
        //        save.SetSilent();
        //    }
        //}
    }

    public void ExitSphereIntersection(Collider other)
    {
        if (startControl || isSpawning)
        {
            return;
        }

        if (furnitureObj != null
            && other.gameObject == furnitureObj)
        {
            furnitureScr.setStatusSilent();
            furnitureObj = null;
            furnitureScr = null;
        }
        //else if(other.gameObject.name == "Load")
        //{
        //    isLoading = false;
        //    load.SetSilent();
        //}
        //else if (other.gameObject.name == "Save")
        //{
        //    isSaving = false;
        //    save.SetSilent();
        //}
    }

    private void MoveFurniture()
    {
        if (isSpawning || isScaling) return;

        if(furnitureObj == null)
        {
            startControl = false;

            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (isLoading)
                {
                    load.SetSelect();
                }
                else if (isSaving)
                {
                    save.SetSelect();
                }
            }

            return;
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (!startControl)
            {
                furnitureScr.setStatusSelected();

                prevControllerRot = rightController.rotation;

                if(GoGoMode)
                {
                    prevControllerPos = sphere.transform.position;
                }
                else
                {
                    prevControllerPos = rightController.position;
                }

                startControl = true;
            }

            Quaternion currControllerRot = rightController.rotation;
            Vector3 offsetFromController = furnitureObj.transform.position - prevControllerPos;
            Vector3 currControllerPos;
            if(GoGoMode)
            {
                currControllerPos = sphere.transform.position;
            }
            else
            {
                currControllerPos = rightController.position;
            }

            Quaternion currRot = currControllerRot * Quaternion.Inverse(prevControllerRot);

            furnitureObj.transform.rotation = currRot * furnitureObj.transform.rotation;
            furnitureObj.transform.position = currRot * offsetFromController + currControllerPos;

            prevControllerRot = currControllerRot;
            prevControllerPos = currControllerPos;
        }
        else if (startControl)
        {
            furnitureScr.setStatusRelease();
            furnitureScr.setStatusSilent();
            furnitureObj = null;
            furnitureScr = null;
            startControl = false;
        }
        else
        {
            startControl = false;
        }
    }

    private void ScalingScene()
    {
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)
            && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if(!isScaling)
            {
                isScaling = true;
                timer = 0.0f;
                //prevDis = Vector3.Distance(rightController.position, leftController.position);
                //prevScale = transform.localScale.x;
                posInRoom = classroom.InverseTransformPoint(transform.position);
            }
            else
            {
                if(timer < 0.5f)
                {
                    timer += Time.fixedDeltaTime;

                    prevDis = Vector3.Distance(rightController.position, leftController.position);
                    prevScale = classroom.localScale.x;
                }
                else
                {
                    float currDis = Vector3.Distance(rightController.position, leftController.position);
                    float currScale = prevScale + currDis - prevDis;
                    currScale = currScale > maxScale ? maxScale : currScale;
                    currScale = currScale < minScale ? minScale : currScale;
                    classroom.localScale = new Vector3(currScale, currScale, currScale);

                    transform.position = classroom.TransformPoint(posInRoom);
                }
                
            }
        }
        else
        {
            if(isScaling)
            {
                if (timer < 0.5)
                {
                    //prevScale = transform.localScale.x;
                    classroom.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    transform.position = posInRoom;
                }
            }

            timer = 0.0f;
            isScaling = false;
        }
    }
}
