using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserTurret : MonoBehaviour, Weapon, ZoneBuilding, MenuDisplayItem
{
    public bool isActive { get; set; } = false;
    public Transform buildingTransform { get { return transform; } }
    public EarthZone ParentZone { get; set; }
    public ZoneBuildingType buildingType { get; set; }
    public int _PowerCost;
    public int PowerCost { get => _PowerCost; set => _PowerCost = value; }
    public int _PopulationCost;
    public int PopulationCost { get => _PopulationCost; set => _PopulationCost = value; }

    private float fireDuration = 0.5f;
    private float rechargeTime = 1;
    private bool isCharged = true;
    private bool isFiring = false;
    private GameObject currentTarget;
    // Draws the laser
    private LineRenderer Laser;
    public Transform BarrelPivot;
    public Transform BarrelTip;

    public string Title { get { return "Laser Turret"; } }
    public string InfoText
    {
        get
        {
            return "Recharge Time: " + rechargeTime + "\n" +
                "Fire Time: " + fireDuration + "\n" +
                "Bonus Effect: " + "n/a";
        }
    }

    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();

    void DecreaseRechargeTime()
    {
        rechargeTime *= 0.9f;
    }

    void Start()
    {
        upgrades.Add(new BuildingUpgrade("Decrease Recharge Time", DecreaseRechargeTime));
        Laser = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!isActive)
            return;
        if (currentTarget != null)
            BarrelPivot.up = currentTarget.transform.position - BarrelPivot.transform.position;
        // Animation for the laser while its bein fired
        if (isFiring && currentTarget != null)
        {
            CheckLineOfSight(currentTarget);
            return;
        }
        // If the laser is done firing we have to wait for it to recharge to fire again
        if (!isCharged)
            return;
        if (currentTarget != null && CheckLineOfSight(currentTarget))
            return;
        foreach (GameObject alienShip in AlienSpawner.ActiveAliens.OrderBy(a => Random.value).ToList())
        {
            //Debug.Log("Laser Turret Beginning Fire Sequence");
            // Check for any sight obstructions to the alien ship
            if (alienShip.activeInHierarchy && CheckLineOfSight(alienShip))
            {
                return;
            }
        }
    }

    public bool CheckLineOfSight(GameObject alienShip)
    {
        //Debug.Log("checking for sight");
        // Determine if there is line of sight to the alien ship
        Vector3 alienShipDirection = alienShip.transform.position - BarrelPivot.transform.position;
        bool hitEarth = false;
        AlienShip alienScript = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(BarrelPivot.transform.position, alienShipDirection, 100f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Earth")
                hitEarth = true;
            if (hit.transform.tag == "Alien")
                alienScript = hit.transform.gameObject.GetComponent<AlienShip>();
        }
        if (!hitEarth)
        {
            currentTarget = alienShip;
            if (alienScript != null && alienScript.TakeDamage())
                FireAt(alienShip.transform.position);
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
            isFiring = true;
            isCharged = false;
            Laser.enabled = true;
            StartCoroutine(Fire());
        }
        Laser.SetPosition(0, BarrelTip.position);
        Laser.SetPosition(1, target);
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDuration);
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
