using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirturalHand : MonoBehaviour
{
    public ControlFurniture cf;
    public Transform torso;

    public float GoGoD;
    public float GoGoAlpha;

    private bool GoGoMode;

    private Transform rightController;

    void Awake()
    {
        rightController = transform.parent.transform;
        GoGoMode = cf.GoGoMode;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GoGoMode)
        {
            transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            //transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GoGoMode)
        {
            Vector3 handPos = rightController.position - torso.position;
            float length = handPos.magnitude;
            if (length > GoGoD)
            {
                length += GoGoAlpha * (length - GoGoD) * (length - GoGoD);
                handPos = handPos * length / handPos.magnitude + torso.position;
                transform.localPosition = rightController.InverseTransformPoint(handPos);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        cf.EnterSphereIntersection(other);
    }

    private void OnTriggerExit(Collider other)
    {
        cf.ExitSphereIntersection(other);
    }
}
