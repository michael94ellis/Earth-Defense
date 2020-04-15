using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 80.0f;
    /// Laser recharge time
    private List<GameObject> LaserTurrets = new List<GameObject>();
    private List<GameObject> Buildings = new List<GameObject>();
    private GameObject CityFoundation;

    void Start()
    {
        Debug.Log("City Coming Online");
        UpdateBuildingsList();
    }

    void UpdateBuildingsList()
    {
        Buildings = new List<GameObject>();
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

    // Update is called once per frame
    void Update()
    {
        // Remove nulls
        Buildings.RemoveAll(item => item == null);
        LaserTurrets.RemoveAll(item => item == null);
        if (Buildings.Count == 0 && LaserTurrets.Count == 0)
        {
            Debug.Log("City Destroyed!");
            CityFoundation.SetActive(false);
            return;
        }
    }
}
