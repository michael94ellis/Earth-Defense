using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShip : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;
    bool laserFired = false;
    bool isFiringLaser = false; 
    private RaycastHit currentTarget;
    GameObject city = null;
    List<GameObject> cityBuildings = new List<GameObject>();
    int targetIndex = 0;
    LineRenderer laser = null;

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
            GetCityInfo();
            if (targetIndex < cityBuildings.Count && !laserFired && cityBuildings[targetIndex] != null)
            {
                RaycastHit hit;
                Vector3 cityDirection = cityBuildings[targetIndex].transform.position - transform.position;
                if (Physics.Raycast(transform.position, cityDirection, out hit))
                {
                    Debug.Log("Did Hit" + hit.transform.gameObject);
                    if (hit.transform.IsChildOf(city.transform))
                    {
                        FireLaserAtCity(hit);
                    }
                    else
                    {
                        Debug.Log("No City In Sight");
                        laser.enabled = false;
                    }
                }
                else
                {
                    Debug.Log("Did not Hit");
                }
            }
            else if (cityBuildings[targetIndex] != null && isFiringLaser)
            {
                FireLaserAtCity(currentTarget);
            }
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(transform.position, earth.transform.position, 0.1f);
        }
    }

    private void GetCityInfo()
    {
        if (city == null)
        {
            city = GameObject.Find("City");
            laser = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
            foreach (Transform child in city.transform)
            {
                cityBuildings.Add(child.gameObject);
            }
        }
    }

    private void FireLaserAtCity(RaycastHit hit)
    {
        Debug.Log("City In Sight");
        currentTarget = hit;
        laser.enabled = true;
        laser.material.color = Color.yellow;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, cityBuildings[targetIndex].transform.position);
        if (!laserFired)
        {
            // Fire Laser
            StartCoroutine(LaserWasFired());
        }
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    IEnumerator LaserWasFired()
    {
        laserFired = true;
        //yield on a new YieldInstruction to wait
        yield return new WaitForSeconds(4);
        targetIndex++;
        laserFired = false;
        //After we have waited 5 seconds 
    }

    /// Must be called like so: StartCoroutine(FiringLaser());
    IEnumerator FiringLaser()
    {
        isFiringLaser = true;
        yield return new WaitForSeconds(1);
        Destroy(currentTarget.transform.gameObject);
        isFiringLaser = false;
    }
}
