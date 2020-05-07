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

    public float shieldRechargeTime = 5;
    public float shieldRegenRate = 1f;
    public bool shieldIsPurchased = false;

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

    void BuyShield()
    {
        ParentZone.Shield.enabled = true;
        ParentZone.ShieldHealth = ParentZone.MaxShieldHealth / 2;
        shieldIsPurchased = true;
    }
    void DecreaseShieldRechargeTime()
    {
        ParentZone.Shield.enabled = true;
        shieldIsPurchased = true;
    }
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
        AddUpgrade("Increase Max Pop.", IncreaseMaxPop);
        AddUpgrade("Increase Pop. Regen", IncreasePopRegen);
        if (!shieldIsPurchased)
        {
            AddUpgrade("Buy Shield", BuyShield);
        }
        else
        {
            AddUpgrade("Decrease Shield Recharge Time", DecreaseShieldRechargeTime);
        }
    }

    void AddUpgrade(string UpgradeName, BuildingUpgrade.UpgradeDelegate UpgradeFunc)
    {
        BuildingUpgrade someUpgrade = new BuildingUpgrade();
        someUpgrade.name = UpgradeName;
        someUpgrade.performUpgrade = UpgradeFunc;
        upgrades.Add(someUpgrade);
    }

    /// Must be called like so: StartCoroutine(Recharge());
    public IEnumerator RechargeShield()
    {
        if (shieldIsPurchased)
        {
            yield return new WaitForSeconds(shieldRechargeTime);
            Debug.Log("Restore shield");
            ParentZone.ShieldHealth += 1000;
        }
    }
}