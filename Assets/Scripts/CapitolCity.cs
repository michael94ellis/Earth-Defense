using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitolCity : MonoBehaviour, ZoneBuilding, MenuDisplayItem
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

    public string Title { get { return "Capitol City"; } }
    public string InfoText
    {
        get
        {
            return "Population: " + ParentZone.Population + "\n" +
                "Max Pop.: " + ParentZone.MaxPopulation + "\n" +
                "Regen Rate: " + ParentZone.PopulationRegenRate;
        }
    }
    public List<BuildingUpgrade> upgrades { get; } = new List<BuildingUpgrade>();



    void IncreaseMaxPop()
    {
        ParentZone.MaxPopulation += 1000;
    }

    void IncreasePopRegen()
    {
        ParentZone.PopulationRegenRate *= 1.01f;
    }

    void Start()
    {
        HandleUpgrades();
    }

    void HandleUpgrades()
    {
        upgrades.Add(new BuildingUpgrade("Increase Max Pop.", IncreaseMaxPop));
        upgrades.Add(new BuildingUpgrade("Increase Pop. Regen", IncreasePopRegen));
    }
}