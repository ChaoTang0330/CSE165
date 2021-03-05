using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnColorChanged : UnityEvent<Color>
{

}

public class ColorPicker : MonoBehaviour
{
    public Transform indiator;
    public LineRenderer track;
    public OnColorChanged onColorChanged;
    
    
    private Outline outline;
    private float hue = 0.0f, saturation = 0.0f, brightness = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (onColorChanged == null)
        {
            onColorChanged = new OnColorChanged();
        }

        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;

        track.startColor = Color.HSVToRGB(hue, saturation, 0.0f);
        track.endColor = Color.HSVToRGB(hue, saturation, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetHueAndSaturation(float x, float y)
    {
        x /= 0.5f;
        y /= 0.5f;
        saturation = Mathf.Sqrt(x * x + y * y);

        float theta = Mathf.Atan(y / x);
        if(x > 0)
        {
            if (y < 0) theta += 2 * Mathf.PI;
        }
        else
        {
            theta += Mathf.PI;
        }

        hue = theta / Mathf.PI / 2;
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
            Vector3 pos = transform.InverseTransformPoint(other.transform.position);
            //pos *= indiator.localScale.x;
            //Debug.Log(String.Format("Local Position: {0}, {1}, {2}", pos.x, pos.y, pos.z));
            pos.z = 0.0f;
            if (pos.magnitude > 0.5f)
            {
                pos.Normalize();
                pos *= 0.5f;
            }

            indiator.localPosition = pos;
            SetHueAndSaturation(pos.x, pos.y);

            track.startColor = Color.HSVToRGB(hue, saturation, 0.0f);
            track.endColor = Color.HSVToRGB(hue, saturation, 1.0f);

            Color currColor = Color.HSVToRGB(hue, saturation, brightness);
            onColorChanged.Invoke(currColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        outline.enabled = false;
    }

    public void ChangeBrightness(float value)
    {
        brightness = (value + 1.0f) / 2.0f;
        Color currColor = Color.HSVToRGB(hue, saturation, brightness);
        onColorChanged.Invoke(currColor);
    }
}
