using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{
    public static int DeadAlienCount = 0;
    private static int waveSize = 3;

    static Object shipRef;
    static GameObject earth;
    public static List<GameObject> Aliens = new List<GameObject>();
    static List<GameObject> InactiveAliens = new List<GameObject>();

    public static void AddAlien(GameObject alien)
    {
        AlienSpawner.Aliens.Add(alien);
    }
    public static void RemovAlien(GameObject alien)
    {
        DeadAlienCount++;
        alien.SetActive(false);
        InactiveAliens.Add(alien);
    }

    void Start()
    {
        earth = GameObject.Find("Earth");
        shipRef = Resources.Load("AlienShip");
    }

    public static void BeginInvasion()
    {
        for (int i = 0; i < waveSize; i++)
        {
            AddAlien(NewAlienShip());
        }
    }

    static GameObject NewAlienShip()
    {
        // Pick a random spawn location
        Vector3 randomSpawnPoint = RandomCoord(250, 320);
        if (InactiveAliens.Count > 0)
        {
            GameObject existingAlienShip = InactiveAliens[0];
            existingAlienShip.GetComponent<AlienShip>().Health = 100;
            existingAlienShip.transform.position = randomSpawnPoint;
            existingAlienShip.SetActive(true);
            InactiveAliens.RemoveAt(0);
            return existingAlienShip;
        }
        else
        {
            //Create a new alien ship at the random point
            GameObject newAlienShip = Instantiate(shipRef, randomSpawnPoint, Quaternion.identity) as GameObject;
            // This makes the alien live in the same coordinate space as the Earth
            newAlienShip.transform.SetParent(earth.transform, true);
            newAlienShip.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            return newAlienShip;
        }
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
        return new Vector3(RandomCoordNum(min, max), RandomCoordNum(min, max), RandomCoordNum(min, max));
    }
}
