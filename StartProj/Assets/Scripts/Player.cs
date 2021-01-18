using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public MainGame mainGame;

    public int layerMask = 1 << 8;
    public Image crosshair;

    private GameObject beStaredObj = null;
    private CharacterBehaviour beStaredScr = null;

    //private bool startTimer;
    //private float timer;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Camera: " + Camera.main.transform.forward.x);// + Camera.main.transform.forward.x
        crosshair.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainGame.getStatus() != MainGame.CONTINUE) return;

        // cast ray
        RaycastHit hitInfo;
        bool hitFlag;
        Ray stareAt = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        hitFlag = Physics.Raycast(stareAt, out hitInfo, 100f, layerMask);
        if (hitFlag)
        {
            if (beStaredObj == null)
            {
                beStaredObj = hitInfo.transform.gameObject;
                beStaredScr = beStaredObj.GetComponent<CharacterBehaviour>();
                beStaredScr.setStaredTimer();
            }
            else if (beStaredObj != hitInfo.transform.gameObject)
            {
                //clear previous
                beStaredScr.clearStaredTimer();
                //set new
                beStaredObj = hitInfo.transform.gameObject;
                beStaredScr = beStaredObj.GetComponent<CharacterBehaviour>();
                beStaredScr.setStaredTimer();
            }

            //debug draw
            //Debug.DrawLine(stareAt.origin, hitInfo.transform.position);
            //Debug.Log("Hit Character");
            crosshair.color = Color.yellow;
        }
        else
        {
            if (beStaredObj != null)
            {
                //clear previous
                beStaredScr.clearStaredTimer();
                beStaredObj = null;
                beStaredScr = null;
            }

            crosshair.color = Color.gray;
        }
        
    }
}
