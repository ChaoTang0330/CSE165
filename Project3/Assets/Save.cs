using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    public Renderer cube;

    public Transform chairs;
    public Transform desks;

    public string fileName = "Classroom.info";

    private string filePath;

    //private Material cubeMaterial;
    private bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        //cubeMaterial = cube.material;
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "VirtualHand")
        {
            SetIntersect();
        }
        Debug.Log("Collider: " + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "VirtualHand")
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                SetSelect();
            }
            else
            {
                SetIntersect();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VirtualHand")
        {
            SetSilent();
        }
            
    }

    public void SetIntersect()
    {
        isSelected = false;
        cube.material.SetColor("_EmissionColor", Color.white);
    }

    public void SetSelect()
    {
        if(!isSelected)
        {
            isSelected = true;
            cube.material.SetColor("_EmissionColor", Color.red);
            SaveInfo();
        }
    }

    public void SetSilent()
    {
        isSelected = false;
        cube.material.SetColor("_EmissionColor", Color.black);
    }

    private void SaveInfo()
    {
        StreamWriter sw;

        try
        {
            sw = File.CreateText(filePath);
        }
        catch (System.IO.IOException e)
        {
            Debug.Log("Save: " + e.Message);
            return;
        }

        //Debug.Log("Save: Chairs " + chairs.childCount.ToString());
        sw.WriteLine(chairs.childCount);
        foreach (Transform child in chairs)
        {
            Vector3 locPos = child.localPosition;
            Vector3 locRot = child.localEulerAngles;
            sw.WriteLine("{0} {1} {2} {3} {4} {5}",
                locPos.x, locPos.y, locPos.z,
                locRot.x, locRot.y, locRot.z);
        }

        //Debug.Log("Save: Desks " + chairs.childCount.ToString());
        sw.WriteLine(desks.childCount);
        foreach (Transform child in desks)
        {
            Vector3 locPos = child.localPosition;
            Vector3 locRot = child.localEulerAngles;
            sw.WriteLine("{0} {1} {2} {3} {4} {5}",
                locPos.x, locPos.y, locPos.z,
                locRot.x, locRot.y, locRot.z);
        }

        sw.Close();
        sw.Dispose();
    }
}
