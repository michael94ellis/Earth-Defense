﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickMenu : MonoBehaviour
{
    public GameObject Panel;
    public Text header;
    public Text population;
    public Text shield;
    public Transform target = null;
    bool isActive;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    if (hit.transform.name != "Earth")
                    {
                        target = hit.transform;
                        //TODO LOOK INTO RETURNING THIS FOR UPDATING LIFE TOTAL
                        header.text = target.name;
                        DisplayPanel();
                    }
                }
            }
        }
        if (isActive && target)
        {
            populationText(target);
            Debug.Log("updating hp");
        }
    }

    public void DisplayPanel()
    {
        if (Panel != null)
        {
            isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    public void populationText(Transform target)
    {
        population.text = target.parent.gameObject.GetComponent<EarthZone>().Population.ToString() + "/"
                         + target.parent.gameObject.GetComponent<EarthZone>().MaxPopulation + "\npopulation";
    }
}