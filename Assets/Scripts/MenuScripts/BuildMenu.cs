using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface Weapon
{
    void FireAt(Vector3 target);
    IEnumerator Fire();
    IEnumerator Recharge();
}
public interface Damageable
{
    bool TakeDamage(int amount);
}

public enum ZoneBuildingType
{
    LaserTurret,
    MissileSilo,
    City,
    ShieldGenerator
}

interface ZoneBuilding
{
    bool isActive { get; set; }
    Transform buildingTransform { get; }
    EarthZone ParentZone { get; set; }
    ZoneBuildingType buildingType { get; set; }
}

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
    ZoneBuilding PurchasedZoneBuilding;
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
                // This hit.point is the point on earth where the mouse hovers
                if (selectedZone == null && ControlledZoneColliders.Keys.Contains(hit.collider))
                {
                    // This should only be reached once each time the user hovers over a zone
                    if (PurchasedZoneBuilding.buildingType == ZoneBuildingType.ShieldGenerator && ControlledZoneColliders[hit.collider].ShieldGenerator != null)
                    {
                        // Only 1 shield generator per zone
                        selectedZone = null;
                        return;
                    }
                    else
                    {
                        //Debug.Log("selected zone");
                        selectedZone = ControlledZoneColliders[hit.collider];
                        PurchasedZoneBuilding.ParentZone = selectedZone;
                        PurchasedZoneBuilding.isActive = true;
                        // Put it in the coord space of the earthzone
                        PurchasedZoneBuilding.buildingTransform.SetParent(selectedZone.transform, true);
                    }
                }
                else if (selectedZone != null && hit.transform.tag == "Earth" // user is hovering over controlled zone on earth
                    && ControlledZoneColliders.FirstOrDefault(key => key.Value == selectedZone).Key.bounds.Contains(hit.point)) // get the collider(key) based on the value(earthzone)
                {
                    //Debug.Log("selecting spot");
                    // assign the up vector for the city so it the top of it faces away from earth and the bottom sits on the planet
                    PurchasedZoneBuilding.buildingTransform.up = hit.point * 2;
                    // set the position of this newly purchased building to the place where the mouse is
                    PurchasedZoneBuilding.buildingTransform.position = hit.point;
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
        ZoneBuilding newZoneBuilding = (Instantiate(LaserTurretRef) as GameObject).GetComponent<ZoneBuilding>();
        newZoneBuilding.buildingType = ZoneBuildingType.LaserTurret;
        HandleNewObject(newZoneBuilding);
    }


    public void BuyMissileSiloButton()
    {
        ZoneBuilding newZoneBuilding = (Instantiate(MissileSiloRef) as GameObject).GetComponent<ZoneBuilding>();
        newZoneBuilding.buildingType = ZoneBuildingType.MissileSilo;
        HandleNewObject(newZoneBuilding);
    }
    //--------End Weapons--------------

    //-----New Defensive ZoneBuildings-----
    public void BuyMinorCity()
    {
        ZoneBuilding newZoneBuilding = (Instantiate(CityRef) as GameObject).GetComponent<ZoneBuilding>();
        newZoneBuilding.buildingType = ZoneBuildingType.City;
        HandleNewObject(newZoneBuilding);
    }


    public void BuyShieldGenerator()
    {
        ZoneBuilding newZoneBuilding = (Instantiate(GeneratorRef) as GameObject).GetComponent<ZoneBuilding>();
        newZoneBuilding.buildingType = ZoneBuildingType.ShieldGenerator;
        HandleNewObject(newZoneBuilding);
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
    void HandleNewObject(ZoneBuilding newObj)
    {
        // Let the user select a spot for the new zone building
        isPickingLocation = true;
        // Set the object to this variable so we can display it in the Update method while the user picks a location
        PurchasedZoneBuilding = newObj;
        // Make the new city a child object so it lives inside the earth's coordinate space
        PurchasedZoneBuilding.buildingTransform.SetParent(transform, true);
        // Set the scale passed in by the Instantiating function
        PurchasedZoneBuilding.buildingTransform.localScale = Vector3.one;
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
