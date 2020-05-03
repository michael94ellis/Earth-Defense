using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, ZoneBuilding
{
    public bool isActive { get; set; } = false;
    public EarthZone ParentZone { get; set; }
    public float MaxPopulation = 500000;
    public float PopulationRegenRate = 1.000001f;
    public float Population { get; private set; } = 50000;


}