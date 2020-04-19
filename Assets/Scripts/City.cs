using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a generic list of building types
public enum BuildingType
{
    Residential,
    Industrial,
    Government,
    Offensive
}

// This is a Generic Type for identifying the location and Type of buildings in the city
public interface CityBuilding
{
    Vector2 Position { get; set; }
    BuildingType category { get; }
}

public class City : MonoBehaviour
{
    Object TurretRef;
    Object BuildingRef;

    public string CityName;
    bool CityEditMode = false;
    int cityMinCoord = 1;
    int cityMaxCoord = 9;
    public IDictionary<Vector2, CityBuilding> CityGrid = new Dictionary<Vector2, CityBuilding>();
    
    // The flat square the city buildings sit on
    private GameObject CityFoundation;

    void Start()
    {
        TurretRef = Resources.Load("Turret");
        BuildingRef = Resources.Load("Building");
    }

    void Update()
    {
    }

    public bool AddBuilding(Vector2 newPosition, BuildingType type)
    {
        if (CityGrid.Keys.Contains(newPosition))
            return false;

        switch (type)
        {
            case BuildingType.Government:
            case BuildingType.Residential:
                return BuildResidentialBuilding(newPosition, type);
            case BuildingType.Industrial:
            case BuildingType.Offensive:
                return BuildLaserTurret(newPosition, type);
        }
        return false;
    }

    bool BuildResidentialBuilding(Vector2 newPosition, BuildingType type)
    {
        GameObject newBuilding = Instantiate(BuildingRef, new Vector3(newPosition.x, 5, newPosition.y), Quaternion.identity) as GameObject;
        // Make the new turret a child object so it lives inside the city's coordinate space
        newBuilding.transform.SetParent(transform, false);
        ResidentialBuilding newBuildingScript = newBuilding.GetComponent<ResidentialBuilding>();
        newBuildingScript.Position = newPosition;
        CityGrid[newPosition] = newBuildingScript;
        // Success
        return true;
    }

    bool BuildLaserTurret(Vector2 newPosition, BuildingType type)
    {
        GameObject newTurret = Instantiate(TurretRef, new Vector3(newPosition.x, 5, newPosition.y), Quaternion.identity) as GameObject;
        // Make the new turret a child object so it lives inside the city's coordinate space
        newTurret.transform.SetParent(transform, false);
        LaserTurret newLaserTurret = newTurret.GetComponent<LaserTurret>();
        newLaserTurret.Position = newPosition;
        CityGrid[newPosition] = newLaserTurret;
        // Success
        return true;
    }

    public void ShowCityMenu(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.Label("Global Defense Budget: $" + Earth.GlobalCurrency.ToString());
        if (CityEditMode)
        {
            GUILayout.BeginVertical();
            CityName = GUILayout.TextField(CityName);
            for (int xAxis = cityMinCoord; xAxis <= cityMaxCoord; xAxis++)
            {
                GUILayout.BeginHorizontal();
                for (int yAxis = cityMinCoord; yAxis <= cityMaxCoord; yAxis++)
                {
                    string buttonText = "";
                    if (CityGrid.ContainsKey(new Vector2(xAxis, yAxis)))
                    {
                        switch (CityGrid[new Vector2(xAxis, yAxis)].category)
                        {
                            case BuildingType.Offensive:
                                buttonText = "LaserTurret";
                                GUILayout.Button(buttonText);
                                break;
                            case BuildingType.Residential:
                                buttonText = "Residential";
                                GUILayout.Button(buttonText);
                                break;
                            default:
                                buttonText = "Unoccupied ";
                                GUILayout.Button(buttonText);
                                Debug.Log("Unknown tag");
                                break;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("New Homes"))
                        {
                            AddBuilding(new Vector2(xAxis, yAxis), BuildingType.Residential);
                        }
                        if (GUILayout.Button("New Turret"))
                        {
                            AddBuilding(new Vector2(xAxis, yAxis), BuildingType.Offensive);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(CityName);
            if (GUILayout.Button("Edit Mode"))
            {
                CityEditMode = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            for (int xAxis = cityMinCoord; xAxis <= cityMaxCoord; xAxis++)
            {
                GUILayout.BeginHorizontal();
                for (int yAxis = cityMinCoord; yAxis <= cityMaxCoord; yAxis++)
                {
                    string buttonText = "";
                    if (CityGrid.ContainsKey(new Vector2(xAxis, yAxis)))
                    {
                        switch (CityGrid[new Vector2(xAxis, yAxis)].category)
                        {
                            case BuildingType.Offensive:
                                buttonText = "LaserTurret";
                                GUILayout.Button(buttonText);
                                break;
                            case BuildingType.Residential:
                                buttonText = "Residential";
                                GUILayout.Button(buttonText);
                                break;
                            default:
                                buttonText = "Unknown    ";
                                GUILayout.Button(buttonText);
                                Debug.Log("Unknown tag");
                                break;
                        }
                    }
                    else
                    {
                        buttonText = "Unoccupied ";
                        GUILayout.Button(buttonText);
                        Debug.Log("Unknown tag");
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        if (CityEditMode)
        {
            if (GUILayout.Button("Turn Off Edit Mode"))
            {
                CityEditMode = false;
            }
        }
        // Bottom save and continue button
        if (GUILayout.Button("Save And Continue"))
        {
            CityEditMode = false;
            Earth.ContinueGame();
        }
    }
}
