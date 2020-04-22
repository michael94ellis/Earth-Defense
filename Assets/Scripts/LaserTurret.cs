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
    private float fireDuration = 0.5f;
    private float firingRange = 15.0f;
    private int rechargeTime = 3;
    private bool isCharged;
    private bool isFiring;
    private GameObject currentTarget;
    // Draws the laser
    private LineRenderer Laser;
    private List<GameObject> Targets;
    private SightDelegate TurretSightCone;

    public int Health { get; private set; }
    public void TakeDamage()
    {
        Debug.Log("Damage");
        Health--;
        if (Health == 0)
        {
            int explosionNumber = Random.Range(1, 10);
            Debug.Log("Explosion" + explosionNumber);
            Object DestructionEffect = Resources.Load("Explosion" + explosionNumber);
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 120;
        Targets = new List<GameObject>();
        Laser = gameObject.GetComponent<LineRenderer>();
        isFiring = false;
        isCharged = true;
        TurretSightCone = transform.Find("SightCone").GetComponent<SightDelegate>();
        if (TurretSightCone != null)
        {
            TurretSightCone.EnteredSight += AddTarget;
            TurretSightCone.ExitedSight += RemoveTarget;
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
        // Animation for the laser while its bein fired
        if (isFiring && currentTarget != null)
        {
            CheckLineOfSight(currentTarget);
            return;
        }
        Targets.RemoveAll(item => item == null);
        foreach (GameObject alienShip in Targets)
        {
            // If the laser is done firing we have to wait for it to recharge to fire again
            if (isCharged)
            {
                //Debug.Log("Laser Turret Beginning Fire Sequence");
                // Set current target in case we can shoot it
                // Check for any sight obstructions to the alien ship
                if (CheckLineOfSight(alienShip))
                    return;
            }
        }
    }

    public bool CheckLineOfSight(GameObject alienShip)
    {
        // Determine if there is line of sight to the alien ship
        RaycastHit hit;
        Vector3 alienShipDirection = alienShip.transform.position - transform.position;
        Vector3 barrelTip = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        if (Physics.Raycast(barrelTip, alienShipDirection, out hit))
        {
            // An object is seen, is it an alien ship?
            //Debug.Log("Turret Can See Object " + hit.transform.gameObject);
            // Don't shoot other stuff
            if (hit.transform.tag == "Alien")
            {
                // Begin animating laser
                //Debug.Log("Aiming Turret at: " + hit.transform.gameObject);
                AlienShip alienScript = hit.transform.gameObject.GetComponent<AlienShip>();
                if (alienScript != null)
                {
                    //Debug.Log("Firing");
                    currentTarget = alienShip;
                    alienScript.TakeDamage();
                    FireLaserAt(alienShip.transform.position);
                    return true;
                }
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

    /// This animates the laser firing
    public void FireLaserAt(Vector3 target)
    {
        //Debug.Log("Alien Ship In Sight");
        // This begins the laser, the FireLaser() method disables when its done firing
        if (!isFiring)
        {
            //Debug.Log("Firing Laser");
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
        currentTarget = null;
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
