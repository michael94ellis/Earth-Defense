using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    public GameObject Panel;
    public Text header;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                if(hit.transform.name != "Earth" && hit.transform.name != "EarthZone")
                    {
                    Debug.Log("HIT");
                    header.text = hit.transform.name;
                    DisplayPanel();
                    }
            }
        }
    }
    }
    //TODO Fix display
    public void DisplayPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}
