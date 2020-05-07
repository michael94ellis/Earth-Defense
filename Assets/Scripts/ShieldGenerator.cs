using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour, IZoneable, IMenuDisplayable
{
    public bool isActive { get; set; } = false;
    public Transform buildingTransform { get { return transform; } }
    public EarthZone _ParentZone;
    public EarthZone ParentZone { get; set; }
    public ZoneBuildingType buildingType { get; set; }
    public float rechargeTime = 5;
    public bool shieldIsCharged = true;
    public float shieldRegenRate = 1f;
    public float ShieldBoost = 3000;

    public string Title { get { return "Shield Generator"; } }
    public string InfoText
    {
        get
        {
            return "Shield Level: " + ShieldBoost + "\n" +
                "Regen Rate: " + shieldRegenRate + "\n" +
                "Charged: " + shieldIsCharged;
        }
    }
    private List<BuildingUpgrade> ShieldUpgrades = new List<BuildingUpgrade>();
    public List<BuildingUpgrade> upgrades { get { return ShieldUpgrades; } }

    void DecreaseShieldRecharge()
    {
        rechargeTime *= 0.9f;
    }

    void Start()
    {
        BuildingUpgrade ShieldRechargeUpgrade = new BuildingUpgrade();
        ShieldRechargeUpgrade.name = "Decrease Shield Recharge Time";
        ShieldRechargeUpgrade.performUpgrade = DecreaseShieldRecharge;
        ShieldUpgrades.Add(ShieldRechargeUpgrade);
    }

    void Update()
    {
        if (ParentZone != null)
        {
            if (ParentZone.Shield.enabled)
            {
                ParentZone.ShieldHealth += shieldRegenRate * Time.deltaTime;
            }
            else if (shieldIsCharged)
            {
                ParentZone.Shield.enabled = true;
            }
        }
    }
    public void ResetShield()
    {
        ParentZone.Shield.enabled = true;
        ParentZone.ShieldHealth += ShieldBoost;
    }
    public void DoubleShieldRegenRate()
    {
        shieldRegenRate *= 2;
    }
    public void BoostStrength()
    {
        ShieldBoost += 1000;
    }
    public void ReduceRechargeTime()
    {
        if (rechargeTime > 1)
            rechargeTime -= 1;
    }
    /// Must be called like so: StartCoroutine(Recharge());
    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        Debug.Log("Restore shield");
        shieldIsCharged = true;
        ParentZone.ShieldHealth += ShieldBoost;
    }
}
