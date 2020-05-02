using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public GameObject Panel;
    private bool isPickingLocation = false;
    Earth earth;

    void Start()
    {
        //needed to initialize earth at start
        earth = GameObject.Find("Earth").GetComponent<Earth>();
    }

    void Update()
    {
        //previews placement of new building until clicked
        //TO DO: need a check for if the click is an approved placement
        if (isPickingLocation)
        {
            earth.DisplayNewObject();
            if (Input.GetMouseButton(0))
            {
                isPickingLocation = false;
                //turns colliders back off
                foreach (EarthZone zone in Earth.ControlledZones)
                {
                    zone.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }

    //opens build menu
    public void OpenMenu()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    //-----New Buildings-----
    public void BuyLaserTurret()
    {
        isPickingLocation = true;
        earth.BuildNewLaserWeapon();
    }


    public void BuyMissileSiloButton()
    {
        ActivateZoneColliders();
        isPickingLocation = true;
        earth.BuildNewMissileSilo();
    }
    //----------------------

    //turn colliders on for zones
    public void ActivateZoneColliders()
    {
        foreach (EarthZone zone in Earth.ControlledZones)
        {
            zone.GetComponent<Collider>().enabled = true;
        }
    }

    //sends new aliens
    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }
}
