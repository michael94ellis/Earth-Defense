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

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.transform.gameObject);
                EarthZone zoneClicked = hit.transform.gameObject.GetComponent<EarthZone>();
                if (zoneClicked != null)
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
            // Only Pause if not already paused, menu must have unpause button
            if (!Paused)
            {
                Pause();
                // Show the user all their cities
                CurrentScreen = MenuManager.MenuScreen.MainMenu;
                // Update and fetch data here, to not run loops like this every frame
            }
            else if (isPickingLocation)
            {
                isPickingLocation = false;
                return;
            }
        }
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

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 120, 20, 80, 40), "Time: " + Time.time, InfoStyle);
        GUI.Label(new Rect(65, 30, 120, 40),
            "Alien Kill Count: " + AlienSpawner.DeadAlienCount + "\n" +
            "Global Wealth: $" + Earth.GlobalCurrency + "M", TopLeftInfoStyle);
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
                    GUILayout.Window(0, new Rect(50, 50, Screen.width - 100, Screen.height - 100), NewGameScreen, "");
                    break;
                case MenuManager.MenuScreen.MainMenu:
                    GUILayout.Window(0, new Rect(50, 50, Screen.width - 100, Screen.height - 100), MainEarthMenu, "");
                    break;
            }
        }
    }

    void NewGameScreen(int windowID)
    {
        GUILayout.Label("The year is 2021", HeaderStyle, GUILayout.Height(75));
        GUILayout.Label("Aliens have destroyed everything, you command whats left of humanity", Header2Style, GUILayout.Height(40));
        GUILayout.Label("They'll attack again, prepare to defend", Header2Style, GUILayout.Height(40));
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
            GUILayout.Label("Assets", HeaderStyle, GUILayout.Height(40));
            GUILayout.BeginHorizontal();
                BuyCityButton();
                //BuyGovernmentSectorButton();
                //BuyIndustrialSectorButton();
                BuyMissileSiloButton();
                BuyLaserTurretButton();
                BuyShieldGeneratorButton();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                CityList();
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        AlienWaveButton();
        ResumeGameButton();
    }

    void CityList()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Earth Zones", HeaderStyle);
        foreach (EarthZone zone in Earth.ControlledZones)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("North American Zone", Header2Style);
            GUILayout.Label("Population " + zone.Population + " / " + zone.MaxPopulation, Header2Style);
            GUILayout.EndHorizontal();
            if (zone.ShieldGenerator != null)
            {
                GUILayout.BeginHorizontal();
                DisplayShieldGeneratorUpgrades(zone);
                GUILayout.EndHorizontal();
            }
            foreach (GameObject building in zone.ZoneBuildings)
            {
                Weapon weapon = building.GetComponent<Weapon>();
                City city = building.GetComponent<City>();
                if (city != null)
                    DisplayMinorCityUpgrades(zone, city);
                if (weapon != null)
                    DisplayWeaponUpgrades(zone, weapon);
            }
        }
        GUILayout.EndVertical();
    }

    void DisplayShieldGeneratorUpgrades(EarthZone zone)
    {
        GUI.enabled = zone.ShieldGenerator.rechargeTime > 1;
        if (GUILayout.Button("Decrease Shield Recharge Time", ButtonStyle))
        {
            zone.ShieldGenerator.ReduceRechargeTime();
        }
        GUI.enabled = zone.ShieldGenerator.ShieldBoost < (zone.MaxShieldHealth / 2);
        if (GUILayout.Button("Boost Shield Strength", ButtonStyle))
        {
            zone.ShieldGenerator.BoostStrength();
        }
        GUI.enabled = zone.ShieldGenerator.shieldRegenRate <= 16;
        if (GUILayout.Button("Double Shield Regen Rate", ButtonStyle))
        {
            zone.ShieldGenerator.DoubleShieldRegenRate();
        }
    }

    void DisplayMinorCityUpgrades(EarthZone zone, City city)
    {
        GUI.enabled = zone.MaxPopulation < 1000000;
        if (GUILayout.Button("Increase Max Population by 50,000", ButtonStyle))
        {
            zone.MaxPopulation += 50000;
        }
        GUI.enabled = true;
        if (GUILayout.Button("Double Population Regen Rate", ButtonStyle))
        {
            city.PopulationRegenRate *= 2;
        }
    }

    void DisplayWeaponUpgrades(EarthZone zone, Weapon weapon)
    {
        GUI.enabled = true;
        if (GUILayout.Button("Increase Weapon Damage", ButtonStyle))
        {
        }
        GUI.enabled = true;
        if (GUILayout.Button("Decrease Recharge/Reload", ButtonStyle))
        {
        }
        GUI.enabled = true;
        if (GUILayout.Button("Boost Firing Range", ButtonStyle))
        {
        }
    }

    void BuyCityButton()
    {
        GUI.enabled = Earth.GlobalCurrency > 100;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy City", ButtonStyle, GUILayout.Height(75)))
        {
            isPickingLocation = true;
            placementLabelRect = GetTopLabelRect("Right Click to Place Residential Sector");
            earth.BuildNewCity();
        }
        GUILayout.Label("Cost: $100M", Header2Style);
        GUILayout.Label("Population Capacity: 100Million", BodyStyle);
        GUILayout.EndVertical();
    }

    //void BuyIndustrialSectorButton()
    //{
    //    GUI.enabled = Earth.GlobalCurrency > 300;
    //    GUILayout.BeginVertical();
    //    if (GUILayout.Button("Buy Industrial Center", ButtonStyle, GUILayout.Height(75)))
    //    {
    //        isPickingLocation = true;
    //        placementLabelRect = GetTopLabelRect("Right Click to Place Industrial Sector");
    //        earth.BuildNewCity();
    //    }
    //    GUILayout.Label("Cost: $300M", Header2Style);
    //    GUILayout.Label("Generates Money Over Time \nStarting at $5 Million every 5 seconds \nIncreases by $5 Million up to $50 Million per 5 seconds\n Population is negatively impacted", BodyStyle);
    //    GUILayout.EndVertical();
    //}

    //void BuyGovernmentSectorButton()
    //{
    //    GUI.enabled = Earth.GlobalCurrency > 300;
    //    GUILayout.BeginVertical();
    //    if (GUILayout.Button("Buy Industrial Center", ButtonStyle, GUILayout.Height(75)))
    //    {
    //        isPickingLocation = true;
    //        placementLabelRect = GetTopLabelRect("Right Click to Place Government Sector");
    //        earth.BuildNewCity();
    //    }
    //    GUILayout.Label("Cost: $100M", Header2Style);
    //    GUILayout.Label("Regenerates Population Over Time \n \n Population is positively impacted", BodyStyle);
    //    GUILayout.EndVertical();
    //}

    void BuyShieldGeneratorButton()
    {
        GUI.enabled = Earth.GlobalCurrency > 300;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy Shield Generator", ButtonStyle, GUILayout.Height(75)))
        {
            isPickingLocation = true;
            placementLabelRect = GetTopLabelRect("Right Click to Place Shield Generator");
            earth.BuildNewShieldGenerator();
        }
        GUILayout.Label("Cost: $800M", Header2Style);
        GUILayout.Label("Shield Regenerates at all times and is stronger", BodyStyle);
        GUILayout.EndVertical();
    }

    void BuyLaserTurretButton()
    {
        GUI.enabled = Earth.GlobalCurrency > 75;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy Laser Turret", ButtonStyle, GUILayout.Height(75)))
        {
            isPickingLocation = true;
            placementLabelRect = GetTopLabelRect("Right Click to Place Laser Turret");
            earth.BuildNewLaserWeapon();
        }
        GUILayout.Label("Cost: $200M", Header2Style);
        GUILayout.Label("Shoots a laser beam \nRecharge time of 3 seconds\n No additional costs", BodyStyle);
        GUILayout.Label("", BodyStyle);
        GUILayout.EndVertical();
    }

    void BuyMissileSiloButton()
    {
        GUI.enabled = Earth.GlobalCurrency > 30;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy Missile Silo", ButtonStyle, GUILayout.Height(75)))
        {
            isPickingLocation = true;
            placementLabelRect = GetTopLabelRect("Right Click to Place Missile Silo");
            earth.BuildNewMissileSilo();
        }
        GUILayout.Label("Cost: $50M", Header2Style);
        GUILayout.Label("Shoots a powerful missile \nRecharge time of 3 seconds \nMissiles cost $1Million each", BodyStyle);
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

    private GUIStyle _TopLeftInfoStyle;
    public GUIStyle TopLeftInfoStyle
    {
        get
        {
            if (_TopLeftInfoStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 30
                };
                newStyle.normal.textColor = Color.white;
                _TopLeftInfoStyle = newStyle;
            }
            return _TopLeftInfoStyle;
        }
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