using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Earth : MonoBehaviour
{
    public bool isPaused = false;
    private bool editMode = false;

    public delegate (string name, List<GameObject> buildings) GetCitiesDelegate();
    public static event GetCitiesDelegate GetCities;

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

            PauseGame();
        }
    }

    void OnGUI()
    {
        if (isPaused)
        {
            int windowWidth = 900;
            int windowHeight = 750;
            int windowOriginX = (Screen.width) / 2 - (windowWidth / 2);
            int windowOriginY = (Screen.height) / 2 - (windowHeight / 2);
            GUI.Window(0, new Rect(windowOriginX, windowOriginY, windowWidth, windowHeight), EditCityGUI, "Earth Defense Shop");
        }
    }

    void EditCityGUI(int windowID)
    {
        // You may put a label to show a message to the player
        (string name, List<GameObject> buildings) City = GetCities();
        int x = 65, y = 40;
        if (editMode)
        {
            GUI.TextField(new Rect(x, y, 150, 30), City.name);
        }
        else
        {
            GUI.Label(new Rect(x, y, 150, 30), "City: " + City.name);
        }
        if (GUI.Button(new Rect(x + 175, y, 100, 30), "Edit City"))
        {
            editMode = !editMode;
        }

        // Map the city/s existing things to their coords in the city grid
        IDictionary<Vector2, GameObject> CityGrid = new Dictionary<Vector2, GameObject>();
        foreach (GameObject child in City.buildings)
        {
            Vector2 coord = new Vector2(child.transform.localPosition.x, child.transform.localPosition.z);
            Debug.Log(child.gameObject.tag);
            CityGrid[coord] = child.gameObject;
        }
        // City is a square
        int cityMinCoord = 1;
        int cityMaxCoord = 9;
        for (int xAxis = cityMinCoord; xAxis <= cityMaxCoord; xAxis++)
        {
            for (int yAxis = cityMinCoord; yAxis <= cityMaxCoord; yAxis++)
            {
                string buttonText = "";
                if (CityGrid.ContainsKey(new Vector2(xAxis, yAxis)))
                {
                    switch (CityGrid[new Vector2(xAxis, yAxis)].tag)
                    {
                        case "Turret":
                            buttonText = "T";
                            break;
                        case "Building":
                            buttonText = "B";
                            break;
                        default:
                            Debug.Log(CityGrid[new Vector2(xAxis, yAxis)].tag);
                            break;
                    }
                }
                GUI.Button(new Rect(65 + xAxis * 30 + 20, y + yAxis * 30 + 10, 30, 30), buttonText);
            }
        }
        y += 350;
        if (GUI.Button(new Rect(x, y, 150, 30), "Save And Continue"))
        {
            isPaused = false;
            ContinueGame();
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