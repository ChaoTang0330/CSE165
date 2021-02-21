using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Load : MonoBehaviour
{
    public Renderer cube;

    public GameObject deskPrefab;
    public GameObject chairPrefab;

    public Transform chairs;
    public Transform desks;
    public string fileName = "Classroom.info";

    private string filePath;

    //private Material cubeMaterial;
    private bool isSelected = false;
    private int updateDelay = -1;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (updateDelay > 0)
        {
            updateDelay--;
        }
        else if (updateDelay == 0)
        {
            updateDelay--;
            LoadFurniture();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VirtualHand")
        {
            SetIntersect();
        }
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
        if (!isSelected)
        {
            isSelected = true;
            cube.material.SetColor("_EmissionColor", Color.blue);
            LoadInfo();
        }
    }

    public void SetSilent()
    {
        isSelected = false;
        cube.material.SetColor("_EmissionColor", Color.black);
    }

    public void LoadInfo()
    {
        foreach (Transform child in chairs)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in desks)
        {
            Destroy(child.gameObject);
        }
        
        if(!File.Exists(filePath))
        {
            File.CreateText(filePath);
            return;
        }
        else
        {
            updateDelay = 5;
        }

    }

    private void LoadFurniture()
    {
        StreamReader sr = File.OpenText(filePath);
        string currLine;

        currLine = sr.ReadLine();
        if (currLine == null) return;
        string[] numsStr;

        //chair
        int num = Convert.ToInt32(currLine);
        for (int i = 0; i < num; i++)
        {
            currLine = sr.ReadLine();
            numsStr = currLine.Split(' ');
            Vector3 locPos
                = new Vector3(
                    Convert.ToSingle(numsStr[0]),
                    Convert.ToSingle(numsStr[1]),
                    Convert.ToSingle(numsStr[2]));
            Quaternion locRot
                = Quaternion.Euler(
                    Convert.ToSingle(numsStr[3]),
                    Convert.ToSingle(numsStr[4]),
                    Convert.ToSingle(numsStr[5]));
            GameObject obj = Instantiate(chairPrefab, chairs.TransformPoint(locPos), locRot, chairs);
            obj.GetComponent<FurnitureBehaviour>().setStatusSilent();
        }

        //desk
        currLine = sr.ReadLine();
        num = Convert.ToInt32(currLine);
        for (int i = 0; i < num; i++)
        {
            currLine = sr.ReadLine();
            numsStr = currLine.Split(' ');
            Vector3 locPos
                = new Vector3(
                    Convert.ToSingle(numsStr[0]),
                    Convert.ToSingle(numsStr[1]),
                    Convert.ToSingle(numsStr[2]));
            Quaternion locRot
                = Quaternion.Euler(
                    Convert.ToSingle(numsStr[3]),
                    Convert.ToSingle(numsStr[4]),
                    Convert.ToSingle(numsStr[5]));
            GameObject obj = Instantiate(deskPrefab, desks.TransformPoint(locPos), locRot, desks);
            obj.GetComponent<FurnitureBehaviour>().setStatusSilent();
        }

        sr.Close();
        sr.Dispose();
    }
}
