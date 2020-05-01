using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public GameObject Panel;
    public bool isPickingLocation = false;
    Earth earth;

    void Start()
    {
        earth = GameObject.Find("Earth").GetComponent<Earth>();
    }

    void Update()
    {
        if (isPickingLocation)
        {
            earth.DisplayNewObject();
            if (Input.GetMouseButton(0))
            {
                isPickingLocation = false;
            }
            return;
        }
    }

    public void OpenMenu()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    public void BuyLaserTurret()
    {
        isPickingLocation = true;
        earth.BuildNewLaserWeapon();
    }

    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }

    public void BuyMissileSiloButton()
    {
        isPickingLocation = true;
        earth.BuildNewMissileSilo();
    }
}
