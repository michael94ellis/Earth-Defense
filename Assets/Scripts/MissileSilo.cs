using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSilo : MonoBehaviour, Weapon
{

    public GameObject LeftDoor;
    public GameObject RightDoor;
    private int fireDuration = 3;
    private int reloadTime = 1;
    private bool isLoaded = true;
    private GameObject currentTarget;
    public Vector3 MissileSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
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
                return;
            }
        }
    }

    public bool CheckLineOfSight(GameObject alienShip)
    {
        //Debug.Log("checking for sight");
        // Determine if there is line of sight to the alien ship
        Vector3 barrelTip = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Vector3 alienShipDirection = alienShip.transform.position - barrelTip;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(barrelTip, alienShipDirection, 500f);
        foreach (RaycastHit hit in hits)
        {
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
        return false;
    }

    public void FireAt(Vector3 target)
    {
        isLoaded = false;
        StartCoroutine(Fire());
    }

    public IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDuration);
    }

    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(reloadTime);
        isLoaded = true;
    }
}
