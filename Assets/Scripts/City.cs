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
        // Give access to the buildings of the city to the earth
        Earth.AddCity(CityName, GetAllBuildings());
    }

    void Update()
    {
    }

    private void OnDestroy()
    {
        Earth.RemoveCity(CityName);
    }

    List<GameObject> GetAllBuildings()
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
        return AllBuildings;
    }

    void UpdateBuildingsList()
    {
        // Reset the lists
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
    }
}
