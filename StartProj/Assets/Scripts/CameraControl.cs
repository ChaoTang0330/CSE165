using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public MainGame mainGame;

    public float mouseSpeedH = 2.0f;
    public float mouseSpeedV = 2.0f;

    private float alpha = 0.0f;
    private float beta = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainGame.getStatus() == MainGame.CHANGED)
        {
            mouseSpeedH = mainGame.getMouseSensitive();
            mouseSpeedV = mainGame.getMouseSensitive();
            return;
        }
        else if (mainGame.getStatus() != MainGame.CONTINUE)
        {
            return;
        }

        alpha -= mouseSpeedH * Input.GetAxis("Mouse Y");
        beta += mouseSpeedV * Input.GetAxis("Mouse X");
        transform.eulerAngles = new Vector3(alpha, beta, 0.0f);

        if(Input.GetMouseButtonDown(1))
        {
            alpha = 0.0f;
            beta = 0.0f;
        }
    }
}
