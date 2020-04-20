using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightDelegate : MonoBehaviour
{
    // Allows a parent to plug in its own methods for what happens when a GameObject enters or exits sight
    public delegate void TriggerEnter(GameObject potentialTarget);
    public event TriggerEnter EnteredSight;
    public delegate void TriggerStay(GameObject potentialTarget);
    public event TriggerEnter StayedInSight;
    public delegate void TriggerExit(GameObject potentialTarget);
    public event TriggerEnter ExitedSight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (EnteredSight != null)
        {
            GameObject newObject = other.gameObject;
            if (newObject != null)
            {
                EnteredSight(newObject);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (StayedInSight != null)
        {
            GameObject newObject = other.gameObject;
            if (newObject != null)
            {
                StayedInSight(newObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (ExitedSight != null)
        {
            GameObject newObject = other.gameObject;
            if (newObject != null)
            {
                ExitedSight(newObject);
            }
        }
    }
}
