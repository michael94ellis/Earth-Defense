﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour, ZoneBuilding
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
            stats.Add(new BuildingStat("Shield Health", ParentZone.ShieldHealth));
            stats.Add(new BuildingStat("Regen Rate", shieldRegenRate));
            stats.Add(new BuildingStat("Recharge Time", shieldRechargeTime));
            return stats;
        }
    }

    public float shieldRechargeTime;
    public float shieldRegenRate;
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
