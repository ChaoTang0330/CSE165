using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnSliderMove : UnityEvent<float>
{

}

public class Slider3DBehaviour : MonoBehaviour
{
    public OnSliderMove onSliderMove;
    public float sliderLength = 0.075f;

    private Vector3 startHandPos;
    private float startX;
    private bool startMove = false;

    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        if (onSliderMove == null)
        {
            onSliderMove = new OnSliderMove();
        }

        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        outline.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (startMove)
            {
                Vector3 endHandPos = other.transform.position;
                Vector3 currPos = transform.localPosition;
                float currX;

                currX = startX + Vector3.Dot(transform.right, endHandPos - startHandPos);
                if (currX > sliderLength) currX = sliderLength;
                else if(currX < -sliderLength) currX = -sliderLength;

                currPos.x = currX;
                transform.localPosition = currPos;

                onSliderMove.Invoke(currX / sliderLength);
            }
            else
            {
                startHandPos = other.transform.position;
                startMove = true;
                startX = transform.localPosition.x;
            }
        }
        else
        {
            startMove = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        outline.enabled = false;
        startMove = false;
    }
}
