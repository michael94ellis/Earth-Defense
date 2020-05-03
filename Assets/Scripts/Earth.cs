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
        foreach (GameObject earthZone in GameObject.FindGameObjectsWithTag("Player"))
        {
            EarthZone zone = earthZone.GetComponent<EarthZone>();
            if (zone != null)
                ControlledZones.Add(zone);
        }
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