using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnDialTurn : UnityEvent<float>
{

}

public class DialBehaviour : MonoBehaviour
{
    //public delegate void ValueChangeAction(float value);
    //public static event ValueChangeAction OnValueChange;
    public OnDialTurn onDialTurn;

    private Vector3 startVec;
    private float startEulerY;
    private bool startRot = false;

    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        if (onDialTurn == null)
        {
            onDialTurn = new OnDialTurn();
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
            if(startRot)
            {
                //Vector3 endVec = other.transform.position - transform.position;
                //endVec.y = 0.0f;

                //if (endVec.magnitude > 1e-6)
                if (Vector3.Dot(transform.up, other.transform.forward) < -0.5)
                {
                    Vector3 endVec = other.transform.up
                        - Vector3.Dot(transform.up, other.transform.up) * transform.up;
                    endVec.Normalize();

                    float currEulerY;
                    //    = transform.localEulerAngles.y;
                    //if(currEulerY > 180) currEulerY -= 360.0f;

                    Vector3 rotUp = Vector3.Cross(endVec, startVec);
                    if(rotUp.magnitude > 1e-6)
                    {
                        if(Vector3.Dot(rotUp, transform.up) > 0)
                        {
                            currEulerY = startEulerY - Vector3.Angle(endVec, startVec);
                        }
                        else
                        {
                            currEulerY = startEulerY + Vector3.Angle(endVec, startVec);
                        }

                        if(currEulerY > 179)
                        {
                            currEulerY = 179;
                            startRot = false;
                        }
                        else if(currEulerY < -179)
                        {
                            currEulerY = -179;
                            startRot = false;
                        }
                        else if(Mathf.Abs(currEulerY - startEulerY) > 90)
                        {
                            startRot = false;
                        }

                        transform.localEulerAngles = new Vector3(0.0f, currEulerY, 0.0f);

                        onDialTurn.Invoke(currEulerY / 179.0f);
                    }
                }
            }
            else
            {
                //startVec = other.transform.position - transform.position;
                //startVec.y = 0.0f;

                //if (startVec.magnitude > 1e-6)
                //{
                //    startRot = true;
                //    startVec.Normalize();
                //    startEulerY = transform.localEulerAngles.y;
                //    if (startEulerY > 180) startEulerY -= 360.0f;
                //}
                
                if (Vector3.Dot(transform.up, other.transform.forward) < -0.5)
                {
                    startRot = true;
                    startVec = other.transform.up 
                        - Vector3.Dot(transform.up, other.transform.up) * transform.up;
                    startVec.Normalize();
                    startEulerY = transform.localEulerAngles.y;
                    if (startEulerY > 180) startEulerY -= 360.0f;
                }
            }
        }
        else
        {
            startRot = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.name != "Hand")
        {
            return;
        }

        outline.enabled = false;
        startRot = false;
    }
}
