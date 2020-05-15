using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour, ZoneBuilding, MenuDisplayItem
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
            BuildingStat MaxPopulationStat = new BuildingStat("Max Population", ParentZone.Population, ParentZone.MaxPopulation);
            BuildingStat PopulationRegenStat = new BuildingStat("Population Regen", ParentZone.PopulationRegenRate, 10f);
            BuildingStat IncomeStat = new BuildingStat("Income Generated", 2000, 10000f);
            stats.Add(MaxPopulationStat);
            stats.Add(PopulationRegenStat);
            stats.Add(IncomeStat);
            return stats;
        }
    }

    public float shieldRechargeTime;
    public float shieldRegenRate;

    public string Title { get { return "Shield Generator"; } }
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
        HandleUpgrades();
    }

    void HandleUpgrades()
    {
        upgrades.Add(new BuildingUpgrade("Decrease Shield Recharge Time", DecreaseShieldRechargeTime));
    }

    void BuyShield()
    {
        ParentZone.Shield.enabled = true;
        ParentZone.ShieldHealth = ParentZone.MaxShieldHealth / 2;
    }

    void DecreaseShieldRechargeTime()
    {
        ParentZone.Shield.enabled = true;
    }

    // Must be called like so: StartCoroutine(Recharge());
    public IEnumerator RechargeShield()
    {
        yield return new WaitForSeconds(shieldRechargeTime);
        Debug.Log("Restore shield");
        ParentZone.ShieldHealth += 1000;
    }
}
