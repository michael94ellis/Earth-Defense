using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthZone : MonoBehaviour, Damageable
{
    public MeshRenderer Shield;
    public City Capitol;
    public ShieldGenerator ShieldGenerator;
    public List<Weapon> Weapons = new List<Weapon>();
    public List<City> MinorCities = new List<City>();

    public float MaxShieldHealth = 10000;
    public float MaxPopulation = 500000;
    public float PopulationRegenRate = 1.000001f;
    public float Population { get; private set; } = 50000;
    public float ShieldHealth { get; private set; } = 10000;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (Population < MaxPopulation)
                Population *= PopulationRegenRate;
        }
    }

    public void AddShieldHealth(float amount)
    {
        if (ShieldHealth <= MaxShieldHealth)
        {
            if (ShieldHealth <= 0)
                ShieldHealth = amount;
            else
                ShieldHealth += amount;
        }
    }

    public void AddPeople(float amount)
    {
        Population += amount;
    }

    public bool TakeDamage(int amount = 1)
    {
        //Debug.Log("Damage");
        if (ShieldHealth <= 0)
        {
            if (Population > 0)
            {
                Population--;
                return true;
            }
            else
            {
                return false;
            }
        }
        ShieldHealth = ShieldHealth - amount;
        if (ShieldHealth <= 0)
        {
            Debug.Log("Disable shield");
            Shield.enabled = false;
            if (ShieldGenerator != null)
            {
                Debug.Log("Trying to recharge shield");
                StartCoroutine(ShieldGenerator.Recharge());
            }
        }
        return true;
    }
}

/*
 Earth Zone
The player must keep at least 1 million people alive, or enough to fight off the aliens

The Zones have a Capitol city in the center that provides an initial 50K people and a shield at the beginning of each wave of aliens
     *      Upgrades:
     *          Population Regen Rate
     *          Maximum Population
     *          Maximum Shield Health
     *          Government
     *              Upgrades:
     *                  Cost to build things
     *                  Initial stats of buildings/weapons
     *                  Building speed
     *                  Unlock level for buildings/weapons

Zones can have weapons to fight off the aliens
     * Laser Turret - Shoots a laser beam at the target
     *      Upgrades:
     *          Firing Range
     *          Firing Duration
     *          Recharge Time
     *          Damage
     *          MultipleTargets
     *          Choosing how it targets enemies
     *          
     * Ballistic Missiles - Missile must reach target location before dealing damage
     *      Upgrades:
     *          Firing Range
     *          Speed
     *          Reload Time
     *          Damage
     *          Area of damage effect
     *          Choosing how it targets enemies

Zones can build their population
    * Minor Cities can be built to increase the Zone's Maximum Population
     *      Upgrades:
     *          Population Regen Rate
     *          Population Capacity(added to max pop. for zone)
     *          Wealth Generation

Zones can have one Additional Shield Generator that will recharge the zone shield when it goes down
     *      Upgrades:
     *          Shield Health Regen Rate(while shield is up)
     *          Max Shield Health(added to max shield health for zone)
     *          Shield recharge time when shield goes down
 */
