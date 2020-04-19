using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialBuilding : MonoBehaviour, CityBuilding
{
    private Vector2 position;
    public Vector2 Position { get => position; set => position = value; }
    public BuildingType category => BuildingType.Residential;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
