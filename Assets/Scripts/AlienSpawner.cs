using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{
    public Object shipRef;
    private int activeAliens = 12;
    private GameObject earth;

    private static List<GameObject> Aliens = new List<GameObject>();
    public static void AddAlien(GameObject alien)
    {
        AlienSpawner.Aliens.Add(alien);
    }
    public static void RemovAlien(GameObject alien)
    {
        AlienSpawner.Aliens.Remove(alien);
        if (Aliens.Count == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void Start()
    {
        earth = GameObject.Find("Earth");
        shipRef = Resources.Load("AlienShip");
    }

    void Update()
    {
    }

    GameObject NewAlienShip()
    {
        // Pick a random spawn location
        Vector3 randomSpawnPoint = new Vector3(RandomCoord(40,50), RandomCoord(40,50), RandomCoord(40,50));
        //Create a new alien ship in at the random point
        GameObject newAlienShip = Instantiate(shipRef, randomSpawnPoint, Quaternion.identity) as GameObject;
        // This makes the alien live in the same coordinate space as the Earth
        newAlienShip.transform.SetParent(earth.transform, true);
        return newAlienShip;
    }

    // Returns a random value in the range, 50% change of being negative
    int RandomCoord(int min, int max)
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
