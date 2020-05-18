using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour, ZoneBuilding, MenuDisplayItem
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
            stats.Add(new BuildingStat("Bunker Health", 100));
            return stats;
        }
    }

    public float MaxProtectedPopulation;
    public float _ProtectedPopulation;
    public float ProtectedPopulation
    {
        get
        {
            return _ProtectedPopulation;
        }
        set
        {
            if (value <= 0)
                _ProtectedPopulation = 0;
            else if (value > MaxProtectedPopulation)
                _ProtectedPopulation = MaxProtectedPopulation;
            else
                _ProtectedPopulation = value;
        }
    }

    public string Title { get { return "Bunker"; } }
    public string InfoText
    {
        get
        {
            return "TBD";
        }
    }
    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();

    void Start()
    {
        //HandleUpgrades();
    }
}
