using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyLaser : MonoBehaviour
{
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
            earth.DisplayNewObjectInNorthAmerica();
            if (Input.GetMouseButton(0))
            {
                isPickingLocation = false;
            }
            return;
        }
    }
    public void BuyLaserTurret()
    {
        isPickingLocation = true;
        earth.BuildNewLaserWeapon();
    }
}
