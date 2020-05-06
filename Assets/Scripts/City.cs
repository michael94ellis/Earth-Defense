using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, ZoneBuilding, MenuDisplayItem
{
    public bool isActive { get; set; } = false;
    public Transform buildingTransform { get { return transform; } }
    public EarthZone ParentZone { get; set; }
    public ZoneBuildingType buildingType { get; set; }
    public float MaxPopulation = 500000;
    public float PopulationRegenRate = 1.000001f;
    public float Population { get; private set; } = 50000;

    public string Title { get { return "City"; } }
    public string InfoText
    {
        get
        {
            return "Population: " + Population + "\n" +
                "Max Pop.: " + MaxPopulation + "\n" +
                "Regen Rate: " + PopulationRegenRate;
        }
    }
}