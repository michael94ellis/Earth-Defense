using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienShip : MonoBehaviour, Damageable, Weapon // LaserGun is declared in LaserTurret right now
{
    private float moveSpeed = 80f;
    private float fireDuration = 0.5f;
    private int rechargeTime = 4;
    private bool isCharged = true;
    private bool isFiring = false;
    private GameObject currentTarget;
    private bool currentlyAboveTarget = false;
    // A point on the path of the circular orbit the alien will use which determines the radius of the orbit over the target
    public Vector3 targetOrbit;
    // Draws the laser
    private LineRenderer Laser;
    public AudioSource LaserSound;
    public AudioSource ExplosionSound;

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        Laser = gameObject.GetComponent<LineRenderer>();
    }

    public int Health { get; set; }
    public bool TakeDamage(int amount = 1)
    {
        if (Health <= 0)
        {
            ExplosionSound.Play();
            AlienSpawner.RemovAlien(gameObject);
            return false;
        }
        Health = Health - amount;
        return true;
    }

    void Update()
    {
        transform.LookAt(Vector3.zero);
        // Determine how far earth's center(0,0,0) is
        if (currentTarget != null && currentlyAboveTarget && Time.timeScale > 0)
        {
            PerformAttackManuevers();
        }
        else if (currentTarget == null && Time.timeScale > 0)
        {
            SelectNewTarget();
        }
        else if (Time.timeScale > 0)
        {
            MoveTowardsTarget();
        }
    }

    void SelectNewTarget()
    {
        // This is for when multiple zones exist, just to serve as a reminder
        //foreach (EarthZone zone in Earth.zones)
        //{
        if (Earth.Zone1.ShieldHealth > 0)
        {
            currentTarget = Earth.Zone1.gameObject;
        }
        else if (Earth.Zone1.Population > 0)
        {
            currentTarget = Earth.Zone1.Capitol.gameObject;
        }
        //}

        // Pick a psuedo random orbit, a point that is nearby a point above the target
        targetOrbit = Vector3.Scale(currentTarget.transform.position * 2, currentTarget.transform.up);
        Debug.Log(currentTarget.transform.position * 2);
        Debug.Log(targetOrbit);
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetOrbit, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(targetOrbit, transform.position) == 0)
        {
            currentlyAboveTarget = true;
        }
    }

    void PerformAttackManuevers()
    {
        // Orbit around a point above the target
        transform.RotateAround(currentTarget.transform.position, currentTarget.transform.position * 2, 30f * Time.deltaTime);
        // Animation for the laser while its being fired
        if (isFiring && currentTarget != null)
        {
            // If the target can be seen then rotate above it
            if (CheckLineOfSight())
                return;
        }
        if (LaserSound.isPlaying)
            LaserSound.Stop();
        if (isCharged && currentTarget != null)
        {
            if (CheckLineOfSight())
                return;
            else
                SelectNewTarget();
        }
        //Debug.Log("Looking for new Enemy ");
        // TODO Look for a new target to shoot at
    }

    public bool CheckLineOfSight()
    {
        // Determine if there is line of sight to the alien ship
        RaycastHit hit;
        Vector3 alienShipDirection = currentTarget.transform.position - transform.position;
        if (Physics.Raycast(transform.position, alienShipDirection, out hit))
        {
            // An object is seen, is it what we want?
            //Debug.Log("Can See Object " + hit.collider.gameObject);
            // Don't shoot other stuff
            if (currentTarget == null)
                return false;
            if (hit.collider.gameObject == currentTarget)
            {
                Damageable attackTarget = currentTarget.gameObject.GetComponent<Damageable>();
                if (attackTarget == null)
                    return false;
                if (!attackTarget.TakeDamage(1000))
                {
                    FireAt(currentTarget.transform.position);
                    currentTarget = null;
                    return false;
                }
                else
                {
                    FireAt(currentTarget.transform.position);
                    return true;
                }
            }
        }
        else
        {
            //Debug.Log("Can Not See Alien Ship");
        }
        return false;
    }

    public void FireAt(Vector3 target)
    {
        //Debug.Log("City In Sight");
        if (!isFiring)
        {
            // Cancel the orbit changing repeater
            CancelInvoke();
            //Debug.Log("Firing Laser");
            StartCoroutine(Fire());
            LaserSound.Play();
            Laser.enabled = true;
            isFiring = true;
            isCharged = false;
        }
        Laser.SetPosition(0, transform.position);
        Laser.SetPosition(1, target);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDuration);
        if (LaserSound.isPlaying)
            LaserSound.Stop();
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

    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(min, max);
        var z = Random.Range(min, max);
        return new Vector3(x, y, z);
    }
}
