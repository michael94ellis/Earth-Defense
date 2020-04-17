using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class City : MonoBehaviour
{
    Object TurretRef;

    public string CityName = "New York City";
    private List<GameObject> LaserTurrets = new List<GameObject>();
    public void RemoveTurret(GameObject turret)
    {
        LaserTurrets.Remove(turret);
    }
    private List<GameObject> Buildings = new List<GameObject>();

    private IDictionary<Vector2, GameObject> CityGrid = new Dictionary<Vector2, GameObject>();

    // The flat square the city buildings sit on
    private GameObject CityFoundation;

    void Start()
    {
        TurretRef = Resources.Load("Turret");
    }

    void Update()
    {
    }

    public bool AddBuilding(Vector2 gridLocation, string type)
    {
        if (CityGrid.Keys.Contains(gridLocation))
            return false;

        switch (type)
        {
            case "Turret":
                GameObject newTurret = Instantiate(TurretRef, new Vector3(gridLocation.x, 5, gridLocation.y), Quaternion.identity) as GameObject;
                // Make the new turret a child object so it lives inside the city's coordinate space
                newTurret.transform.SetParent(transform, false);
                LaserTurret newLaserTurret = newTurret.GetComponent<LaserTurret>();
                newLaserTurret.city = this;
                return true;
        }
        return false;
    }

    // Consumable list of all the city's weapons and buildings
    public List<GameObject> UpdateBuildings()
    {
        // Reset the lists
        List<GameObject> AllBuildings = new List<GameObject>();
        Buildings = new List<GameObject>();
        LaserTurrets = new List<GameObject>();
        // Add the city's children to the lists  
        foreach (Transform child in transform)
        {
            // Needs to be one of each type of object that lives in a city
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

            AllBuildings.Add(child.gameObject);
        }
        return AllBuildings;
    }
}
