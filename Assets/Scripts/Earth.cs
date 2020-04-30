using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface Damageable
{
    bool TakeDamage(int amount);
}

public class Earth : MonoBehaviour
{
    MenuManager GameManager;
    Object CityRef;
    Object GeneratorRef;
    Object LaserTurretRef;
    //Object SatelliteRef;
    Object MissileSiloRef;

    // This holds an item that was just purchased so it can be placed on the earth
    public GameObject NewObject;
    public EarthZone NewObjectZone;
    public static List<EarthZone> AllZones = new List<EarthZone>();
    public static List<EarthZone> ControlledZones = new List<EarthZone>();
    public static float GlobalCurrency { get; private set; }
    public static void AddGlobalCurrency(float money)
    {
        GlobalCurrency += money;
    }
    public static void SpendGlobalCurrency(float money)
    {
        GlobalCurrency -= money;
    }

    void Start()
    {
        GameManager = GameObject.Find("Earth").GetComponent<MenuManager>();
        CityRef = Resources.Load("City");
        LaserTurretRef = Resources.Load("Turret");
        MissileSiloRef = Resources.Load("MissileSilo");
        GeneratorRef = Resources.Load("Generator");
        //SatelliteRef = Resources.Load("EarthSatellite");
        foreach (GameObject earthZone in GameObject.FindGameObjectsWithTag("Player"))
        {
            EarthZone zone = earthZone.GetComponent<EarthZone>();
            if (zone != null)
                ControlledZones.Add(zone);
        }
    }

    void Update()
    {
        if (GameManager.isPickingLocation)
        {
            DisplayNewObjectInNorthAmerica();
            return;
        }
    }

    public void DisplayNewObjectInNorthAmerica()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool isInAllowedSpace = false;
        Vector3 earthHit = Vector3.zero;
        foreach (RaycastHit hit in hits)
        {
            // This hit.point is the point on earth where you clicked
            foreach (EarthZone controlledZone in ControlledZones)
            {
                if (hit.collider == controlledZone.GetComponent<Collider>())
                {
                    isInAllowedSpace = true;
                    NewObjectZone = controlledZone;
                    //Debug.Log(hit.point + " is in the Zone: ");
                }
                else if (hit.transform.gameObject == this.gameObject && controlledZone.GetComponent<Collider>().bounds.Contains(hit.point))
                {
                    earthHit = hit.point;
                }
            }
        }
        if (isInAllowedSpace && earthHit != Vector3.zero)
        {
            // Get a point directly above the city away from earth
            Vector3 awayFromEarth = earthHit - transform.position;
            // assign the up vector for the city
            NewObject.transform.up = awayFromEarth;
            NewObject.transform.position = earthHit;
        }
    }

    public GameObject BuildNewCity()
    {
        GameObject newCityObj = Instantiate(CityRef) as GameObject;
        NewObject = newCityObj;
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, true);
        NewObject.transform.localScale = new Vector3(0.0025f, 0.0025f, 0.0025f);
        return NewObject;
    }

    public GameObject BuildNewLaserWeapon()
    {
        GameObject NewWeapon = Instantiate(LaserTurretRef) as GameObject;
        HandleNewWeapon(NewWeapon);
        SpendGlobalCurrency(75);
        return NewWeapon;
    }

    public GameObject BuildNewMissileSilo()
    {
        GameObject NewWeapon = Instantiate(MissileSiloRef) as GameObject;
        HandleNewWeapon(NewWeapon);
        return NewWeapon;
        //SpendGlobalCurrency(75);
    }

    private void HandleNewWeapon(GameObject NewWeapon)
    {
        NewObject = NewWeapon;
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, true);
        NewObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    public void BuildNewShieldGenerator()
    {
        GameObject NewShieldGenerator = Instantiate(GeneratorRef) as GameObject;
        NewObject = NewShieldGenerator;
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, true);
        NewObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        NewObjectZone.ShieldGenerator = NewShieldGenerator.GetComponent<ShieldGenerator>();
        NewObjectZone.ShieldGenerator.parentZone = NewObjectZone;
        //SpendGlobalCurrency(75);
    }

    //public void BuildNewEarthSatellite()
    //{
    //    GameObject NewSatellite = Instantiate(SatelliteRef, new Vector3(RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f)), Quaternion.identity) as GameObject;
    //    // Make it smaller than the Earth
    //    NewSatellite.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    //    // Make the new city a child object so it lives inside the earth's coordinate space
    //    NewSatellite.transform.SetParent(transform, false);
    //    // Get a point directly above the city away from earth
    //    Vector3 awayFromEarth = NewSatellite.transform.position - transform.position;
    //    // assign the up vector for the city
    //    NewSatellite.transform.up = awayFromEarth;
    //    SpendGlobalCurrency(75);
    //}
    
    //// Returns a random value in the range, 50% change of being negative
    //float RandomCoord(float min, float max)
    //{
    //    // Sign(+/-): 1 is plus, 2 is minus
    //    var sign = Random.Range(1, 3);
    //    var value = Random.Range(min, max);
    //    if (sign == 2)
    //    {
    //        return value * -1;
    //    }
    //    else
    //    {
    //        return value;
    //    }
    //}
}