using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    public float gravityFactor;
    public float sizeFactor;

    private GameObject currBlockObj = null;
    private BlockBehaviour currBlockScr = null;
    private bool isHolding = false;
    private Vector3 originGravity;

    private float currBlockSize = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        originGravity = Physics.gravity;
    }

    void FixedUpdate()
    {
        if (isHolding)
        {
            if (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                isHolding = false;
                currBlockObj.transform.parent = null;
                currBlockScr.SetControlled(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ChangeGravity(float value)
    {
        Physics.gravity = Mathf.Pow(gravityFactor, value) * originGravity;
    }

    public void ChangeSize(float value)
    {
        currBlockSize = Mathf.Pow(sizeFactor, value);
        if(currBlockScr)
        {
            currBlockScr.SetSize(currBlockSize);
        }
    }

    public void ChangeColor(Color color)
    {
        if (currBlockScr)
        {
            currBlockScr.SetColor(color);
        }
    }

    public void GenerateBlock(int idx)
    {
        if(!isHolding)
        {
            isHolding = true;
            if(currBlockObj)
            {
                currBlockScr.SetSelected(false);
            }
            
            currBlockObj = Instantiate(blockPrefabs[idx], 
                transform.position, Quaternion.Euler(0, 0, 0), transform.parent);
            currBlockScr = currBlockObj.GetComponent<BlockBehaviour>();
            currBlockScr.SetSize(currBlockSize);
            currBlockScr.SetSelected(true);
            currBlockScr.SetControlled(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag != "Block")
        {
            return;
        }

        if (!isHolding)
        {
            if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                isHolding = true;

                if(currBlockScr)
                {
                    currBlockScr.SetSelected(false);
                }

                currBlockObj = other.gameObject;
                currBlockObj.transform.parent = transform.parent;

                currBlockScr = currBlockObj.GetComponent<BlockBehaviour>();
                currBlockScr.SetSelected(true);
                currBlockScr.SetControlled(true);
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{

    //}
}
