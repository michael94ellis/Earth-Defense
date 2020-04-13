using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShip : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;
    bool isFiringLaser = false; 
    GameObject city = null;
    List<GameObject> cityBuildings = new List<GameObject>();
    LineRenderer laser = null;
    private float hitLast = 0;
    private float hitDelay = 5;

    private GameObject earth;

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
        transform.LookAt(earth.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector3.forward * Time.deltaTime);
        float distanceToEarth = Vector3.Distance(earth.transform.position, transform.position);

        if (distanceToEarth < 10)
        {
            transform.RotateAround(earth.transform.position, axis, rotationSpeed * Time.deltaTime);
            // Alien ship scans city to find all buildings to destroy

            if (city == null)
            {
                GetCityInfo();
            }
            if (isFiringLaser)
            {
                return;
            }
            foreach (GameObject building in cityBuildings)
            {
                RaycastHit hit;
                Vector3 cityDirection = building.transform.position - transform.position;
                if (Physics.Raycast(transform.position, cityDirection, out hit))
                {
                    Debug.Log("Did Hit" + hit.transform.gameObject);
                    if (hit.transform.IsChildOf(city.transform))
                    {
                        FireLaserAtCity(building.transform.position);
                        GameObject destroyedBuilding = building;
                        cityBuildings.Remove(building);
                        Destroy(destroyedBuilding);
                        break;
                    }
                    else
                    {
                        Debug.Log("Building Not In Sight");
                        laser.enabled = false;
                    }
                }
                else
                {
                    Debug.Log("Did not Hit");
                }
            }
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(transform.position, earth.transform.position, 0.1f);
        }
    }

    private void GetCityInfo()
    {
        city = GameObject.Find("City");
        laser = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        foreach (Transform child in city.transform)
        {
            cityBuildings.Add(child.gameObject);
        }
    }

    private void FireLaserAtCity(Vector3 target)
    {
        Debug.Log("City In Sight");
        laser.enabled = true;
        laser.material.color = Color.yellow;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, target);
        if (!isFiringLaser)
        {
            StartCoroutine(FireLaserAt());
        }
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator FireLaserAt()
    {
        isFiringLaser = true;
        //yield on a new YieldInstruction to wait
        yield return new WaitForSeconds(2);
        isFiringLaser = false;
        hitLast = Time.time;
    }
}
