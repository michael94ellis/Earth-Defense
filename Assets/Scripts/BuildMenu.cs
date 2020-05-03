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
        //TO DO: need a check for if the click is an approved placement
        if (isPickingLocation)
        {
            RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
            EarthZone selectedZone = null;
            foreach (RaycastHit hit in hitsInOrder)
            {
                // This hit.point is the point on earth where you clicked
                foreach (EarthZone controlledZone in Earth.ControlledZones)
                {
                    if (selectedZone == null && hit.collider == controlledZone.GetComponent<Collider>())
                    {
                        if (PurchasedZoneBuilding.GetComponent<ShieldGenerator>() != null && controlledZone.ShieldGenerator != null)
                            return;
                        else
                            selectedZone = controlledZone;
                    }
                    else if (selectedZone != null && hit.transform.gameObject == this.gameObject && controlledZone.GetComponent<Collider>().bounds.Contains(hit.point))
                    {
                        PurchasedZoneBuilding.GetComponent<ZoneBuilding>().ParentZone = selectedZone;
                        PurchasedZoneBuilding.transform.SetParent(controlledZone.transform, true);
                        // Get a point directly above the city away from earth
                        Vector3 awayFromEarth = hit.point - transform.position;
                        // assign the up vector for the city
                        PurchasedZoneBuilding.transform.up = awayFromEarth;
                        PurchasedZoneBuilding.transform.position = hit.point;
                        if (Input.GetMouseButton(0))
                        {
                            isPickingLocation = false;
                            //turns colliders back off
                            foreach (EarthZone zone in Earth.ControlledZones)
                            {
                                zone.GetComponent<Collider>().enabled = false;
                            }
                        }
                        return;
                    }
                }
            }
        }
    }

    //opens build menu
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
        isPickingLocation = true;
        PurchasedZoneBuilding = BuildNewLaserWeapon();
    }


    public void BuyMissileSiloButton()
    {
        ActivateZoneColliders();
        isPickingLocation = true;
        PurchasedZoneBuilding = BuildNewMissileSilo();
    }
    //----------------------

    //-----New Defensive ZoneBuildings-----
    public void BuyMinorCity()
    {
        isPickingLocation = true;
        PurchasedZoneBuilding = BuildNewCity();
    }


    public void BuyShieldGenerator()
    {
        ActivateZoneColliders();
        isPickingLocation = true;
        PurchasedZoneBuilding = BuildNewShieldGenerator();
    }
    //----------------------

    //turn colliders on for zones
    public void ActivateZoneColliders()
    {
        foreach (EarthZone zone in Earth.ControlledZones)
        {
            zone.GetComponent<Collider>().enabled = true;
        }
    }

    //sends new aliens
    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }


    //-----Build Objects------

    GameObject BuildNewCity()
    {
        GameObject newCity = Instantiate(CityRef) as GameObject;
        return HandleNewObject(newCity, new Vector3(0.0025f, 0.0025f, 0.0025f));
    }

    GameObject BuildNewLaserWeapon()
    {
        GameObject NewWeapon = Instantiate(LaserTurretRef) as GameObject;
        return HandleNewObject(NewWeapon, new Vector3(0.01f, 0.01f, 0.01f));
    }

    GameObject BuildNewMissileSilo()
    {
        GameObject NewWeapon = Instantiate(MissileSiloRef) as GameObject;
        return HandleNewObject(NewWeapon, new Vector3(0.01f, 0.01f, 0.01f));
    }

    GameObject BuildNewShieldGenerator()
    {
        GameObject NewShieldGenerator = Instantiate(GeneratorRef) as GameObject;
        return HandleNewObject(NewShieldGenerator, new Vector3(0.01f, 0.01f, 0.01f));
    }

    GameObject HandleNewObject(GameObject newObj, Vector3 scale)
    {
        PurchasedZoneBuilding = newObj;
        // Make the new city a child object so it lives inside the earth's coordinate space
        PurchasedZoneBuilding.transform.SetParent(transform, true);
        PurchasedZoneBuilding.transform.localScale = scale;
        return PurchasedZoneBuilding;
    }
}
