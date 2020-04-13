using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    public List<GameObject> alienShips = new List<GameObject>();
    public List<GameObject> cities = new List<GameObject>();
    public Object shipRef;
    public Object cityRef;
    public Vector3 origin = new Vector3(0, 0, 0);
    public int count = 0;

    void Awake()
    {
        shipRef = Resources.Load("AlienShip");
        cityRef = Resources.Load("City");
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject newAlienShip = Instantiate(shipRef, new Vector3(12, 12, 12), Quaternion.identity) as GameObject;
        newAlienShip.transform.SetParent(transform, true);
        alienShips.Add(newAlienShip);
    }

    void Update()
    {

    }

    void OnMouseUp()
    {
        // If your mouse hovers over the GameObject with the script attached, output this message
        Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray))
        {

        }
    }
}