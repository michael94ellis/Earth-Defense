﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : MonoBehaviour, ZoneBuilding, MenuDisplayItem
{
    public bool isActive { get; set; } = false;
    public Transform buildingTransform { get { return transform; } }

    public EarthZone Zone;
    public EarthZone ParentZone { get => Zone; set => Zone = value; }
    public ZoneBuildingType buildingType { get; set; }
    public int _PowerCost;
    public int PowerCost { get => _PowerCost; set => _PowerCost = value; }
    public int _PopulationCost;
    public int PopulationCost { get => _PopulationCost; set => _PopulationCost = value; }

    public int MaxPowerGeneration;
    public int _PowerGeneration;
    public int PowerGeneration
    {
        get {
            return _PowerGeneration;
        }
        set
        {
            if (value <= MaxPowerGeneration)
                _PowerGeneration = 0;
            else if (value >= MaxPowerGeneration)
                _PowerGeneration = MaxPowerGeneration;
            else
                _PowerGeneration = value;
        }
    }

    public string Title { get { return "Power Plant"; } }
    public string InfoText
    {
        get
        {
            return "TBD";
        }
    }
    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();
}