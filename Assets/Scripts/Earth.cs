using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface Damageable
{
    bool TakeDamage();
}

public class Earth : MonoBehaviour
{
    MenuManager GameManager;
    Object CityRef;
    Object GeneratorRef;
    Object LaserTurretRef;
    Object SatelliteRef;
    float mainSpeed = 0.5f; //regular speed
    // This holds an item that was just purchased so it can be placed on the earth
    public GameObject NewObject;
    //public static List<EarthZone> Zones = new List<EarthZone>();
    public static EarthZone Zone1;
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
       // Generator = Resources.Load("Generator");
        SatelliteRef = Resources.Load("EarthSatellite");
        //Zones.Add(GameObject.Find("NorthAmericanShield").GetComponent<EarthZone>());
        Zone1 = GameObject.Find("NorthAmericanShield").GetComponent<EarthZone>();
       // SouthAmerica = GameObject.Find("SouthAmerica").GetComponent<Collider>();
        GlobalCurrency = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // Only Pause if not already paused, menu must have unpause button
            if (!GameManager.Paused)
            {
                GameManager.Pause();
                // Show the user all their cities
                GameManager.CurrentScreen = MenuManager.MenuScreen.MainMenu;
                // Update and fetch data here, to not run loops like this every frame
            }
            else if (GameManager.isPickingLocation)
            {
                GameManager.isPickingLocation = false;
                return;
            }
        }
        if (GameManager.isPickingLocation)
        {
            DisplayNewObjectInNorthAmerica();
            return;
        }
    }

    void DisplayNewObjectInNorthAmerica()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            // This hit.point is the point on earth where you clicked
            if (hit.transform.gameObject == this.gameObject)
            {
                if (Zone1.GetComponent<Collider>().bounds.Contains(hit.point))
                {
                    Debug.Log(hit.point + " is in North America: ");
                    // Get a point directly above the city away from earth
                    Vector3 awayFromEarth = hit.point - transform.position;
                    // assign the up vector for the city
                    NewObject.transform.up = awayFromEarth;
                    NewObject.transform.position = hit.point;
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void BuildNewCity()
    {
        GameObject newCityObj = Instantiate(CityRef) as GameObject;
        NewObject = newCityObj;
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, true);
        City newCity = NewObject.GetComponent<City>();
        if (newCity != null)
            Zone1.MinorCities.Add(newCity);
        NewObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        SpendGlobalCurrency(100);
    }

    public void BuildNewWeapon()
    {
        GameObject NewWeapon = Instantiate(LaserTurretRef) as GameObject;
        NewObject = NewWeapon;
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, true);
        Weapon newWeapon = NewObject.GetComponent<Weapon>();
        if (newWeapon != null)
            Zone1.Weapons.Add(newWeapon);
        NewObject.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
        SpendGlobalCurrency(75);
    }

    public void BuildNewEarthSatellite()
    {
        GameObject NewSatellite = Instantiate(SatelliteRef, new Vector3(RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f)), Quaternion.identity) as GameObject;
        // Make it smaller than the Earth
        NewSatellite.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewSatellite.transform.SetParent(transform, false);
        // Get a point directly above the city away from earth
        Vector3 awayFromEarth = NewSatellite.transform.position - transform.position;
        // assign the up vector for the city
        NewSatellite.transform.up = awayFromEarth;
        SpendGlobalCurrency(75);
    }
    
    // Returns a random value in the range, 50% change of being negative
    float RandomCoord(float min, float max)
    {
        // Sign(+/-): 1 is plus, 2 is minus
        var sign = Random.Range(1, 3);
        var value = Random.Range(min, max);
        if (sign == 2)
        {
            return value * -1;
        }
        else
        {
            return value;
        }
    }
}