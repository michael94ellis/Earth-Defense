using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{
    public Object shipRef;
    public static int activeAliens = 12;
    private static bool AliensDefeated = false;
    private GameObject earth;
    // Positions of stuff to shoot at
    public static List<Transform> Targets;

    private static List<GameObject> Aliens = new List<GameObject>();
    public static void AddAlien(GameObject alien)
    {
        AlienSpawner.Aliens.Add(alien);
    }
    public static void RemovAlien(GameObject alien)
    {
        activeAliens--;
        alien.transform.position = RandomCoord(40, 50);
        if (activeAliens <= 0)
        {
            AliensDefeated = true;
        }
    }

    void Start()
    {
        earth = GameObject.Find("Earth");
        shipRef = Resources.Load("AlienShip");
        NewAlienShip();
        NewAlienShip();
        NewAlienShip();
        NewAlienShip();
    }

    void Update()
    {
    }

    void OnGUI()
    {
        if (AliensDefeated)
        {
            Earth.GameManager.Pause();
            GUILayout.Window(0, new Rect(Earth.GameManager.windowOriginX, Earth.GameManager.windowOriginY, Earth.GameManager.windowWidth, Earth.GameManager.windowHeight), PlayerWinScreen, "");
        }
    }

    void PlayerWinScreen(int windowID)
    {
        GUILayout.Label("All the Aliens are dead!", Earth.GameManager.HeaderStyle, GUILayout.Height(75));
        if (GUILayout.Button("Click here to dispose of the excess war supplies", Earth.GameManager.ButtonStyle, GUILayout.Height(75)))
        {
            Earth.Explode();
        }
    }

    GameObject NewAlienShip()
    {
        // Pick a random spawn location
        Vector3 randomSpawnPoint = RandomCoord(40, 50);
        //Create a new alien ship in at the random point
        GameObject newAlienShip = Instantiate(shipRef, randomSpawnPoint, Quaternion.identity) as GameObject;
        // This makes the alien live in the same coordinate space as the Earth
        newAlienShip.transform.SetParent(earth.transform, true);
        newAlienShip.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        return newAlienShip;
    }

    // Returns a random value in the range, 50% change of being negative
    public static float RandomCoordNum(float min, float max)
    {
        // Sign(+/-): 1 is plus, 2 is minus
        int sign = Random.Range(1, 3);
        float value = Random.Range(min, max);
        if (sign == 2)
        {
            return value * -1f;
        }
        else
        {
            return value;
        }
    }

    public static Vector3 RandomCoord(int min, int max)
    {
        return new Vector3(RandomCoordNum(40, 50), RandomCoordNum(40, 50), RandomCoordNum(40, 50));
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator RefreshCitiesList()
    {
        yield return new WaitForSeconds(10);
        Targets = new List<Transform>();
        foreach (Transform child in earth.transform)
        {
            Targets.Add(child);
        }
        RefreshCitiesList();
    }
}
