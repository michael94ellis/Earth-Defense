using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, ZoneBuilding
{
    private EarthZone _ParentZone;
    public EarthZone ParentZone { get { return _ParentZone; } set { _ParentZone = value; } }
    public float MaxPopulation = 500000;
    public float PopulationRegenRate = 1.000001f;
    public float Population { get; private set; } = 50000;
}