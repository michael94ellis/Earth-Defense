using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Earth : MonoBehaviour
{
    public static MenuManager GameManager;
    Object CityRef;
    Object LaserTurretRef;
    Object SatelliteRef;

    public static List<GameObject> Children = new List<GameObject>();

    public static float GlobalCurrency { get; private set; }
    public static void AddGlobalCurrency(float money)
    {
        GlobalCurrency += money;
    }

    void Start()
    {
        CityRef = Resources.Load("City");
        LaserTurretRef = Resources.Load("Turret");
        SatelliteRef = Resources.Load("EarthSatellite");
        // Init the world
        GlobalCurrency = 1000;
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
        Children.Add(NewObject);
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
        Children.Add(NewSatellite);
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
        GUILayout.BeginHorizontal();
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
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy City", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameManager.NewObject = CityRef;
            GameManager.isPickingLocation = true;
            GlobalCurrency -= 100;
        }
        GUILayout.Label("Cost: $100M", GameManager.Header2Style);
        GUILayout.Label("Generates Money Over Time \nStarting at $5 Million every 5 seconds \nIncreases by $50 Million up to $20 Million per 5 seconds", GameManager.BodyStyle);
        GUILayout.EndVertical();
    }

    void BuyLaserTurretButton()
    {
        GUI.enabled = GlobalCurrency > 40;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy Laser Turret", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameManager.NewObject = LaserTurretRef;
            GameManager.isPickingLocation = true;
            GlobalCurrency -= 40;
        }
        GUILayout.Label("Cost: $40M", GameManager.Header2Style);
        GUILayout.Label("Shoots any Alien Ships above it \nRecharge time of 3 seconds", GameManager.BodyStyle);
        GUILayout.Label("", GameManager.BodyStyle);
        GUILayout.EndVertical();
    }

    void BuySatelliteButton()
    {
        GUI.enabled = GlobalCurrency > 75;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy New Satellite", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            BuildNewEarthSatellite();
            GlobalCurrency -= 75;
        }
        GUILayout.Label("Cost: $75M", GameManager.Header2Style);
        GUILayout.Label("Orbits Earth, look cool \nDoes Nothing Else Yet", GameManager.BodyStyle);
        GUILayout.EndVertical();
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