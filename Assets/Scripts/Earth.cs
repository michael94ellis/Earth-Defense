using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is instantiated by the Earth class
// It holds important vars for the menu separate from the earth object
public class MenuManager
{
    public bool Paused;
    public int windowWidth = Screen.width * 4 / 6;
    public int windowHeight = Screen.height * 4 / 6;
    public int windowOriginX = Screen.width / 6;
    public int windowOriginY = Screen.height / 6;
    public enum MenuScreen
    {
        NewGame,
        BuyCity,
        PickNewCityLocation,
        ShowAllCities,
        EditCity, // Eventually the City Object should present it's own edit screen
        GameOver
    }
    public MenuScreen CurrentScreen;
    public City CurrentCity;
}

public class Earth : MonoBehaviour
{
    public static MenuManager GameManager;
    Object CityRef;
    // Used to display the currently viewed city in GUI
    List<City> Cities;
    // City is a square
    string newCityName;
    private static int globalcurrency;
    public static int GlobalCurrency
    {
        get
        {
            return globalcurrency;
        }
        set
        {
            globalcurrency = value;
        }
    }

    void Start()
    {
        CityRef = Resources.Load("City");
        Cities = new List<City>();
        // Init the world
        globalcurrency = 1000000000;
        // Set up the game manager for beginning of game(will change when gameplay changes)
        GameManager = new MenuManager();
        GameManager.CurrentScreen = MenuManager.MenuScreen.NewGame;
        // Pause the game so the player starts in the Menu Screen - OnGUI() method
        GameManager.Paused = true;
    }

    void Update()
    {
    }

    void OnMouseUp()
    {
        // Only Pause if not already paused, menu must have unpause button
        if (!GameManager.Paused)
        {
            PauseGame();
            // Show the user all their cities
            GameManager.CurrentScreen = MenuManager.MenuScreen.ShowAllCities;
            // Update and fetch data here, to not run loops like this every frame
        }
        else if (GameManager.CurrentScreen == MenuManager.MenuScreen.PickNewCityLocation)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                // This hit.point is the point on earth where you clicked
                Debug.Log(hit.point);
                // HANDLE THE ROTATION PLEASE
                HandleNewCity(hit.point, Quaternion.identity);
                GameManager.CurrentScreen = MenuManager.MenuScreen.ShowAllCities;
            }
        }
    }

    void OnGUI()
    {
        // Only show menus if the game is paused
        if (GameManager.Paused)
        {
            switch (GameManager.CurrentScreen)
            {
                case MenuManager.MenuScreen.NewGame:
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), NewGameScreen, "New Game");
                    break;
                case MenuManager.MenuScreen.BuyCity:
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), BuyCityScreen, "Buy A New City");
                    break;
                case MenuManager.MenuScreen.PickNewCityLocation:
                        // Handled in OnMouseUp()
                    break;
                case MenuManager.MenuScreen.EditCity:
                    GUILayout.Window(0, new Rect(Earth.GameManager.windowOriginX, Earth.GameManager.windowOriginY, Earth.GameManager.windowWidth, Earth.GameManager.windowHeight), GameManager.CurrentCity.ShowCityMenu, "City Detail Page");
                    break;
                case MenuManager.MenuScreen.ShowAllCities:
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), ShowAllCities, "Earth Defense Shop");
                    break;
            }
        }
    }

    void NewGameScreen(int windowID)
    {
        GUILayout.Label("Welcome To The Game!");
        if (GUILayout.Button("Click here to begin"))
        {
            // MagnetoCat: Add/Invoke The Place City Feature Here
            GameManager.CurrentScreen = MenuManager.MenuScreen.BuyCity; // Temporary: Just send the player to the buy city page for their first city
        }
    }

    void BuyCityScreen(int windowID)
    {
        GUILayout.Label("Name Your City: ");
        newCityName = GUILayout.TextField(newCityName);

        if (GUILayout.Button("Click here to pick a city location on the earth"))
        {
            GameManager.CurrentScreen = MenuManager.MenuScreen.PickNewCityLocation;
        }
    }
    // MagnetoCat: Erase this whole method when you add the city placement
    void HandleNewCity(Vector3 location, Quaternion angle)
    {
        GameObject newCity = Instantiate(CityRef, location, angle) as GameObject;
        // Make it smaller than the Earth
        newCity.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // Make the new city a child object so it lives inside the earth's coordinate space
        newCity.transform.SetParent(transform, false);
        // City needs to be smaller than earth
        // This lets us control the City Script attached to the City object we just made
        City newCityScript = newCity.GetComponent<City>();
        Cities.Add(newCityScript);
        newCityScript.CityName = newCityName;
        // Now show all the cities 
        GameManager.CurrentScreen = MenuManager.MenuScreen.ShowAllCities;
        newCityName = "";
    }

    void ShowAllCities(int windowID)
    {
        if (GUILayout.Button("Buy New City"))
        {
            GameManager.CurrentScreen = MenuManager.MenuScreen.BuyCity;
        }
        foreach (City city in Cities)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("City: " + city.CityName);
            if (GUILayout.Button("Edit City"))
            {
                // Set the current city to edit it
                GameManager.CurrentCity = city;
                GameManager.CurrentScreen = MenuManager.MenuScreen.EditCity;

            }
            GUILayout.EndHorizontal();
        }
        // Bottom save and continue button
        if (GUILayout.Button("Save And Continue"))
        {
            ContinueGame();
        }
    }

    public static void PauseGame()
    {
        GameManager.Paused = true;
        Time.timeScale = 0;
        //Disable scripts that still work while timescale is set to 0
    }
    public static void ContinueGame()
    {
        GameManager.Paused = false;
        Time.timeScale = 1;
        //enable the scripts again
    }
}