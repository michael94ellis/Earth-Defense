using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface Damageable
{
    bool TakeDamage();
}

public class Earth : MonoBehaviour
{
    public static MenuManager GameManager;
    Object CityRef;
    Object LaserTurretRef;
    Object SatelliteRef;
    private Touch touchOne;   //First touch position
    private Touch touchTwo;   //Last touch position
    private float minimumDragDistance = 20f;
    float mainSpeed = 0.5f; //regular speed

    public static List<City> Cities = new List<City>();

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
        GlobalCurrency = 0;
        // Set up the game manager for beginning of game(will change when gameplay changes)
        GameManager = new MenuManager();
        GameManager.CurrentScreen = MenuManager.MenuScreen.NewGame;
        // Pause the game so the player starts in the Menu Screen - OnGUI() method
        GameManager.Paused = true;
    }
    public Vector2 startPos;
    public Vector2 direction;

    void Update()
    {
        if (GameManager.isPickingLocation)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                // This hit.point is the point on earth where you clicked
                if (hit.transform.gameObject == this.gameObject)
                {
                    // Get a point directly above the city away from earth
                    Vector3 awayFromEarth = hit.point - transform.position;
                    // assign the up vector for the city
                    GameManager.NewObject.transform.up = awayFromEarth;
                    GameManager.NewObject.transform.position = hit.point;
                    return;
                }
            }
            return;
        }
        // Earth rotation
        transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * 30);

        if (SystemInfo.deviceType == DeviceType.Desktop)
            return;
        // Touch controls for mobile only
        if (Input.touchCount == 2)
        {
            Debug.Log("Pinch");
            touchOne = Input.GetTouch(0);
            touchTwo = Input.GetTouch(1);
            float previousMagnitude = ((touchOne.position - touchOne.deltaPosition) - (touchTwo.position - touchTwo.deltaPosition)).magnitude; // pop pop
            float currentMagnitude = (touchOne.position - touchTwo.position).magnitude;
            Vector3 Direction = currentMagnitude > previousMagnitude ? Vector3.zero : Camera.main.transform.position * 2;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, Direction, mainSpeed * Time.deltaTime);
        }
        else if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch currentTouch = Input.GetTouch(0);
            if (currentTouch.phase == TouchPhase.Began)
            {
                //Debug.Log("Begin Touches");
                touchOne = currentTouch;
            }
            else if (currentTouch.deltaPosition.magnitude > 0)
            {
                if (currentTouch.phase == TouchPhase.Moved)
                {
                    //Debug.Log("Swiping");
                    touchTwo = currentTouch;  //last touch position. Ommitted if you use list

                    if (Mathf.Abs(touchTwo.position.x - touchOne.position.x) > minimumDragDistance || Mathf.Abs(touchTwo.position.y - touchOne.position.y) > minimumDragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(touchTwo.position.x - touchOne.position.x) > Mathf.Abs(touchTwo.position.y - touchOne.position.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((touchTwo.position.x > touchOne.position.x))  //If the movement was to the right)
                            {   //Right swipe
                                //Debug.Log("Right Swipe");
                                Camera.main.transform.RotateAround(Vector3.zero, Vector3.down, mainSpeed);
                            }
                            else
                            {   //Left swipe
                                //Debug.Log("Left Swipe");
                                Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, mainSpeed);
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (touchTwo.position.y > touchOne.position.y)  //If the movement was up
                            {   //Up swipe
                                //Debug.Log("Up Swipe");
                                Camera.main.transform.RotateAround(Vector3.zero, Vector3.left, mainSpeed);
                            }
                            else
                            {   //Down swipe
                                //Debug.Log("Down Swipe");
                                Camera.main.transform.RotateAround(Vector3.zero, Vector3.right, mainSpeed);
                            }
                        }
                    }
                }
            }
            else if (currentTouch.phase == TouchPhase.Ended) 
            {
                //Debug.Log("Tap");
                //Only Pause if not already paused, menu must have unpause button
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
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(ray);
                    foreach (RaycastHit hit in hits)
                    {
                        GameManager.isPickingLocation = false;
                        // This hit.point is the point on earth where you clicked
                        if (hit.transform.gameObject == this.gameObject)
                        {
                            Debug.Log(hit.point);
                            //DisplayNewObjectOnEarth(hit.point);
                        }
                    }
                }
            }
        }
    }
    // For PC/Mac
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
            GameManager.isPickingLocation = false;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(65, 30, 120, 40),
            "Alien Kill Count: " + AlienSpawner.DeadAlienCount + "\n" +
            "Earth Cities: " + Cities.Count + "\n" +
            "Global Wealth: $" + GlobalCurrency + "M", GameManager.HeaderStyle);

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
        if (GlobalCurrency == 0)
        {
            GUILayout.Label("Welcome To Earth, you are an exiled alien in disguise and have just taken control of the World!", GameManager.HeaderStyle, GUILayout.Height(75));
            if (GUILayout.Button("Continue", GameManager.ButtonStyle, GUILayout.Height(75)))
            {
                GlobalCurrency = 1000;
            }
        }
        else
        {
            GUILayout.Label("You notice something in the sky", GameManager.HeaderStyle, GUILayout.Height(75));
            if (GUILayout.Button("You've angered the Aliens living in Uranus.\nHere's some money to defend Earth!", GameManager.ButtonStyle, GUILayout.Height(75)))
            {
                GameManager.CurrentScreen = MenuManager.MenuScreen.MainMenu;
            }
        }
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

    void BuyCityButton()
    {
        GUI.enabled = GlobalCurrency > 100;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy City", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameObject NewObject = Instantiate(CityRef) as GameObject;
            GameManager.NewObject = NewObject;
            // Make the new city a child object so it lives inside the earth's coordinate space
            GameManager.NewObject.transform.SetParent(transform, true);
            City newCity = GameManager.NewObject.GetComponent<City>();
            if (newCity != null)
                Cities.Add(newCity);
            GameManager.Pause();
            GameManager.NewObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            GameManager.isPickingLocation = true;
            GlobalCurrency -= 100;
        }
        GUILayout.Label("Cost: $100M", GameManager.Header2Style);
        GUILayout.Label("Generates Money Over Time \nStarting at $5 Million every 5 seconds \nIncreases by $5 Million up to $50 Million per 5 seconds", GameManager.BodyStyle);
        GUILayout.EndVertical();
    }

    void BuyLaserTurretButton()
    {
        GUI.enabled = GlobalCurrency > 75;
        GUILayout.BeginVertical();
        if (GUILayout.Button("Buy Laser Turret", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameObject NewObject = Instantiate(LaserTurretRef) as GameObject;
            GameManager.NewObject = NewObject;
            // Make the new city a child object so it lives inside the earth's coordinate space
            GameManager.NewObject.transform.SetParent(transform, true);
            NewObject.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
            GameManager.isPickingLocation = true;
            GlobalCurrency -= 75;
            GameManager.Pause();
        }
        GUILayout.Label("Cost: $75M", GameManager.Header2Style);
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

    void BuildNewEarthSatellite()
    {
        GameObject NewSatellite = Instantiate(SatelliteRef, new Vector3(RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f), RandomCoord(0.4f, 0.7f)), Quaternion.identity) as GameObject;
        // Make it smaller than the Earth
        NewSatellite.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        // Make the new city a child object so it lives inside the earth's coordinate space
        NewSatellite.transform.SetParent(transform, false);
        // Get a point directly above the city away from earth
        Vector3 awayFromEarth = NewSatellite.transform.position - transform.position;
        // assign the up vector for the city
        NewSatellite.transform.up = awayFromEarth;
    }

    void ResumeGameButton()
    {
        GUI.enabled = true;
        if (GUILayout.Button("Resume Game", GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            GameManager.Resume();
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