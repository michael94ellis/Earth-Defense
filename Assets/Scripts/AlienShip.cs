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
    private Object DestructionEffect;

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        Laser = gameObject.GetComponent<LineRenderer>();
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
        currentTarget = GameObject.Find("NorthAmericanShield");
        // Pick a psuedo random orbit, a point that is nearby a point above the target
        targetOrbit = currentTarget.transform.position * 2 + RandomVector(0.5f, 0.85f);
    }

    public int Health { get; set; }
    public bool TakeDamage()
    {
        if (Health == 0)
        {
            ExplosionSound.Play();
            //GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            //DestructionAnimation.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            AlienSpawner.RemovAlien(gameObject);
            return false;
        }
        Health--;
        return true;
    }

    void Update()
    {
        transform.LookAt(Vector3.zero);
        // Determine how far earth's center(0,0,0) is
        if (currentTarget != null && currentlyAboveTarget && Time.timeScale > 0)
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
            }
            Debug.Log("Looking for new Enemy ");
            // TODO Look for a new target to shoot at
        }
        else if (Time.timeScale > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetOrbit, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(targetOrbit, transform.position) == 0)
            {
                currentlyAboveTarget = true;
            }
        }
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
                Damageable attackTarget = currentTarget.GetComponent<Damageable>();
                if (attackTarget == null)
                    return false;
                if (!attackTarget.TakeDamage())
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
        Laser.receiveShadows = false;
        Laser.material.color = Color.red;
        Laser.startWidth = 0.005f;
        Laser.endWidth = 0.005f;
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
