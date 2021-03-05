using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public float iniHeight = 1.7f;
    //public float moveFactor = 3.0f;

    private bool playerControllerEnabled = false;
    private OVRCameraRig CameraRig;

    //private Vector3 prevTouchPos;
    //private Quaternion prevRot;
    //private bool currGrabHand = false;
    //private bool isTurning = false;
    //private bool isMoving = false;

    void Awake()
    {
        CameraRig = gameObject.GetComponentInChildren<OVRCameraRig>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add eye-depth as a camera offset from the player controller
        var p = CameraRig.transform.localPosition;
        p.z = OVRManager.profile.eyeDepth;
        CameraRig.transform.localPosition = p;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerEnabled)
        {
            if (OVRManager.OVRManagerinitialized)
            {
                OVRManager.display.RecenteredPose += ResetOrientation;

                if (CameraRig != null)
                {
                    CameraRig.UpdatedAnchors += UpdateTransform;
                }
                playerControllerEnabled = true;
            }
            else
                return;
        }
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
        if (playerControllerEnabled)
        {
            OVRManager.display.RecenteredPose -= ResetOrientation;

            if (CameraRig != null)
            {
                CameraRig.UpdatedAnchors -= UpdateTransform;
            }
            playerControllerEnabled = false;
        }
    }

    public void UpdateTransform(OVRCameraRig rig)
    {
        //if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        //{
        //    return;
        //}

        //bool isRightDown = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        //bool isLeftDown = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);

        //if(isRightDown && isLeftDown)
        //{
        //    isMoving = false;
        //    if(!isTurning)
        //    {
        //        isTurning = true;
        //        prevTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch)
        //            - OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        //        prevTouchPos.y = 0;
        //        prevTouchPos = prevTouchPos.normalized;
        //        prevRot = transform.rotation;
        //    }
        //    else
        //    {
        //        Vector3 currTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch)
        //            - OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        //        currTouchPos.y = 0;
        //        currTouchPos = currTouchPos.normalized;
        //        transform.rotation =
        //            //Quaternion.Inverse(Quaternion.FromToRotation(prevTouchPos, currTouchPos))
        //            //Quaternion.FromToRotation(prevTouchPos, currTouchPos)
        //        Quaternion.FromToRotation(currTouchPos, prevTouchPos)
        //            * prevRot;
        //        //prevTouchPos = currTouchPos;
        //    }
        //}
        //else if(isRightDown)
        //{
        //    isTurning = false;
        //    if(!isMoving || !currGrabHand)
        //    {
        //        isMoving = true;
        //        currGrabHand = true;
        //        prevTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        //    }
        //    else
        //    {
        //        Vector3 currTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        //        Vector3 trans = prevTouchPos - currTouchPos;
        //        trans.y = 0.0f;
        //        transform.Translate(trans * moveFactor);
        //        prevTouchPos = currTouchPos;
        //    }
            
        //}
        //else if(isLeftDown)
        //{
        //    isTurning = false;
        //    if (!isMoving || currGrabHand)
        //    {
        //        isMoving = true;
        //        currGrabHand = false;
        //        prevTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        //    }
        //    else
        //    {
        //        Vector3 currTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        //        Vector3 trans = prevTouchPos - currTouchPos;
        //        trans.y = 0.0f;
        //        transform.Translate(trans * moveFactor);
        //        prevTouchPos = currTouchPos;
        //    }
        //}
        //else
        //{
        //    isMoving = false;
        //    isTurning = false;
        //}
    }

    public void ResetOrientation()
    {
        //if (HmdResetsY && !HmdRotatesY)
        //{
        //    Vector3 euler = transform.rotation.eulerAngles;
        //    euler.y = InitialYRotation;
        //    transform.rotation = Quaternion.Euler(euler);
        //}
    }
}
