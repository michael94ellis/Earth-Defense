using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public string CityName = "New York City";
    private List<GameObject> LaserTurrets = new List<GameObject>();
    private List<GameObject> Buildings = new List<GameObject>();

    // The flat square the city buildings sit on
    private GameObject CityFoundation;

    void Start()
    {
        Debug.Log("City Coming Online");
        UpdateBuildingsList();
        // Give access to the buildings of the city to the earth
        Earth.GetCities += GetCityInfo;
    }

    void Update()
    {
    }

    (string, List<GameObject>) GetCityInfo()
    {
        UpdateBuildingsList();
        List<GameObject> AllBuildings = new List<GameObject>();
        foreach (GameObject building in Buildings)
        {
            AllBuildings.Add(building);
        }
        foreach (GameObject turret in LaserTurrets)
        {
            AllBuildings.Add(turret);
        }
        (string, List<GameObject>) ThisCity = (CityName, AllBuildings);
        return ThisCity;
    }

    void UpdateBuildingsList()
    {
        Buildings = new List<GameObject>();
        LaserTurrets = new List<GameObject>();
        // Add the city's objects to the lists  
        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case "CityFoundation":
                    CityFoundation = child.gameObject;
                    break;
                case "Turret":
                    LaserTurrets.Add(child.gameObject);
                    break;
                case "Building":
                    Buildings.Add(child.gameObject);
                    break;
            }
        }
        Debug.Log(LaserTurrets.Count + " Laser Turrets");
        Debug.Log(Buildings.Count + " Buildings");
    }
}
