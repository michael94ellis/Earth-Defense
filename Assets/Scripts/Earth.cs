using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is instantiated by the Earth class
// It holds important vars for the menu separate from the earth object
public class MenuManager
{
    public bool Paused;
    public bool isPickingLocation;
    public int windowWidth = Screen.width * 4 / 6;
    public int windowHeight = Screen.height * 4 / 6;
    public int windowOriginX = Screen.width / 6;
    public int windowOriginY = Screen.height / 6;
    // This holds an item that was just purchased so it can be placed on the earth
    public Object NewObject;

    public enum MenuScreen
    {
        NewGame,
        MainMenu,
        GameOver
    }
    public MenuScreen CurrentScreen;

    public void Pause()
    {
        Paused = true;
        Time.timeScale = 0;
        //Disable scripts that still work while timescale is set to 0
    }
    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1;
        //enable the scripts again
    }
}

public class Earth : MonoBehaviour
{
    public static MenuManager GameManager;
    Object CityRef;
    Object LaserTurretRef;
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
        LaserTurretRef = Resources.Load("Turret");
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
            GameManager.Pause();
            // Show the user all their cities
            GameManager.CurrentScreen = MenuManager.MenuScreen.MainMenu;
            // Update and fetch data here, to not run loops like this every frame
        }
        else if (GameManager.isPickingLocation)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                GameManager.isPickingLocation = false;
                // This hit.point is the point on earth where you clicked
                BuildNewObjectOnEarth(hit.point);
            }
        }
    }

    void OnGUI()
    {
        // Only show menus if the game is paused
        if (GameManager.Paused && !GameManager.isPickingLocation)
        {
            switch (GameManager.CurrentScreen)
            {
                case MenuManager.MenuScreen.NewGame:
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), NewGameScreen, "New Game");
                    break;
                case MenuManager.MenuScreen.MainMenu:
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), MainEarthMenu, "Earth Defense Shop");
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
            GameManager.CurrentScreen = MenuManager.MenuScreen.MainMenu; // Temporary: Just send the player to the buy city page for their first city
        }
    }

    // MagnetoCat: Erase this whole method when you add the city placement
    void BuildNewObjectOnEarth(Vector3 location)
    {
        GameObject NewObject = Instantiate(GameManager.NewObject, location, Quaternion.identity) as GameObject;
        // Make it smaller than the Earth
        NewObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, false);
        // Get a point directly above the city away from earth
        Vector3 awayFromEarth = location - transform.position;
        // assign the up vector for the city
        NewObject.transform.up = awayFromEarth;
    }

    void MainEarthMenu(int windowID)
    {
        if (GUILayout.Button("Buy New City"))
        {
            GameManager.NewObject = CityRef;
            GameManager.isPickingLocation = true;
        }
        if (GUILayout.Button("Buy New Laser Turret"))
        {
            GameManager.NewObject = LaserTurretRef;
            GameManager.isPickingLocation = true;
        }
        foreach (City city in Cities)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("City at Location: " + city.transform.position);
            GUILayout.EndHorizontal();
        }
        // Bottom save and continue button
        if (GUILayout.Button("Save And Continue"))
        {
            GameManager.Resume();
        }
    }
}