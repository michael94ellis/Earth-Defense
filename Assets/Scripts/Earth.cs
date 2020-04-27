using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface Damageable
{
    bool TakeDamage();
}

public class EarthZone
{
    public Collider collider;
    public City city;
    public List<Generator> Generator;
    public List<Weapon> weapons;
}

public class Earth : MonoBehaviour
{
    MenuManager GameManager;
    Object CityRef;
    Object LaserTurretRef;
    Object SatelliteRef;
    private Touch touchOne;   //First touch position
    private Touch touchTwo;   //Last touch position
    private float minimumDragDistance = 20f;
    float mainSpeed = 0.5f; //regular speed

    Collider NorthAmerica;
    Collider SouthAmerica;
    public static List<City> Cities = new List<City>();
    public static float GlobalCurrency { get; private set; }
    public static void AddGlobalCurrency(float money)
    {
        GlobalCurrency += money;
    }
    public static void SpendGlobalCurrency(float money)
    {
        GlobalCurrency -= money;
    }

    void Start()
    {
        GameManager = GameObject.Find("Earth").GetComponent<MenuManager>();
        CityRef = Resources.Load("City");
        LaserTurretRef = Resources.Load("Turret");
        SatelliteRef = Resources.Load("EarthSatellite");
        NorthAmerica = GameObject.Find("NorthAmerica").GetComponent<BoxCollider>();
        SouthAmerica = GameObject.Find("SouthAmerica").GetComponent<BoxCollider>();
        GlobalCurrency = 0;
    }
    public Vector2 startPos;
    public Vector2 direction;

    void Update()
    {
        if (Input.GetMouseButton(1))
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
                return;
            }
        }
        if (GameManager.isPickingLocation)
        {
            DisplayNewObjectInNorthAmerica();
            return;
        }
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

    void DisplayNewObjectInNorthAmerica()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            // This hit.point is the point on earth where you clicked
            if (hit.transform.gameObject == this.gameObject)
            {
                if (NorthAmerica.bounds.Contains(hit.point))
                {
                    //Debug.Log(hit.point + " is in North America: " + NorthAmerica.bounds.Contains(hit.point));
                    // Get a point directly above the city away from earth
                    Vector3 awayFromEarth = hit.point - transform.position;
                    // assign the up vector for the city
                    GameManager.NewObject.transform.up = awayFromEarth;
                    GameManager.NewObject.transform.position = hit.point;
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void BuildNewCity()
    {
        GameObject NewObject = Instantiate(CityRef) as GameObject;
        GameManager.NewObject = NewObject;
        // Make the new city a child object so it lives inside the earth's coordinate space
        GameManager.NewObject.transform.SetParent(transform, true);
        City newCity = GameManager.NewObject.GetComponent<City>();
        if (newCity != null)
            Cities.Add(newCity);
        GameManager.NewObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        SpendGlobalCurrency(100);
    }

    public void BuildNewWeapon()
    {
        GameObject NewObject = Instantiate(LaserTurretRef) as GameObject;
        GameManager.NewObject = NewObject;
        // Make the new city a child object so it lives inside the earth's coordinate space
        GameManager.NewObject.transform.SetParent(transform, true);
        NewObject.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
        SpendGlobalCurrency(75);
    }

    public void BuildNewEarthSatellite()
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
        SpendGlobalCurrency(75);
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