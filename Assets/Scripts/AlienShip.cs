using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienShip : MonoBehaviour, Damageable, LaserGun // LaserGun is declared in LaserTurret right now
{
    private float moveSpeed = 0.1f;
    private float fireDuration = 0.5f;
    private int rechargeTime = 4;
    private bool isCharged = true;
    private bool isFiring = false;
    private GameObject currentTarget;
    // Draws the laser
    private LineRenderer Laser;
    public AudioSource LaserSound;
    public AudioSource ExplosionSound;

    float orbitDistance = 20;
    Vector3 currentOrbit;
    Vector3 currentOrbit2;
    Vector3[] orbits = new Vector3[] { Vector3.left, Vector3.right, Vector3.back, Vector3.forward, Vector3.up, Vector3.down };

    private Object DestructionEffect;
    private GameObject earth;

    // Start is called before the first frame update
    void Start()
    {
        currentOrbit = orbits[Random.Range(0, orbits.Length - 1)];
        Health = 100;
        //Find where to go
        earth = GameObject.Find("Earth");
        orbitDistance += earth.transform.localScale.x;
        transform.LookAt(earth.transform.position);
        Laser = gameObject.GetComponent<LineRenderer>();
        AudioSource[] soundSources = gameObject.GetComponents<AudioSource>();
        LaserSound = soundSources[0];
        ExplosionSound = soundSources[1];
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
    }

    public int Health { get; set; }
    public void TakeDamage()
    {
        Health--;
        currentOrbit = orbits[Random.Range(0, orbits.Length - 1)];
        currentOrbit2 = orbits[Random.Range(0, orbits.Length - 1)];
        if (Health == 0)
        {
            ExplosionSound.Play();
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            DestructionAnimation.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            AlienSpawner.RemovAlien(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Determine how far earth's center(0,0,0) is
        float distanceToEarth = Vector3.Distance(earth.transform.position, transform.position);
        if (distanceToEarth < orbitDistance)
        {
            // Orbit the earth
            transform.RotateAround(earth.transform.position, currentOrbit, 30f * Time.deltaTime);
            transform.RotateAround(earth.transform.position, currentOrbit2, 30f * Time.deltaTime);
            // Animation for the laser while its bein fired
            if (isFiring && currentTarget != null)
            {
                CheckLineOfSight(currentTarget);
                return;
            }
            if (LaserSound.isPlaying)
                LaserSound.Stop();
            foreach (GameObject earthObject in Earth.Children)
            {
                // If the laser is done firing we have to wait for it to recharge to fire again
                if (isCharged)
                {
                    //Debug.Log("Laser Turret Beginning Fire Sequence");
                    // Set current target in case we can shoot it
                    currentTarget = earthObject;
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
        if (Physics.Raycast(transform.position, alienShipDirection, out hit, 35))
        {
            // An object is seen, is it what we want?
            //Debug.Log("Can See Object " + hit.collider.gameObject);
            // Don't shoot other stuff
            if (earthObject == null)
                return false;
            if (hit.collider.gameObject.tag == "Human")
            {
                Damageable humanObject = earthObject.GetComponent<Damageable>();
                if (humanObject == null)
                    return false;
                humanObject.TakeDamage();
                FireLaserAt(earthObject.transform.position);
                return true;
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
        if (!isFiring)
        {
            //Debug.Log("Firing Laser");
            StartCoroutine(FireLaser());
            LaserSound.Play();
            Laser.enabled = true;
            isFiring = true;
            isCharged = false;
        }
        Laser.receiveShadows = false;
        Laser.material.color = Color.red;
        Laser.startWidth = 0.05f;
        Laser.endWidth = 0.005f;
        Laser.SetPosition(0, transform.position);
        Laser.SetPosition(1, target);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(fireDuration);
        if (LaserSound.isPlaying)
            LaserSound.Stop();
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
