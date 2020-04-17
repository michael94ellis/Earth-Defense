using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{
    public static Object shipRef;
    private static Earth earth;

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
        earth = GameObject.Find("Earth").GetComponent<Earth>();
        shipRef = Resources.Load("AlienShip");
    }

    void Update()
    {
    }

    public static void SpawnAliens(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Aliens.Add(NewAlienShip());
        }
    }

    private static GameObject NewAlienShip()
    {
        // Pick a random spawn location
        Vector3 randomSpawnPoint = new Vector3(RandomCoord(150,200), RandomCoord(150,200), RandomCoord(150,200));
        //Create a new alien ship in at the random point
        GameObject newAlienShip = Instantiate(shipRef, randomSpawnPoint, Quaternion.identity) as GameObject;
        // This makes the alien live in the same coordinate space as the Earth
        newAlienShip.transform.SetParent(earth.gameObject.transform, true);
        return newAlienShip;
    }

    // Returns a random value in the range, 50% change of being negative
    private static int RandomCoord(int min, int max)
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
