using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private GUIStyle _HeaderStyle;
    public GUIStyle HeaderStyle
    {
        get
        {
            if (_HeaderStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 24
                };
                newStyle.normal.textColor = Color.white;
                _HeaderStyle = newStyle;
            }
            return _HeaderStyle;
        }
    }

    private GUIStyle _ButtonStyle;
    public GUIStyle ButtonStyle
    {
        get
        {
            if (_ButtonStyle == null)
            {
                GUIStyle newStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 22
                };
                newStyle.normal.textColor = Color.white;
                _ButtonStyle = newStyle;
            }
            return _ButtonStyle;
        }
    }
}

public class Earth : MonoBehaviour
{
    public static MenuManager GameManager;
    Object CityRef;
    Object LaserTurretRef;
    Object SatelliteRef;
    // Used to display the currently viewed city in GUI
    List<City> Cities;
    // City is a square
    string newCityName;
    private static float globalcurrency;
    public static float GlobalCurrency
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
        SatelliteRef = Resources.Load("EarthSatellite");
        Cities = new List<City>();
        // Init the world
        globalcurrency = 1000;
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
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), NewGameScreen, "");
                    break;
                case MenuManager.MenuScreen.MainMenu:
                    GUILayout.Window(0, new Rect(GameManager.windowOriginX, GameManager.windowOriginY, GameManager.windowWidth, GameManager.windowHeight), MainEarthMenu, "");
                    break;
            }
        }
    }

    void NewGameScreen(int windowID)
    {
        GUILayout.Label("Welcome To The Game!", GameManager.HeaderStyle, GUILayout.Height(75));
        if (GUILayout.Button("Click here to begin", GameManager.ButtonStyle, GUILayout.Height(75)))
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
        if (GameManager.NewObject == LaserTurretRef)
        {
            NewObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        }
        else
        {
            NewObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        }
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewObject.transform.SetParent(transform, false);
        // Get a point directly above the city away from earth
        Vector3 awayFromEarth = location - transform.position;
        // assign the up vector for the city
        NewObject.transform.up = awayFromEarth;
        // Reset this
        GameManager.NewObject = null;
    }

    void BuildNewEarthSatellite()
    {
        GameObject NewSatellite = Instantiate(SatelliteRef, new Vector3(RandomCoord(0.4f,0.7f), RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f)), Quaternion.identity) as GameObject;
        // Make it smaller than the Earth
        NewSatellite.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewSatellite.transform.SetParent(transform, false);
        // Get a point directly above the city away from earth
        Vector3 awayFromEarth = NewSatellite.transform.position - transform.position;
        // assign the up vector for the city
        NewSatellite.transform.up = awayFromEarth;
    }

    void MainEarthMenu(int windowID)
    {
        GUILayout.Label("Earth Defense Shop", GameManager.HeaderStyle, GUILayout.Height(40));
        GUILayout.Label("Global Budget: $" + GlobalCurrency + " Million", GameManager.HeaderStyle, GUILayout.Height(75));
        GUILayout.BeginHorizontal(GUILayout.Height(75));
        BuyCityButton();
        BuyLaserTurretButton();
        BuySatelliteButton();
        GUILayout.EndHorizontal();
        ResumeGameButton();
    }

    void ResumeGameButton()
    {
        GUI.enabled = true;
        if (GUILayout.Button("Resume Game", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameManager.Resume();
        }
    }

    void BuyCityButton()
    {
        GUI.enabled = GlobalCurrency > 100;
        if (GUILayout.Button("Place New City $100M", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameManager.NewObject = CityRef;
            GameManager.isPickingLocation = true;
            GlobalCurrency -= 100;
        }
    }

    void BuyLaserTurretButton()
    {
        GUI.enabled = GlobalCurrency > 40;
        if (GUILayout.Button("Buy New Laser Turret $40M", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameManager.NewObject = LaserTurretRef;
            GameManager.isPickingLocation = true;
            GlobalCurrency -= 40;
        }
    }

    void BuySatelliteButton()
    {
        GUI.enabled = GlobalCurrency > 75;
        if (GUILayout.Button("Buy New Satellite $75M", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            BuildNewEarthSatellite();
            GlobalCurrency -= 75;
        }
    }

    // Returns a random value in the range, 50% change of being negative
    float RandomCoord(float min, float max)
    {
        // Sign(+/-): 1 is plus, 2 is minus
        var sign = Random.Range(1, 3);
        var value = Random.Range(min, max);
        if (sign == 2)
        {
            return value * -1;
        }
        else
        {
            return value;
        }
    }
}