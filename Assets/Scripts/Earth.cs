using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Earth : MonoBehaviour
{
    public bool isPaused = false;

    private bool editMode = false;
    int windowOriginX = Screen.width / 4;
    int windowOriginY = Screen.height / 6;
    int windowWidth = Screen.width - 2 * (Screen.width / 4);
    int windowHeight = Screen.height - 2 * (Screen.height / 6);
    // Used to display the currently viewed city in GUI
    List<City> Cities;
    // City is a square
    int cityMinCoord = 1;
    int cityMaxCoord = 9;
    int citySquareSize = 40;
    int labelWidth = 150;
    int labelHeight = 30;
    string newCityName;
    Object CityRef;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseUp()
    {
        if (!isPaused)
        {
            Cities = new List<City>();
            GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
            foreach (GameObject city in cities)
            {
                Cities.Add(city.GetComponent<City>());
            }
            PauseGame();
        }
    }

    void OnGUI()
    {
        if (isPaused)
        {
            GUI.Window(0, new Rect(windowOriginX, windowOriginY, windowWidth, windowHeight), EditCityGUI, "Earth Defense Shop");
        }
        else if (isPaused)
        {
            GUI.Window(0, new Rect(windowOriginX, windowOriginY, windowWidth, windowHeight), BuildFirstCity, "Build Your First City");
        }
    }

    void BuildFirstCity(int windowID)
    {
        int x = 65, y = 40;
        GUI.Label(new Rect(x, y, labelWidth, labelHeight), "Name Your City: ");
        newCityName = GUI.TextField(new Rect(x + labelWidth + 10, y, labelWidth, labelHeight), newCityName);
        y += 2 * labelHeight;
        if (GUI.Button(new Rect(x, y, labelWidth, labelHeight), "North Pole"))
        {
            GameObject newCity = Instantiate(CityRef, new Vector3(-5, 49, -5), Quaternion.identity) as GameObject;
            // Make the new city a child object so it lives inside the earth's coordinate space
            newCity.transform.SetParent(transform);
            City newCityScript = newCity.GetComponent<City>();
            Cities.Add(newCityScript);
            newCityScript.CityName = newCityName;

            isPaused = false;
            ContinueGame();
        }
        y += 2 * labelHeight;
        if (GUI.Button(new Rect(x, y, labelWidth, labelHeight), "South Pole"))
        {
            GameObject newCity = Instantiate(CityRef, new Vector3(5, -49, -5), Quaternion.Euler(0, 0, 180)) as GameObject;
            // Make the new city a child object so it lives inside the earth's coordinate space
            newCity.transform.SetParent(transform);
            City newCityScript = newCity.GetComponent<City>();
            Cities.Add(newCityScript);
            newCityScript.CityName = newCityName;

            isPaused = false;
            ContinueGame();
        }
        y += 2 * labelHeight;
        if (GUI.Button(new Rect(x, y, labelWidth, labelHeight), "African Coast"))
        {
            GameObject newCity = Instantiate(CityRef, new Vector3(49, 5, -5), Quaternion.Euler(0, 0, 270)) as GameObject;
            // Make the new city a child object so it lives inside the earth's coordinate space
            newCity.transform.SetParent(transform);
            City newCityScript = newCity.GetComponent<City>();
            Cities.Add(newCityScript);
            newCityScript.CityName = newCityName;

            isPaused = false;
            ContinueGame();
        }
        y += 2 * labelHeight;
        if (GUI.Button(new Rect(x, y, labelWidth, labelHeight), "Pacific Ocean"))
        {
            GameObject newCity = Instantiate(CityRef, new Vector3(-49, -5, -5), Quaternion.Euler(0, 0, 90)) as GameObject;
            // Make the new city a child object so it lives inside the earth's coordinate space
            newCity.transform.SetParent(transform);
            City newCityScript = newCity.GetComponent<City>();
            Cities.Add(newCityScript);
            newCityScript.CityName = newCityName;

            isPaused = false;
            ContinueGame();
        }
    }

    void EditCityGUI(int windowID)
    {
        int x = 65, y = 40;
        int labelWidth = 150;
        int labelHeight = 30;
        int spacer = 10;
        // Start positions, these are cursors for printing the UI elements
        foreach (City city in Cities)
        {
            if (editMode)
            {
                city.CityName = GUI.TextField(new Rect(x, y, labelWidth, labelHeight), city.CityName);
                ModifyCityLayoutGUI(city);
            }
            else
            {
                GUI.Label(new Rect(x, y, labelWidth, labelHeight), "City: " + city.CityName);
            }
            if (GUI.Button(new Rect(x + 175, y, labelWidth, labelHeight), "Edit City"))
            {
                editMode = !editMode;
            }
            y += labelHeight + spacer;
            // TODO Add ability to move camera to a city and look at it and open its edit page

        }
        // Bottom save and continue button
        if (GUI.Button(new Rect(x, windowHeight - 60, labelWidth, 30), "Save And Continue"))
        {
            isPaused = false;
            ContinueGame();
        }
    }

    private IDictionary<Vector2, GameObject> CurrentlyDisplayedCityGrid = new Dictionary<Vector2, GameObject>();

    void ModifyCityLayoutGUI(City city)
    {
        int y = 40;
        int spacer = 20;

        // Map the city/s existing things to their coords in the city grid
        List<GameObject> existingBuildings = city.UpdateBuildings();
        foreach (GameObject building in existingBuildings)
        {
            Vector2 coord = new Vector2(building.transform.localPosition.x, building.transform.localPosition.z);
            CurrentlyDisplayedCityGrid[coord] = building;
        }
        foreach (GameObject building in existingBuildings)
        {
            for (int xAxis = cityMinCoord; xAxis <= cityMaxCoord; xAxis++)
            {
                for (int yAxis = cityMinCoord; yAxis <= cityMaxCoord; yAxis++)
                {
                    string buttonText = "";
                    if (CurrentlyDisplayedCityGrid.ContainsKey(new Vector2(xAxis, yAxis)))
                    {
                        switch (CurrentlyDisplayedCityGrid[new Vector2(xAxis, yAxis)].tag)
                        {
                            case "Turret":
                                buttonText = "T";
                                GUI.Button(new Rect(65 + xAxis * citySquareSize + spacer, y + yAxis * citySquareSize + spacer, citySquareSize, citySquareSize), buttonText);
                                break;
                            case "Building":
                                buttonText = "B";
                                GUI.Button(new Rect(65 + xAxis * citySquareSize + spacer, y + yAxis * citySquareSize + spacer, citySquareSize, citySquareSize), buttonText);
                                break;
                            default:
                                Debug.Log("Unkwonw tag: " + CurrentlyDisplayedCityGrid[new Vector2(xAxis, yAxis)].tag);
                                break;
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(65 + xAxis * citySquareSize + spacer, y + yAxis * citySquareSize + spacer, citySquareSize, citySquareSize), buttonText))
                        {
                            city.AddBuilding(new Vector2(xAxis, yAxis), "Turret");
                        }
                    }
                }
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        //enable the scripts again
    }
}