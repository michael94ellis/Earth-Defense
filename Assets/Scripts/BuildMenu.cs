using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public GameObject Panel;
    private bool isPickingLocation = false;
    Earth earth;
    Object CityRef;
    Object GeneratorRef;
    Object MissileSiloRef;
    Object LaserTurretRef;
    //Object SatelliteRef;
    GameObject PurchasedZoneBuilding;
    EarthZone selectedZone;
    // Cached list of colliders fetched/reset every time a ZoneBuilding is bought
    Dictionary<Collider, EarthZone> ControlledZoneColliders = new Dictionary<Collider, EarthZone>();

    void Start()
    {
        //needed to initialize earth at start
        earth = GameObject.Find("Earth").GetComponent<Earth>();
        CityRef = Resources.Load("City");
        LaserTurretRef = Resources.Load("Turret");
        MissileSiloRef = Resources.Load("MissileSilo");
        GeneratorRef = Resources.Load("Generator");
        //SatelliteRef = Resources.Load("EarthSatellite");
    }

    void Update()
    {
        //previews placement of new building until clicked
        if (isPickingLocation)
        {
            RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
            foreach (RaycastHit hit in hitsInOrder)
            {
                // This hit.point is the point on earth where you clicked
                if (selectedZone == null && ControlledZoneColliders.Keys.Contains(hit.collider))
                {
                    if (PurchasedZoneBuilding.GetComponent<ShieldGenerator>() != null && ControlledZoneColliders[hit.collider].ShieldGenerator != null)
                    {
                        return;
                    }
                    else
                    {
                        Debug.Log("selected zone");
                        selectedZone = ControlledZoneColliders[hit.collider];
                        PurchasedZoneBuilding.GetComponent<ZoneBuilding>().ParentZone = selectedZone;
                        // Put it in the coord space of the earthzone
                        PurchasedZoneBuilding.transform.SetParent(selectedZone.transform, true);
                    }
                }
                else if (selectedZone != null && hit.transform.tag == "Earth" && selectedZone.GetComponent<Collider>().bounds.Contains(hit.point))
                {
                    Debug.Log("selecting spot");
                    // assign the up vector for the city so it the top of it faces away from earth and the bottom sits on the planet
                    PurchasedZoneBuilding.transform.up = hit.point * 2;
                    // set the position of this newly purchased building to the place where the mouse is
                    PurchasedZoneBuilding.transform.position = hit.point;
                    // If the user clicks while the newly purchased zone building is being displayed
                    if (Input.GetMouseButton(0))
                    {
                        SetZoneCollidersEnabled(false);
                        isPickingLocation = false;
                        selectedZone = null;
                    }
                    return;
                }
            }
        }
    }

    //-----Open Build Menu-------
    public void OpenMenu()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    //-----New Weapon ZoneBuildings-----
    public void BuyLaserTurret()
    {
        GameObject NewWeapon = Instantiate(LaserTurretRef) as GameObject;
        HandleNewObject(NewWeapon, Vector3.one);
    }


    public void BuyMissileSiloButton()
    {
        GameObject NewWeapon = Instantiate(MissileSiloRef) as GameObject;
        HandleNewObject(NewWeapon, Vector3.one);
    }
    //--------End Weapons--------------

    //-----New Defensive ZoneBuildings-----
    public void BuyMinorCity()
    {
        GameObject newCity = Instantiate(CityRef) as GameObject;
        HandleNewObject(newCity, Vector3.one * 0.25f);
    }


    public void BuyShieldGenerator()
    {
        GameObject NewShieldGenerator = Instantiate(GeneratorRef) as GameObject;
        HandleNewObject(NewShieldGenerator, Vector3.one);
    }
    //--------End Defenses--------------

    //turn colliders on for zones
    public void SetZoneCollidersEnabled(bool isEnabled)
    {
        foreach (Collider zoneCollider in ControlledZoneColliders.Keys)
        {
            zoneCollider.enabled = isEnabled;
        }
    }

    //sends new aliens
    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }


    //-----Build Objects------
    void HandleNewObject(GameObject newObj, Vector3 scale)
    {
        // Let the user select a spot for the new zone building
        isPickingLocation = true;
        // Set the object to this variable so we can display it in the Update method while the user picks a location
        PurchasedZoneBuilding = newObj;
        // Make the new city a child object so it lives inside the earth's coordinate space
        PurchasedZoneBuilding.transform.SetParent(transform, true);
        // Set the scale passed in by the Instantiating function
        PurchasedZoneBuilding.transform.localScale = scale;
        // Reset the cached list of colliders just in case they changed
        ControlledZoneColliders = new Dictionary<Collider, EarthZone>();
        // Fetch the user's controlled earth zone colliders
        foreach (EarthZone controlledZone in Earth.ControlledZones)
        {
            ControlledZoneColliders.Add(controlledZone.GetComponent<Collider>(), controlledZone);
        }
        // Enabled them so we can place the object
        SetZoneCollidersEnabled(true);
    }
}
