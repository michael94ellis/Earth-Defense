using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public interface MissileSpec
{
    string name { get; }
    float launchSpeed { get; set; }
    float moveSpeed { get; set; }
    float damage { get; set; }
}

public class MissileSilo : MonoBehaviour, Weapon, ZoneBuilding, MenuDisplayItem
{
    public bool isActive { get; set; } = false;
    public Transform buildingTransform { get { return transform; } }
    public EarthZone ParentZone { get; set; }
    public ZoneBuildingType buildingType { get; set; }
    public GameObject LeftDoor;
    public GameObject RightDoor;
    public Vector3 MissileSpawnPoint;
    private float fireDuration = 2;
    private float reloadTime = 2;
    private bool isLoaded = true;
    private GameObject currentTarget;
    public GameObject Missile;
    public GameObject earth;

    public string Title { get { return "Missile Silo"; } }
    public string InfoText
    {
        get
        {
            return "Reload Time: " + reloadTime + "\n" +
                "Fire Time: " + fireDuration + "\n" +
                "Missile Type: " +  "n/a";
        }
    }

    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();

    void DecreaseReloadTime()
    {
        reloadTime *= 0.9f;
    }

    void Start()
    {
        upgrades.Add(new BuildingUpgrade("Decrease Reload Time", DecreaseReloadTime));
        earth = GameObject.Find("Earth");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;
        // If the missile launcher is done firing we have to wait for it to reload to fire again
        if (!isLoaded)
            return;
        if (currentTarget != null && CheckLineOfSight(currentTarget) && currentTarget.activeInHierarchy)
            return;
        foreach (GameObject alienShip in AlienSpawner.ActiveAliens.OrderBy(a => Random.value).ToList())
        {
            //Debug.Log("Laser Turret Beginning Fire Sequence");
            // Check for any sight obstructions to the alien ship
            if (alienShip != null && CheckLineOfSight(alienShip) && !ParentZone.ActiveTargets.Contains(alienShip))
            {
                currentTarget = alienShip;
                ParentZone.ActiveTargets.Add(alienShip);
            }
            return;
        }
    }

    public bool CheckLineOfSight(GameObject alienShip)
    {
        //Debug.Log("checking for sight");
        // Determine if there is line of sight to the alien ship
        if ((alienShip.transform.position - transform.position).magnitude < 1000f) // pop pop
        {
            // Begin animating laser
            //Debug.Log("Aiming Turret at: " + hit.transform.gameObject);
            FireAt(alienShip.transform.position);
                return true;
        }
        return false;
    }

    IEnumerator RotateDoor(Transform door, Vector3 byAngles, float inTime)
    {
        var fromAngle = door.rotation;
        var toAngle = Quaternion.Euler(door.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            door.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    public void FireAt(Vector3 target)
    {
        // Open Doors
        StartCoroutine(RotateDoor(RightDoor.transform, new Vector3(-90, 0, 0), 1f));
        StartCoroutine(RotateDoor(LeftDoor.transform, new Vector3(-90, 0, 0), 1f));
        StartCoroutine(Fire());
    }

    public IEnumerator Fire()
    {
        isLoaded = false;
        yield return new WaitForSeconds(fireDuration);
        // Fire Missile 
        GameObject newMissile = Instantiate(Missile, earth.transform);
        newMissile.transform.localScale = new Vector3(0.1f, 0.25f, 0.1f);
        Missile missileScript = newMissile.GetComponent<Missile>();
        if (missileScript != null)
        {
            missileScript.target = currentTarget;
            newMissile.transform.position = transform.position;
        }
        ParentZone.ActiveTargets.Remove(currentTarget);
        currentTarget = null;
        StartCoroutine(Recharge());
    }

    public IEnumerator Recharge()
    {
        // Close doors
        StartCoroutine(RotateDoor(RightDoor.transform, new Vector3(90, 0, 0), 1f));
        StartCoroutine(RotateDoor(LeftDoor.transform, new Vector3(-90, 0, 0), 1f));
        yield return new WaitForSeconds(reloadTime);
        isLoaded = true;
        // Load new missile(or reuse from pool)
    }
}
