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
        foreach (GameObject earthZone in GameObject.FindGameObjectsWithTag("Player"))
        {
            EarthZone zone = earthZone.GetComponent<EarthZone>();
            if (zone != null)
                ControlledZones.Add(zone);
        }
    }
}