﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public LineRenderer Laser;
    public bool isFiring;
    public bool isCharged;
    public int laserRechargeTime = 3;
    public float fireDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Laser = gameObject.GetComponent<LineRenderer>();
        isFiring = false;
        isCharged = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Find how far the ship is
        foreach (GameObject alienShip in GameObject.FindGameObjectsWithTag("Alien"))
        {
            // Make sure this isn't a dead ship
            if (alienShip == null)
            {
                // Update our list if we found a dead ship and try again
                Debug.Log("Dead Alien Confirmed");
                return;
            }
            float distanceToAlienShip = Vector3.Distance(alienShip.transform.position, transform.position);
            // If within firing range then prepare to fire the laser
            if (distanceToAlienShip < 15)
            {
                // Animation for the laser while its bein fired
                if (isFiring)
                {
                    FireLaserAt(alienShip);
                    return;
                }
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (isCharged)
                {
                    Debug.Log("Laser Turret Beginning Fire Sequence");
                    AimAtTarget(alienShip);
                }
            }
            else
            {
                Debug.Log("Alien too far");
                // Look for any visible target or do nothing, maybe the city can generate value while its not firing its lasers? idk just a thought
            }
        }
    }

    private void AimAtTarget(GameObject alienShip)
    {
        // Determine if there is line of sight to the alien ship
        RaycastHit hit;
        Vector3 alienShipDirection = alienShip.transform.position - transform.position;
        if (Physics.Raycast(transform.position, alienShipDirection, out hit))
        {
            // An object is seen, is it an alien ship?
            Debug.Log("Can See Object " + hit.transform.gameObject);
            // Don't shoot other stuff
            if (hit.transform.tag == "Alien")
            {
                // Begin animating laser
                FireLaserAt(alienShip);
                // Destroy the ship
                GameObject destroyedAlienShip = alienShip;
                Destroy(destroyedAlienShip);
                return;
            }
            else
            {
                // Something is in the way
                Debug.Log("Alien Ship Not In Sight");
            }
        }
        else
        {
            Debug.Log("Can Not See Alien Ship");
        }
    }

    /// This animates the laser firing
    private void FireLaserAt(GameObject alienShip)
    {
        Debug.Log("Alien Ship In Sight");
        // This begins the laser, the FireLaser() method disables when its done firing
        if (!isFiring)
        {
            Debug.Log("Firing Laser");
            isFiring = true;
            isCharged = false;
            Laser.enabled = true;
            StartCoroutine(FireLaser());
        }
        Laser.receiveShadows = false;
        Laser.material.color = Color.green;
        Laser.startWidth = 0.1f;
        Laser.endWidth = 0.1f;
        Laser.SetPosition(0, transform.position);
        Laser.SetPosition(1, alienShip.transform.position);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(fireDuration);
        isFiring = false;
        StartCoroutine(RechargeLaser());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator RechargeLaser()
    {
        yield return new WaitForSeconds(laserRechargeTime);
        isCharged = true;
    }
}