using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienShip : MonoBehaviour, Damageable, Weapon // LaserGun is declared in LaserTurret right now
{
    private float moveSpeed = 80f;
    public int AttackDamage = 100;
    private float fireDuration = 0.5f;
    private int rechargeTime = 4;
    private bool isCharged = true;
    private bool isFiring = false;
    private GameObject currentTarget;
    private bool currentlyAboveTarget = false;
    public LineRenderer Laser;
    public AudioSource LaserSound;
    public AudioSource ExplosionSound;
    public GameObject HealthBar;

    public float MaxHealth { get; private set; } = 100;
    private float _Health = 100;
    public float Health
    {
        get
        {
            return _Health;
        }
        private set
        {
            if (value <= 0)
                _Health = 0;
            else if (value > _Health)
                _Health = MaxHealth;
            else
                _Health = value;
        }
    }

    public bool TakeDamage(int amount = 1)
    {
        Health -= amount;
        if (Health <= 0)
        {
            ExplosionSound.Play();
            AlienSpawner.RemovAlien(gameObject);
            return false;
        }
        UpdateHealthbar();
        return true;
    }

    void UpdateHealthbar()
    {
        RectTransform healthBarRect = HealthBar.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2((Health / MaxHealth) * 5, healthBarRect.rect.height);
    }

    void Start()
    {
        UpdateHealthbar();
    }

    void Update()
    {
        if (Time.timeScale <= 0)
            return;
        transform.LookAt(Vector3.zero);
        // Determine how far earth's center(0,0,0) is
        if (currentTarget != null && currentlyAboveTarget)
        {
            PerformAttackManuevers();
        }
        else if (currentTarget == null )
        {
            SelectNewTarget();
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    void SelectNewTarget()
    {
        // This is for when multiple zones exist, just to serve as a reminder
        foreach (EarthZone zone in Earth.ControlledZones.OrderBy(a => Random.value).ToList())
        {
            if (zone.ShieldHealth > 0)
            {
                currentTarget = zone.gameObject;
                return;
            }
            else if (zone.Population > 0)
            {
                currentTarget = zone.Capitol.gameObject;
                return;
            }
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.Slerp(transform.position, currentTarget.transform.position * 2, Time.deltaTime);
        if (Vector3.Distance(currentTarget.transform.position * 2, transform.position) < 40)
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
            if (currentTarget == null)
                return false;
            if (hit.collider.gameObject == currentTarget)
            {
                Damageable attackTarget = currentTarget.gameObject.GetComponent<Damageable>();
                if (attackTarget == null)
                    return false;
                bool attackSuccess = attackTarget.TakeDamage(AttackDamage);
                FireAt(currentTarget.transform.position);
                if (!attackSuccess)
                    currentTarget = null;
                return attackSuccess;
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

    public void Reset()
    {
        Health = MaxHealth;
        isCharged = true;
        isFiring = false;
        currentTarget = null;
        currentlyAboveTarget = false;
    }
}
