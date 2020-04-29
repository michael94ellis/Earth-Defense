using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MissileSilo : MonoBehaviour, Weapon
{
    public GameObject LeftDoor;
    public GameObject RightDoor;
    public Vector3 MissileSpawnPoint;
    private int fireDuration = 2;
    private int reloadTime = 2;
    private bool isLoaded = true;
    private GameObject currentTarget;
    bool doorsOpen = false;

    public GameObject Missile;
    public GameObject earth;

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
    }

    // Update is called once per frame
    void Update()
    {
        // If the missile launcher is done firing we have to wait for it to reload to fire again
        if (!isLoaded)
            return;
        if (currentTarget != null && CheckLineOfSight(currentTarget))
            return;
        foreach (GameObject alienShip in AlienSpawner.Aliens)
        {
            //Debug.Log("Laser Turret Beginning Fire Sequence");
            // Check for any sight obstructions to the alien ship
            if (alienShip.activeInHierarchy && CheckLineOfSight(alienShip))
            {
                currentTarget = alienShip;
                return;
            }
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
        // Fire Missile 
        GameObject newMissile = Instantiate(Missile);
        Missile missileScript = newMissile.GetComponent<Missile>();
        if (missileScript != null)
        {
            newMissile.transform.SetParent(earth.transform, true);
            newMissile.transform.position = new Vector3(0, -newMissile.transform.localScale.y, 0);
            missileScript.target = currentTarget;
        }
        yield return new WaitForSeconds(fireDuration);
        // Close doors
        StartCoroutine(RotateDoor(RightDoor.transform, new Vector3(90, 0, 0), 1f));
        StartCoroutine(RotateDoor(LeftDoor.transform, new Vector3(-90, 0, 0), 1f));
        StartCoroutine(Recharge());
    }

    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(reloadTime);
        isLoaded = true;
        // Load new missile(or reuse from pool)
    }
}
