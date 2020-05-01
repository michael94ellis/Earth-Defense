using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GameManager = gameObject.GetComponent<MenuManager>();
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
            DisplayNewObject();
            return;
        }
    }

    public void DisplayNewObject()
    {
        RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
        EarthZone selectedZone = null;
        foreach (RaycastHit hit in hitsInOrder)
        {
            // This hit.point is the point on earth where you clicked
            foreach (EarthZone controlledZone in ControlledZones)
            {
                if (selectedZone == null && hit.collider == controlledZone.GetComponent<Collider>())
                {
                    if (NewObject.GetComponent<ShieldGenerator>() != null && controlledZone.ShieldGenerator != null)
                        return;
                    else
                        selectedZone = controlledZone;
                }
                else if (selectedZone != null && hit.transform.gameObject == this.gameObject && controlledZone.GetComponent<Collider>().bounds.Contains(hit.point))
                {
                    NewObject.GetComponent<MissileSilo>().ParentZone = selectedZone;
                    NewObject.transform.SetParent(controlledZone.transform, true);
                    // Get a point directly above the city away from earth
                    Vector3 awayFromEarth = hit.point - transform.position;
                    // assign the up vector for the city
                    NewObject.transform.up = awayFromEarth;
                    NewObject.transform.position = hit.point;
                    return;
                }
            }
        }
    }

    public GameObject BuildNewCity()
    {
        GameObject newCity = Instantiate(CityRef) as GameObject;
        return HandleNewObject(newCity, new Vector3(0.0025f, 0.0025f, 0.0025f));
    }

    public GameObject BuildNewLaserWeapon()
    {
        GameObject NewWeapon = Instantiate(LaserTurretRef) as GameObject;
        return HandleNewObject(NewWeapon, new Vector3(0.01f, 0.01f, 0.01f));
    }

    public GameObject BuildNewMissileSilo()
    {
        GameObject NewWeapon = Instantiate(MissileSiloRef) as GameObject;
        return HandleNewObject(NewWeapon, new Vector3(0.01f, 0.01f, 0.01f));
    }

    public GameObject BuildNewShieldGenerator()
    {
        GameObject NewShieldGenerator = Instantiate(GeneratorRef) as GameObject;
        return HandleNewObject(NewShieldGenerator, new Vector3(0.01f, 0.01f, 0.01f));
    }

    private GameObject HandleNewObject(GameObject newObj, Vector3 scale)
    {
        NewObject = newObj;
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, true);
        NewObject.transform.localScale = scale;
        return NewObject;
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