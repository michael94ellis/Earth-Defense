using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Weapon
{
    void FireAt(Vector3 target);
    IEnumerator Fire();
    IEnumerator Recharge();
}

public class LaserTurret : MonoBehaviour, Weapon
{
    private float fireDuration = 0.5f;
    private int rechargeTime = 3;
    private bool isCharged = true;
    private bool isFiring = false;
    private GameObject currentTarget;
    // Draws the laser
    private LineRenderer Laser;
    public AudioSource LaserSound;
    public AudioSource ExplosionSound;
    private Object DestructionEffect;
    private List<GameObject> Targets = new List<GameObject>();
    private SightDelegate TurretSightCone;
    public Transform BarrelPivot;

    // Start is called before the first frame update
    void Start()
    {
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
        // Laser and Explosion Sounds
        Laser = gameObject.GetComponent<LineRenderer>();
        AudioSource[] soundSources = gameObject.GetComponents<AudioSource>();
        LaserSound = soundSources[0];
        ExplosionSound = soundSources[1];
        // Visibility of Turret
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
        if (LaserSound.isPlaying)
            LaserSound.Stop();
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
        BarrelPivot.up = alienShipDirection;
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
                    FireAt(alienShip.transform.position);
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
    public void FireAt(Vector3 target)
    {
        //Debug.Log("Alien Ship In Sight");
        // This begins the laser, the FireLaser() method disables when its done firing
        if (!isFiring)
        {
            //Debug.Log("Firing Laser");
            LaserSound.Play();
            isFiring = true;
            isCharged = false;
            Laser.enabled = true;
            Laser.material.color = Color.green;
            Laser.receiveShadows = false;
            StartCoroutine(Fire());
        }
        Laser.startWidth = 1f;
        Laser.endWidth = 1f;
        Laser.SetPosition(0, transform.position);
        Laser.SetPosition(1, target);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDuration);
        if (LaserSound.isPlaying)
            LaserSound.Stop();
        currentTarget = null;
        Laser.enabled = false;
        isFiring = false;
        StartCoroutine(Recharge());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        isCharged = true;
    }
}
