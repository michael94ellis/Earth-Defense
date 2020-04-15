using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class City : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 80.0f;
    /// Laser recharge time
    private List<LaserTurret> LaserTurrets = new List<LaserTurret>();
    private List<GameObject> Buildings = new List<GameObject>();
    private GameObject CityFoundation;

    private class LaserTurret
    {
        public GameObject Turret;
        public LineRenderer Laser;
        public bool isFiring;
        public bool isCharged;
        public int laserRechargeTime = 3;
        public float fireDuration = 0.5f;
    }

    private LaserTurret NewTurret(GameObject turretObject)
    {
        LaserTurret NewLaser = new LaserTurret();
        NewLaser.Turret = turretObject.gameObject;
        NewLaser.Laser = NewLaser.Turret.AddComponent(typeof(LineRenderer)) as LineRenderer;
        NewLaser.isFiring = false;
        NewLaser.isCharged = true;
        return NewLaser;
    }

    void Start()
    {
        UpdateBuildingsList();
    }

    void UpdateBuildingsList()
    {
        Buildings = new List<GameObject>();
        // Add the city's objects to the lists
        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case "CityFoundation":
                    CityFoundation = child.gameObject;
                    break;
                case "Turret":
                    LaserTurrets.Add(NewTurret(child.gameObject));
                    break;
                case "Building":
                    Buildings.Add(child.gameObject);
                    break;
            }
        }
        Debug.Log(LaserTurrets.Count + " Laser Turrets");
        Debug.Log(Buildings.Count + " Buildings");
    }

    // Update is called once per frame
    void Update()
    {
        // Remove nulls
        Buildings.RemoveAll(item => item == null);
        LaserTurrets.RemoveAll(item => item.Turret == null);
        if (Buildings.Count == 0)
        {
            Debug.Log("City Destroyed!");
            //SceneManager.LoadScene("MainMenu");
            return;
        }
        foreach (LaserTurret     turret in LaserTurrets)
        {
            Debug.Log("Laser Turret Activating");
            FireLaserFrom(turret);
        }
    }

    private void FireLaserFrom(LaserTurret turret)
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
                if (turret.isFiring)
                {
                    FireLaserAt(alienShip, turret);
                    return;
                }
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (turret.isCharged)
                {
                    Debug.Log("Laser Turret Beginning Fire Sequence");
                    AimAtTarget(alienShip, turret);
                }
            }
            else
            {
                Debug.Log("Alien too far");
                // Look for any visible target or do nothing, maybe the city can generate value while its not firing its lasers? idk just a thought
            }
        }
    }

    private void AimAtTarget(GameObject alienShip, LaserTurret turret)
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
                FireLaserAt(alienShip, turret);
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
    private void FireLaserAt(GameObject alienShip, LaserTurret turret)
    {
        Debug.Log("Alien Ship In Sight");
        // This begins the laser, the FireLaser() method disables when its done firing
        if (!turret.isFiring)
        {
            Debug.Log("Firing Laser");
            turret.isFiring = true;
            turret.isCharged = false;
            turret.Laser.enabled = true;
            StartCoroutine(FireLaser(turret, turret.fireDuration));
        }
        turret.Laser.material.color = Color.yellow;
        turret.Laser.startWidth = 0.1f;
        turret.Laser.endWidth = 0.1f;
        turret.Laser.material.color = Color.yellow;
        turret.Laser.SetPosition(0, transform.position);
        turret.Laser.SetPosition(1, alienShip.transform.position);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator FireLaser(LaserTurret turret, float fireDuration)
    {
        yield return new WaitForSeconds(fireDuration);
        turret.Laser.enabled = false;
        turret.isFiring = false;
        StartCoroutine(RechargeLaser(turret));
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator RechargeLaser(LaserTurret turret)
    {
        yield return new WaitForSeconds(turret.laserRechargeTime);
        turret.isCharged = true;
    }
}
