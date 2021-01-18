using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    public float IninMovementSpeed = 1;
    public float jumpForce = 300;
    public float maskThreshold = 2.0f;

    public Slider slider;
    public GameObject indicator;
    public GameObject maskObj;
    private MainGame mainGame = null;

    private Animator animator;
    //private Collider collider;
    private Rigidbody rb;
    private Vector3 forwardDir = new Vector3(0.0f, 0.0f, 1.0f);

    private bool beStared =  false;
    private float staredTimer = 0.0f;

    private bool maskFlag = false;

    private int prevMainGameStatus = 0;
    private float movementSpeed = 1;

    void Awake()
    {
        mainGame = GameObject.Find("MainGame").GetComponent<MainGame>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        animator.SetInteger("Walk", 1);
        slider.value = 0.0f;
        indicator.SetActive(false);
        movementSpeed = mainGame.getCharacterSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        int currMainGameStatus = mainGame.getStatus();
        if(currMainGameStatus == MainGame.CONTINUE)
        {
            if(prevMainGameStatus == MainGame.CHANGED 
                || prevMainGameStatus == MainGame.FINISH)
            {
                Destroy(gameObject);
            }
            prevMainGameStatus = currMainGameStatus;
            animator.SetInteger("Walk", 1);
        }
        else
        {
            animator.SetInteger("Walk", 0);
            prevMainGameStatus = currMainGameStatus;
            return;
        }

        transform.Translate(forwardDir * movementSpeed * Time.deltaTime);

        //refresh mask
        if(!maskFlag)
        {
            if(beStared)
            {
                staredTimer += Time.deltaTime;
                slider.value = staredTimer / 2.0f * 0.97f;
                if (staredTimer > 2.0f)
                {
                    maskFlag = true;
                    //indicator.SetActive(false);
                    maskObj.SetActive(true);
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider otherObj)
    {
        if(otherObj.gameObject.tag == "Despawn")
        {
            Destroy(gameObject);
        }
        else if (otherObj.gameObject.tag == "Finish" && !maskFlag)
        {
            rb.AddForce(0, jumpForce, 0);
            animator.SetTrigger("jump");
            mainGame.setFinishStatus();
        }
    }

    public void setStaredTimer()
    {
        if (!maskFlag)
        {
            beStared = true;
            staredTimer = 0.0f;
            slider.value = 0.0f;
        }
        indicator.SetActive(true);
    }

    public void clearStaredTimer()
    {
        if (!maskFlag)
        {
            beStared = false;
            staredTimer = 0.0f;
            slider.value = 0.0f;
        }
        indicator.SetActive(false);
    }
}
