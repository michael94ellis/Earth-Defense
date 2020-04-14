using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 80.0f;
    bool isFiringLaser = false;
    List<GameObject> cityBuildings = new List<GameObject>();
    /// Draws the laser
    LineRenderer laser;
    /// Laser recharge time
    private bool isLaserCharged = true;
    private int laserRechargeTime = 3;
    private float fireDuration = 0.5f;
    public List<GameObject> aliens;
    GameObject laserTurret;
    GameObject laserPivot;

    // Start is called before the first frame update
    void Start()
    {
        // Add the turret reference
        laserTurret = transform.Find("Turret").gameObject;
        // Add the pivot reference for aiming the turret
        laserPivot = laserTurret.transform.Find("BarrelPivot").gameObject;
        // Add the ability to draw laser beams
        laser = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        // Find a target alien ship
        foreach (GameObject alien in GameObject.FindGameObjectsWithTag("Alien"))
        {
            aliens.Add(alien);
        }
        // Add the city's buildings to the list
        foreach (Transform child in transform)
        {
            cityBuildings.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Find how far the ship is
        foreach (GameObject alienShip in aliens)
        {
            float distanceToAlienShip = Vector3.Distance(alienShip.transform.position, transform.position);
            // If within firing range then prepare to fire the laser
            if (distanceToAlienShip < 15)
            {
                // Always be aiming
                laserPivot.transform.LookAt(alienShip.transform);
                // Animation for the laser while its bein fired
                if (isFiringLaser)
                {
                    FireLaserAt(alienShip);
                    return;
                }
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (isLaserCharged)
                {
                    // Determine if there is line of sight to the building
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
                            //GameObject destroyedBuilding = building;
                            //cityBuildings.Remove(building);
                            //Destroy(destroyedBuilding);
                            break;
                        }
                        else
                        {
                            // Something is in the way
                            Debug.Log("Alien Ship Not In Sight");
                            laser.enabled = false;
                        }
                    }
                    else
                    {
                        Debug.Log("Can Not See Alien Ship");
                    }
                }
            }
            else
            {
                // Look for any visible target or do nothing, maybe the city can generate value while its not firing its lasers? idk just a thought
            }
        }
    }

    /// This animates the laser firing
    private void FireLaserAt(GameObject alienShip)
    {
        Debug.Log("Alien Ship In Sight");
        // This begins the laser, the FireLaser() method disables when its done firing
        if (!isFiringLaser)
        {
            Debug.Log("Firing Laser");
            StartCoroutine(FireLaser());
            laser.enabled = true;
        }
        laser.startWidth = 0.1f;
        laser.endWidth = 0.1f;
        laser.material.color = Color.yellow;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, alienShip.transform.position);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator FireLaser()
    {
        isFiringLaser = true;
        isLaserCharged = false;
        yield return new WaitForSeconds(fireDuration);
        laser.enabled = false;
        isFiringLaser = false;
        StartCoroutine(RechargeLaser());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator RechargeLaser()
    {
        yield return new WaitForSeconds(laserRechargeTime);
        isLaserCharged = true;
    }
}
