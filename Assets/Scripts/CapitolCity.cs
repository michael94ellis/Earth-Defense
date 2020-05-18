using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitolCity : MonoBehaviour, ZoneBuilding
{
    public bool isActive { get; set; } = false;
    public Transform buildingTransform { get { return transform; } }

    public EarthZone Zone;
    public EarthZone ParentZone { get => Zone; set => Zone = value; }
    public ZoneBuildingType buildingType { get => ZoneBuildingType.Capitol; set => buildingType = ZoneBuildingType.Capitol; }
    public int _PowerCost;
    public int PowerCost { get => _PowerCost; set => _PowerCost = value; }
    public int _PopulationCost;
    public int PopulationCost { get => _PopulationCost; set => _PopulationCost = value; }

    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();

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

    void Start()
    {
        HandleUpgrade();
    }

    void HandleUpgrade()
    {
        upgrades.Add(new BuildingUpgrade("Increase Max Pop.", IncreaseMaxPop));
        upgrades.Add(new BuildingUpgrade("Increase Pop. Regen", IncreasePopRegen));
    }

    void IncreaseMaxPop()
    {
        ParentZone.MaxPopulation += 1000;
    }

    void IncreasePopRegen()
    {
        ParentZone.PopulationRegenRate *= 1.01f;
    }
}