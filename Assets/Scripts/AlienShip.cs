using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienShip : MonoBehaviour, LaserGun // LaserGun is declared in LaserTurret right now
{
    private float moveSpeed = 0.1f;

    private bool isLaserFiring = false;
    private bool isLaserCharged = true;
    private int laserRechargeTime = 4;
    private float fireDuration = 0.5f;

    private SightDelegate AlienSightSphere;
    private LineRenderer laser;
    private List<GameObject> Targets = new List<GameObject>();
    private GameObject currentLaserTarget;

    private GameObject earth;

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
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
            Debug.Log("Error: Failed to get Sight for Alien Ship");
        }
    }

    public int Health { get; private set; }
    public void TakeDamage()
    {
        Health--;
        if (Health == 0)
        {
            int explosionNumber = Random.Range(1, 10);
            Object DestructionEffect = Resources.Load("Explosion" + explosionNumber);
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
        }
    }

    public void AddTarget(GameObject otherObject)
    {
        if (otherObject.tag == "City" || otherObject.tag == "Turret")
        {
            Targets.Add(otherObject);
        }
        else
        {
            Debug.Log(otherObject + " tag " + otherObject.tag);
        }
    }

    public void RemoveTarget(GameObject otherObject)
    {
        if (otherObject.tag == "City" || otherObject.tag == "Turret")
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
        if (distanceToEarth < 3)
        {
            // Orbit the earth
            transform.RotateAround(earth.transform.position, Vector3.down, 80f * Time.deltaTime);
            // Animation for the laser while its bein fired
            Targets.RemoveAll(item => item == null);
            if (isLaserFiring && currentLaserTarget != null)
            {
                CheckLineOfSight(currentLaserTarget);
                return;
            }
            foreach (GameObject earthObject in Targets)
            {
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (isLaserCharged)
                {
                    Debug.Log("Laser Turret Beginning Fire Sequence");
                    // Set current target in case we can shoot it
                    currentLaserTarget = earthObject;
                    // Check for any sight obstructions to the thing on earth
                    if (CheckLineOfSight(earthObject))
                        return;
                }
            }
        }
        else if (Time.timeScale > 0)
        {
           transform.position = Vector3.MoveTowards(transform.position, earth.transform.position, moveSpeed);
        }
    }

    public bool CheckLineOfSight(GameObject earthObject)
    {
        // Determine if there is line of sight to the alien ship
        RaycastHit hit;
        Vector3 alienShipDirection = earthObject.transform.position - transform.position;
        Vector3 barrelTip = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        if (Physics.Raycast(barrelTip, alienShipDirection, out hit))
        {
            // An object is seen, is it an alien ship?
            Debug.Log("Can See Object " + hit.transform.gameObject);
            // Don't shoot other stuff
            if (hit.transform.gameObject == earthObject)
            {
                if (earthObject.tag == "Turret")
                {
                    LaserTurret script = earthObject.GetComponent<LaserTurret>();
                    script.TakeDamage();
                }
                if (earthObject.tag == "City")
                {
                    City script = earth.GetComponent<City>();
                    script.TakeDamage();
                }
                FireLaserAt(earthObject.transform.position);
                return true;
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
        return false;
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
