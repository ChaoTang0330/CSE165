using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnColorButtonClick : UnityEvent<Color>
{

}

public class ColorButtonBehaviour : MonoBehaviour
{
    public OnColorButtonClick onColorButtonClick;
    public Color targetColor;

    private Outline outline;
    private bool isSet = false;

    // Start is called before the first frame update
    void Start()
    {
        if(onColorButtonClick == null)
        {
            onColorButtonClick = new OnColorButtonClick();
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
        if(!isSet && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            isSet = true;
            onColorButtonClick.Invoke(targetColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        outline.enabled = false;
        isSet = false;
    }
}
