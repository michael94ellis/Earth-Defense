using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Earth : MonoBehaviour
{
    public bool isPaused = false;

    private bool editMode = false;
    int windowWidth = 900;
    int windowHeight = 750;
    int windowOriginX = (Screen.width) / 2 - (900 / 2);
    int windowOriginY = (Screen.height) / 2 - (750 / 2);

    public delegate (string name, List<GameObject> buildings) GetCitiesDelegate();
    public static event GetCitiesDelegate GetCities;

    private (string name, List<GameObject> buildings) City = ("", new List<GameObject>());
    private IDictionary<Vector2, GameObject> ExistingCityGrid = new Dictionary<Vector2, GameObject>();


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
            GUI.Window(0, new Rect(windowOriginX, windowOriginY, windowWidth, windowHeight), EditCityGUI, "Earth Defense Shop");
        }
    }

    void FetchCityInfo()
    {
        City = GetCities();
        // Map the city/s existing things to their coords in the city grid
        foreach (GameObject child in City.buildings)
        {
            Vector2 coord = new Vector2(child.transform.localPosition.x, child.transform.localPosition.z);
            ExistingCityGrid[coord] = child.gameObject;
        }
    }

    void EditCityGUI(int windowID)
    {
        if (City.name == "")
        {
            FetchCityInfo();
        }
        // Start positions, these are cursors for printing the UI elements
        int x = 65, y = 40;
        int labelWidth = 150;
        int labelHeight = 30;
        int citySquareSize = 40;
        if (editMode)
        {
            GUI.TextField(new Rect(x, y, labelWidth, 30), City.name);
        }
        else
        {
            GUI.Label(new Rect(x, y, labelWidth, labelHeight), "City: " + City.name);
        }
        if (GUI.Button(new Rect(x + 175, y, labelWidth, labelHeight), "Edit City"))
        {
            editMode = !editMode;
        }
        // City is a square
        int cityMinCoord = 1;
        int cityMaxCoord = 9;
        for (int xAxis = cityMinCoord; xAxis <= cityMaxCoord; xAxis++)
        {
            for (int yAxis = cityMinCoord; yAxis <= cityMaxCoord; yAxis++)
            {
                string buttonText = "";
                if (ExistingCityGrid.ContainsKey(new Vector2(xAxis, yAxis)))
                {
                    switch (ExistingCityGrid[new Vector2(xAxis, yAxis)].tag)
                    {
                        case "Turret":
                            buttonText = "T";
                            break;
                        case "Building":
                            buttonText = "B";
                            break;
                        default:
                            Debug.Log("Unkwonw tag: " + ExistingCityGrid[new Vector2(xAxis, yAxis)].tag);
                            break;
                    }
                }
                GUI.Button(new Rect(65 + xAxis * citySquareSize + 20, y + yAxis * citySquareSize + 20, citySquareSize, citySquareSize), buttonText);
            }
        }
        y += 350;
        if (GUI.Button(new Rect(x, windowHeight - 60, labelWidth, 30), "Save And Continue"))
        {
            City = ("", new List<GameObject>());
            ExistingCityGrid = new Dictionary<Vector2, GameObject>();
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