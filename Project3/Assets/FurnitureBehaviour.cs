using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureBehaviour : MonoBehaviour
{
    public GameObject indicator;
    private Light indicatorLight;

    private Collider cd;
    private Rigidbody rb;

    const int SILENT = 0;
    const int INTERSECTED = 1;
    const int SELECTED = 1 << 1;
    const int VALID = 1 << 2;
    private int status = SILENT;

    private float range;

    void Awake()
    {
        cd = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        indicatorLight = indicator.GetComponent<Light>();

        range = indicatorLight.range;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -1.0f && (status & SELECTED) != SELECTED)
        {
            Destroy(gameObject);
        }

        indicatorLight.range = range * transform.parent.parent.localScale.x;
    }

    void FixedUpdate()
    {
        if (status == SILENT)
        {
            rb.AddForce(-transform.up * 0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Ground"
            && other.gameObject.tag != "VirtualHand")
        {
            status &= (~VALID);
            indicatorLight.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground"
            && other.gameObject.tag != "VirtualHand")
        {
            status |= VALID;
            indicatorLight.color = Color.green;
        }
    }

    public void setStatusIntersected()
    {
        status = INTERSECTED;
        indicator.SetActive(true);
        indicatorLight.color = Color.blue;
    }

    public void setStatusSilent()
    {
        status = SILENT;
        cd.isTrigger = false;
        rb.isKinematic = false;
        indicator.SetActive(false);
    }

    public void setStatusSelected()//bool isRaycastMode
    {
        status |= SELECTED;
        status |= VALID;
        cd.isTrigger = true;
        rb.isKinematic = true;
        indicatorLight.color = Color.green;
    }

    public void setStatusRelease()
    {
        if((status & VALID) == VALID)
        {
            status &= (~SELECTED);
            status &= (~VALID);
            cd.isTrigger = false;
            rb.isKinematic = false;
            indicatorLight.color = Color.blue;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
