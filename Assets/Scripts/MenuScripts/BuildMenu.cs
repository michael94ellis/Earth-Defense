using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    // Object Refs to Instantiate new stuff
    Object CityRef;
    Object GeneratorRef;
    Object MissileSiloRef;
    Object LaserTurretRef;
    //Object SatelliteRef;
    // For handling the time when user places purchased zone buildings
    public static ZoneBuilding PurchasedZoneBuilding = null;
    EarthZone selectedZone;
    // Cached list of colliders fetched/reset every time a ZoneBuilding is bought
    Dictionary<Collider, EarthZone> ControlledZoneColliders = new Dictionary<Collider, EarthZone>();

    void Start()
    {
        // idk why but its not working when i set these in the inspector of unity
        //CityRef = Resources.Load("City");
        //GeneratorRef = Resources.Load("Generator");
        MissileSiloRef = Resources.Load("MissileSilo");
        LaserTurretRef = Resources.Load("Turret");
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
    //public void BuyMinorCity()
    //{
    //    ZoneBuilding newZoneBuilding = (Instantiate(CityRef) as GameObject).GetComponent<ZoneBuilding>();
    //    newZoneBuilding.buildingType = ZoneBuildingType.City;
    //    HandleNewObject(newZoneBuilding);
    //}


    //public void BuyShieldGenerator()
    //{
    //    ZoneBuilding newZoneBuilding = (Instantiate(GeneratorRef) as GameObject).GetComponent<ZoneBuilding>();
    //    newZoneBuilding.buildingType = ZoneBuildingType.ShieldGenerator;
    //    HandleNewObject(newZoneBuilding);
    //}
    //--------End Defenses--------------
    //-----Build Objects------
    void HandleNewObject(ZoneBuilding newObj)
    {
        // Set the object to this variable so we can display it in the Update method while the user picks a location
        PurchasedZoneBuilding = newObj;
        PurchasedZoneBuilding.isActive = false;
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

    //turn colliders on for zones
    public void SetZoneCollidersEnabled(bool isEnabled)
    {
        foreach (Collider zoneCollider in ControlledZoneColliders.Keys)
        {
            zoneCollider.enabled = isEnabled;
        }
    }

    void Update()
    {
        //previews placement of new building until clicked
        if (PurchasedZoneBuilding != null)
        {
            RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
            foreach (RaycastHit hit in hitsInOrder)
            {
                // This hit.point is the point on earth where the mouse hovers
                if (selectedZone == null && ControlledZoneColliders.Keys.Contains(hit.collider))
                {
                    // This should only be reached once each time the user hovers over a zone
                    //Debug.Log("selected zone");
                    selectedZone = ControlledZoneColliders[hit.collider];
                    PurchasedZoneBuilding.ParentZone = selectedZone;
                    PurchasedZoneBuilding.isActive = true;
                    // Put it in the coord space of the earthzone
                    PurchasedZoneBuilding.buildingTransform.SetParent(selectedZone.transform, true);
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
                        selectedZone.ZoneBuildings.Add(PurchasedZoneBuilding);
                        //SetZoneCollidersEnabled(false);
                        PurchasedZoneBuilding = null;
                        selectedZone = null;
                    }
                    return;
                }
            }
        }
    }
}