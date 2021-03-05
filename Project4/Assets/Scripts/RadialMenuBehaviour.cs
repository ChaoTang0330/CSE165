using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnRadialMenuClick : UnityEvent<int>
{

}

public class RadialMenuBehaviour : MonoBehaviour
{
    //public GameObject[] prefabs = new GameObject[4];
    public GameObject[] sections = new GameObject[4];
    public OnRadialMenuClick onRadialMenuClick;

    private int currIdx = -1;
    private bool isGenerated = false;

    // Start is called before the first frame update
    void Start()
    {
        if(onRadialMenuClick == null)
        {
            onRadialMenuClick = new OnRadialMenuClick();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        Vector3 localPos = transform.InverseTransformPoint(other.transform.position);
        if(localPos.magnitude < 0.16)
        {
            int nextIdx;
            if(localPos.z > 0)
            {
                if (localPos.y > 0) nextIdx = 0;
                else nextIdx = 3;
            }
            else
            {
                if (localPos.y > 0) nextIdx = 1;
                else nextIdx = 2;
            }

            if(currIdx != nextIdx)
            {
                if(currIdx >= 0) sections[currIdx].SetActive(false);
                sections[nextIdx].SetActive(true);
                currIdx = nextIdx;
            }

            if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && !isGenerated)
            {
                onRadialMenuClick.Invoke(currIdx);
                isGenerated = true;
            }
        }
        else
        {
            if(currIdx >= 0)
            {
                sections[currIdx].SetActive(false);
                currIdx = -1;
            }
            isGenerated = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        if (currIdx >= 0)
        {
            sections[currIdx].SetActive(false);
            currIdx = -1;
        }

        isGenerated = false;
    }

    void OnDisable()
    {
        if (currIdx >= 0)
        {
            sections[currIdx].SetActive(false);
            currIdx = -1;
        }
    }

}
