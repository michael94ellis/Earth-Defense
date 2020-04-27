using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is instantiated by the Earth class
// It holds important vars for the menu separate from the earth object
public class MenuManager: MonoBehaviour
{
    public bool Paused = true;
    public bool isPickingLocation = false;
    public int windowWidth = Screen.width * 4 / 6;
    public int windowHeight = Screen.height * 4 / 6;
    public int windowOriginX = Screen.width / 6;
    public int windowOriginY = Screen.height / 6;
    Earth earth;
    Rect placementLabelRect;
    private float secondsCount;
    private int minuteCount;
    private int hourCount;

    public enum MenuScreen
    {
        NewGame,
        MainMenu,
        GameOver
    }
    public MenuScreen CurrentScreen = MenuManager.MenuScreen.NewGame;

    void Start()
    {
        Time.timeScale = 0;
        earth = GameObject.Find("Earth").GetComponent<Earth>();
        // Pause the game so the player starts in the Menu Screen - OnGUI() method
    }

    /// MARK: - Pause/Resume

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
    public Rect GetTopLabelRect(string newLabel, int padding = 5)
    {
        // Determine how large the label will be with the text
        Vector2 labelSize = HeaderStyle.CalcSize(new GUIContent(newLabel));
        return new Rect(Screen.width / 2 - ((labelSize.x + padding) / 2), 20 + padding, labelSize.x + padding, labelSize.y + padding);
    }

    public void UpdateTimer()
    {
        //set timer UI
        secondsCount += Time.deltaTime;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            hourCount++;
            minuteCount = 0;
        }
    }

    void OnGUI()
    {
        UpdateTimer();
        GUI.Label(new Rect(Screen.width - 120, 20, 80, 40), hourCount + ":" + minuteCount + ":" + (int)secondsCount, InfoStyle);
        GUI.Label(new Rect(65, 30, 120, 40),
            "Alien Kill Count: " + AlienSpawner.DeadAlienCount + "\n" +
            "Earth Cities: " + Earth.Cities.Count + "\n" +
            "Global Wealth: $" + Earth.GlobalCurrency + "M", HeaderStyle);
        if (isPickingLocation)
        {
            GUI.Label(GetTopLabelRect("Right Click to Place"), "Right Click to Place", HeaderStyle);
        }

        // Only show menus if the game is paused
        if (Paused && !isPickingLocation)
        {
            switch (CurrentScreen)
            {
                case MenuManager.MenuScreen.NewGame:
                    GUILayout.Window(0, new Rect(windowOriginX, windowOriginY, windowWidth, windowHeight), NewGameScreen, "");
                    break;
                case MenuManager.MenuScreen.MainMenu:
                    GUILayout.Window(0, new Rect(windowOriginX, windowOriginY, windowWidth, windowHeight), MainEarthMenu, "");
                    break;
            }
        }
    }

    void NewGameScreen(int windowID)
    {
        GUILayout.Label("Welcome to Earth in the distant future, the year is 2029.", HeaderStyle, GUILayout.Height(75));
        GUILayout.Label("You are the Dictator of the North America", Header2Style, GUILayout.Height(40));
        GUILayout.Label("Aliens are coming to attack", Header2Style, GUILayout.Height(40));
        GUILayout.Label("Scroll/Pinch to zoom, click and drag to move the Earth, right click for actions.", Header2Style, GUILayout.Height(40));
        if (GUILayout.Button("Continue", ButtonStyle, GUILayout.Height(75)))
        {
            CurrentScreen = MenuManager.MenuScreen.MainMenu;
            Earth.AddGlobalCurrency(400);
        }
    }

    void MainEarthMenu(int windowID)
    {
        GUILayout.Label("Earth Defense Shop", HeaderStyle, GUILayout.Height(40));
        GUILayout.Label("Global Budget: $" + Earth.GlobalCurrency + " Million", HeaderStyle, GUILayout.Height(75));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        //BuyCityButton();
        BuyLaserTurretButton();
        //BuySatelliteButton();
        GUILayout.EndHorizontal();
        CityList();
        GUILayout.EndVertical();
        AlienWaveButton();
        ResumeGameButton();
    }

    void CityList()
    {
        GUILayout.Label("Earth Zones", HeaderStyle);
        GUILayout.BeginVertical();
        foreach (City city in Earth.Cities)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("North America: ");
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    //void BuyCityButton()
    //{
    //    GUI.enabled = Earth.GlobalCurrency > 100;
    //    GUILayout.BeginVertical();
    //    if (GUILayout.Button("Buy City", ButtonStyle, GUILayout.Height(75)))
    //    {
    //        isPickingLocation = true;
    //        earth.BuildNewCity();
    //    }
    //    GUILayout.Label("Cost: $100M", Header2Style);
    //    GUILayout.Label("Generates Money Over Time \nStarting at $5 Million every 5 seconds \nIncreases by $5 Million up to $50 Million per 5 seconds", BodyStyle);
    //    GUILayout.EndVertical();
    //}

    void BuyLaserTurretButton()
    {
        GUI.enabled = Earth.GlobalCurrency > 75;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy Laser Turret", ButtonStyle, GUILayout.Height(75)))
        {
            isPickingLocation = true;
            placementLabelRect = GetTopLabelRect("Right Click to Place Turret");
            earth.BuildNewWeapon();
        }
        GUILayout.Label("Cost: $75M", Header2Style);
        GUILayout.Label("Shoots any Alien Ships above it \nRecharge time of 3 seconds", BodyStyle);
        GUILayout.Label("", BodyStyle);
        GUILayout.EndVertical();
    }

    //void BuySatelliteButton()
    //{
    //    GUI.enabled = Earth.GlobalCurrency > 75;
    //    GUILayout.BeginVertical();
    //    if (GUILayout.Button("Buy New Satellite", ButtonStyle, GUILayout.Height(75)))
    //    {
    //        earth.BuildNewEarthSatellite();
    //    }
    //    GUILayout.Label("Cost: $75M", Header2Style);
    //    GUILayout.Label("Orbits Earth, look cool \nDoes Nothing Else Yet", BodyStyle);
    //    GUILayout.EndVertical();
    //}

    void ResumeGameButton()
    {
        GUI.enabled = true;
        if (GUILayout.Button("Resume Game", ButtonStyle, GUILayout.Height(75)))
        {
            Resume();
        }
    }

    void AlienWaveButton()
    {
        GUI.enabled = true;
        if (GUILayout.Button("Send Alien Wave", ButtonStyle, GUILayout.Height(75)))
        {
            AlienSpawner.BeginInvasion();
        }
    }


    /// MARK: - UI Elements

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
    private GUIStyle _Header2Style;
    public GUIStyle Header2Style
    {
        get
        {
            if (_Header2Style == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 22
                };
                newStyle.normal.textColor = Color.white;
                _Header2Style = newStyle;
            }
            return _Header2Style;
        }
    }

    private GUIStyle _BodyStyle;
    public GUIStyle BodyStyle
    {
        get
        {
            if (_BodyStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 18,
                    wordWrap = true
                };
                newStyle.normal.textColor = Color.white;
                _BodyStyle = newStyle;
            }
            return _BodyStyle;
        }
    }

    private GUIStyle _InfoStyle;
    public GUIStyle InfoStyle
    {
        get
        {
            if (_InfoStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 26,
                };
                newStyle.normal.textColor = Color.white;
                _InfoStyle = newStyle;
            }
            return _InfoStyle;
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