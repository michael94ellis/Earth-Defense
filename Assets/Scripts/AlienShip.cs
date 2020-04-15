using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShip : MonoBehaviour
{
    public Vector3 axis = new Vector3(0,0,0);
    public float rotationSpeed = 80.0f;
    public float moveSpeed = 0.1f;
    bool isFiringLaser = false; 
    GameObject city = null;
    List<GameObject> cityBuildings = new List<GameObject>();
    LineRenderer laser;
    /// Laser recharge time
    private bool isLaserCharged = true;
    private int laserRechargeTime = 4;
    private float fireDuration = 0.5f;
    private Vector3 currentLaserTarget;
    private GameObject earth;
    

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
        transform.LookAt(earth.transform.position);
        laser = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        // Alien ship scans city to find all buildings to destroy
        GetCityInfo();
    }

    private void GetCityInfo()
    {
        city = GameObject.Find("City");
        foreach (Transform child in city.transform)
        {
            cityBuildings.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector3.forward * Time.deltaTime);
        float distanceToEarth = Vector3.Distance(earth.transform.position, transform.position);

        if (distanceToEarth < 10)
        {
            // Orbit the earth
            transform.RotateAround(earth.transform.position, axis, rotationSpeed * Time.deltaTime);
            // Animation for the laser
            if (isFiringLaser)
            {
                FireLaserAt(currentLaserTarget);
                return;
            }
            // There should be a recharge period for the laser
            if (isLaserCharged)
            {
                if (cityBuildings.Count == 1)
                    return;
                // Search for a target to fire laser at
                SearchForTarget();
                
            }
        }
        else if (Time.timeScale > 0)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, earth.transform.position, moveSpeed);
        }
    }

    private void SearchForTarget()
    {
        foreach (GameObject building in cityBuildings)
        {
            if (building == null || building.tag == "CityFoundation")
            {
                // Dont Destroy the Foundations of Cities
                // aliens will land and build their own alien invasion city eventually
                continue;
            }
            // Determine if there is line of sight to the building
            RaycastHit hit;
            Vector3 cityDirection = building.transform.position - transform.position;
            if (Physics.Raycast(transform.position, cityDirection, out hit))
            {
                Debug.Log("Can See Object " + hit.transform.gameObject);
                if (hit.transform.IsChildOf(city.transform))
                {
                    // Begin animating laser
                    FireLaserAt(building.transform.position);
                    // Destroy the building, move this later
                    GameObject destroyedBuilding = building;
                    cityBuildings.Remove(building);
                    Destroy(destroyedBuilding);
                    break;
                }
                else // Something is in the way
                {
                    Debug.Log("Building Not In Sight");
                    laser.enabled = false;
                }
            }
            else // Cannot see this building
            {
                Debug.Log("Did not Hit");
            }
        }
    }

    private void FireLaserAt(Vector3 target)
    {
        Debug.Log("City In Sight");
        currentLaserTarget = target;
        if (!isFiringLaser)
        {
            Debug.Log("Firing Laser");
            StartCoroutine(FireLaser());
            laser.startWidth = 0.5f;
            laser.endWidth = 0.1f;
            laser.enabled = true;
            laser.material.color = Color.yellow;
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, target);
        }
        else
        {
            laser.material.color = Color.yellow;
            laser.startWidth = 0.5f;
            laser.endWidth = 0.1f;
            laser.material.color = Color.yellow;
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, target);
        }
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
