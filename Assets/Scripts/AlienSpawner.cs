using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{
    public List<GameObject> Aliens { get; private set; }
    public Object shipRef;
    private int activeAliens = 5;
    private GameObject earth;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Manager Started");
        earth = GameObject.Find("Earth");
        shipRef = Resources.Load("AlienShip");
        Aliens = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Aliens.Count < activeAliens)
        {
            Aliens.Add(NewAlienShip());
        }
    }

    GameObject NewAlienShip()
    {
        Vector3 randomSpawnPoint = new Vector3(RandomCoord(40,50), RandomCoord(40,50), RandomCoord(40,50));
        GameObject newAlienShip = Instantiate(shipRef, randomSpawnPoint, Quaternion.identity) as GameObject;
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
