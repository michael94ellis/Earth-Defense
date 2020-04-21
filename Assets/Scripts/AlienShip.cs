using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShip : MonoBehaviour, LaserGun // LaserGun is declared in LaserTurret right now
{
    private Vector3 axis = new Vector3(0,1,0);
    private float rotationSpeed = 80.0f;
    private float moveSpeed = 0.1f;
    private bool isLaserFiring = false;
    private GameObject currentLaserTarget;
    private bool isLaserCharged = true;
    private int laserRechargeTime = 4;
    private float fireDuration = 0.5f;
    private SightDelegate AlienSightSphere;
    private List<GameObject> Targets = new List<GameObject>();
    private GameObject earth;
    private LineRenderer laser;

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
        transform.LookAt(earth.transform.position);
        laser = gameObject.GetComponent<LineRenderer>();
        AlienSightSphere = transform.Find("SightSphere").GetComponent<SightDelegate>();
        if (AlienSightSphere != null)
        {
            AlienSightSphere.EnteredSight += AddTarget;
            AlienSightSphere.ExitedSight += RemoveTarget;
        }
        else
        {
            Debug.Log("Error: Failed to get Cone Of Sight for LaserTurret");
        }
    }

    public void AddTarget(GameObject otherObject)
    {
        if (otherObject.tag == "City")
        {
            Targets.Add(otherObject);
        } else
        {
            //Debug.Log(otherObject.tag);
        }
    }

    public void RemoveTarget(GameObject otherObject)
    {
        if (otherObject.tag == "City")
        {
            Targets.Remove(otherObject);
        }
        else
        {
            //Debug.Log(otherObject.tag);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Determine how far earth's center(0,0,0) is
        float distanceToEarth = Vector3.Distance(earth.transform.position, transform.position);

        if (distanceToEarth < 4)
        {
            // Orbit the earth
            transform.RotateAround(earth.transform.position, axis, rotationSpeed * Time.deltaTime);
            // Animation for the laser while its bein fired
            if (isLaserFiring)
            {
                CheckLineOfSight(currentLaserTarget);
                return;
            }
            foreach (GameObject alienShip in Targets)
            {
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (isLaserCharged)
                {
                    Debug.Log("Laser Turret Beginning Fire Sequence");
                    // Set current target in case we can shoot it
                    currentLaserTarget = alienShip;
                    // Check for any sight obstructions to the alien ship
                    CheckLineOfSight(alienShip);
                    return;
                }
            }
        }
        else if (Time.timeScale > 0)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, earth.transform.position, moveSpeed);
        }
    }

    public void CheckLineOfSight(GameObject alienShip)
    {
        // Determine if there is line of sight to the alien ship
        RaycastHit hit;
        Vector3 alienShipDirection = alienShip.transform.position - transform.position;
        Vector3 barrelTip = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        if (Physics.Raycast(barrelTip, alienShipDirection, out hit))
        {
            // An object is seen, is it an alien ship?
            //Debug.Log("Can See Object " + hit.transform.gameObject);
            // Don't shoot other stuff
            if (hit.transform.tag == "Alien")
            {
                // Begin animating laser
                FireLaserAt(alienShip.transform.position);
                return;
            }
            else
            {
                // Something is in the way
                //Debug.Log("Alien Ship Not In Sight");
            }
        }
        else
        {
            //Debug.Log("Can Not See Alien Ship");
        }
    }

    public void FireLaserAt(Vector3 target)
    {
        //Debug.Log("City In Sight");
        if (!isLaserFiring)
        {
            //Debug.Log("Firing Laser");
            StartCoroutine(FireLaser());
            laser.enabled = true;
            isLaserFiring = true;
            isLaserCharged = false;
        }
        laser.receiveShadows = false;
        laser.material.color = Color.red;
        laser.startWidth = 0.05f;
        laser.endWidth = 0.005f;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, target);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(fireDuration);
        laser.enabled = false;
        isLaserFiring = false;
        StartCoroutine(RechargeLaser());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator RechargeLaser()
    {
        yield return new WaitForSeconds(laserRechargeTime);
        isLaserCharged = true;
    }
}
