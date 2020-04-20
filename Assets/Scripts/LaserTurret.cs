using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LaserGun
{
    void AddTarget(GameObject target);
    void RemoveTarget(GameObject alienShip);
    void FireLaserAt(Vector3 target);
    IEnumerator FireLaser();
    IEnumerator RechargeLaser();
}

public class LaserTurret : MonoBehaviour, LaserGun
{
    SightCone TurretSightArea;
    float fireDuration = 0.5f;
    float firingRange = 15.0f;
    int rechargeTime = 3;
    bool isFiring;
    bool isCharged;
    // Draws the laser
    private LineRenderer Laser;
    private List<GameObject> Targets;

    // Start is called before the first frame update
    void Start()
    {
        Targets = new List<GameObject>();
        Laser = gameObject.GetComponent<LineRenderer>();
        isFiring = false;
        isCharged = true;
        TurretSightArea = transform.Find("SightCone").GetComponent<SightCone>();
        if (TurretSightArea != null)
        {
            TurretSightArea.EnteredSight += AddTarget;
            TurretSightArea.ExitedSight += RemoveTarget;
        }
        else
        {
            Debug.Log("Error: Failed to get Cone Of Sight for LaserTurret");
        }
    }

    public void AddTarget(GameObject alienShip)
    {
        Targets.Add(alienShip);
    }

    public void RemoveTarget(GameObject alienShip)
    {
        Targets.Remove(alienShip);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject alienShip in Targets)
        {
            // Find how far the ship is
            float distanceToAlienShip = Vector3.Distance(alienShip.transform.position, transform.position);
            // TODO Add turret aiming animation
            if (distanceToAlienShip > firingRange)
                return;
            // Make sure this isn't a dead ship
            if (alienShip == null)
            {
                // Update our list if we found a dead ship and try again
                Debug.Log("Dead Alien Confirmed");
                return;
            }
            if (distanceToAlienShip < firingRange)
            {
                // Animation for the laser while its bein fired
                if (isFiring)
                {
                    FireLaserAt(alienShip.transform.position);
                    return;
                }
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (isCharged)
                {
                    Debug.Log("Laser Turret Beginning Fire Sequence");
                    CheckLineOfSight(alienShip);
                    return;
                }
            }
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
            Debug.Log("Can See Object " + hit.transform.gameObject);
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
                Debug.Log("Alien Ship Not In Sight");
            }
        }
        else
        {
            Debug.Log("Can Not See Alien Ship");
        }
    }

    /// This animates the laser firing
    public void FireLaserAt(Vector3 target)
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
        Laser.startWidth = 0.025f;
        Laser.endWidth = 0.025f;
        Laser.SetPosition(0, transform.position);
        Laser.SetPosition(1, target);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(fireDuration);
        Laser.enabled = false;
        isFiring = false;
        StartCoroutine(RechargeLaser());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator RechargeLaser()
    {
        yield return new WaitForSeconds(rechargeTime);
        isCharged = true;
    }
}
