﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, ZoneBuilding
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
    public List<BuildingStat> Stats
    {
        get
        {
            List<BuildingStat> stats = new List<BuildingStat>();
            BuildingStat MaxPopulationStat = new BuildingStat("Max Population", ParentZone.Population);
            BuildingStat PopulationRegenStat = new BuildingStat("Population Regen", ParentZone.PopulationRegenRate);
            BuildingStat IncomeStat = new BuildingStat("Income Generated", 2000);
            stats.Add(MaxPopulationStat);
            stats.Add(PopulationRegenStat);
            stats.Add(IncomeStat);
            return stats;
        }
    }

    public float MaxIncomeGeneration;
    public float _IncomeGeneration;
    public float IncomeGeneration
    {
        get
        {
            return _IncomeGeneration;
        }
        set
        {
            if (value <= MaxIncomeGeneration)
                _IncomeGeneration = 0;
            else if (value >= MaxIncomeGeneration)
                _IncomeGeneration = MaxIncomeGeneration;
            else
                _IncomeGeneration = value;
        }
    }

    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();

    void Start()
    {
        //HandleUpgrades();
    }
}
