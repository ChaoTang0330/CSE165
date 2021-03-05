using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSwitch : MonoBehaviour
{
    public GameObject radialMenu;
    public Transform centerCam;
    void Awake()
    {
        radialMenu.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        radialMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(centerCam.forward, transform.right) < -0.7)
        {
            radialMenu.SetActive(true);
        }
        else
        {
            radialMenu.SetActive(false);
        }
    }
}
