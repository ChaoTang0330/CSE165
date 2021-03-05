using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    private Renderer block;
    private Outline outline;
    private Rigidbody rb;

    private bool isSelected;
    private Vector3 originScale;

    //private Vector3 prevPos;
    private Vector3[] posTracker;
    private int interval = 5;
    private int currIdx = 0;
    private bool isFull = false;

    private bool willDelete = false;

    void Awake()
    {
        block = gameObject.GetComponent<Renderer>();
        outline = gameObject.GetComponent<Outline>();
        rb = gameObject.GetComponent<Rigidbody>();
        outline.enabled = false;
        originScale = transform.localScale;

        posTracker = new Vector3[interval];
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    void FixedUpdate()
    {
        if(rb.isKinematic)
        {
            posTracker[currIdx] = transform.position;
            currIdx++;
            if (currIdx == interval)
            {
                currIdx = 0;
                isFull = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(willDelete && !rb.isKinematic)
        {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color)
    {
        block.material.SetColor("_Color", color);
    }

    public void SetSelected(bool status)
    {
        isSelected = status;
        outline.enabled = status;
        //rb.isKinematic = false;
    }

    public void SetControlled(bool status)
    {
        if(status)
        {
            rb.isKinematic = true;
            //rb.velocity = (transform.position - prevPos) / Time.fixedDeltaTime;
        }
        else
        {
            //Vector3 v = rb.velocity;
            rb.isKinematic = false;
            //rb.velocity = v;
            //rb.velocity = (transform.position - prevPos) / Time.fixedDeltaTime;
            if(isFull)
            {
                isFull = false;
                if(currIdx == 0)
                {
                    rb.velocity = (posTracker[interval - 1] - posTracker[0]) 
                        / (Time.fixedDeltaTime * (float)interval);
                }
                else
                {
                    rb.velocity = (posTracker[currIdx - 1] - posTracker[currIdx])
                        / (Time.fixedDeltaTime * (float)interval);
                }
            }
            currIdx = 0;
        }
    }

    public void SetSize(float size)
    {
        transform.localScale = size * originScale;
        rb.mass = size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Delete")
        {
            willDelete = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Delete")
        {
            willDelete = false;
        }
    }
}
