using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthZone : MonoBehaviour, Damageable
{
    public MeshRenderer Shield;
    public City Capitol;
    public List<City> MinorCities = new List<City>();
    public ShieldGenerator ShieldGenerator;
    public List<Weapon> Weapons = new List<Weapon>();
    public float MaxShieldHealth = 10000;
    public float ShieldHealth { get; private set; } = 10000;
    public float MaxPopulation = 500000;
    public float Population { get; private set; } = 50000;
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

    void Start()
    {
        Shield = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (Population < MaxPopulation)
                Population *= 1.000001f;
        }
    }
}

/*
 * Earth Zone
 *
 * Player starts with 10 Million Population
 * Population grows by 0.5% Time.deltaTime
 *
 * Non-Weapon Buildings:
 * Capitol City - Base Population 1 Billion
 *      Upgrades:
 *          Population Capacity
 *          Population Regen
 * Additional City - Base Population 500 Million
 *      Upgrades:
 *          Population Capacity
 *          Population Regen
 * Private Sector - Generates Money Quickly, kills a few people in the process
 *      Upgrades:
 *          Generate More Money, Kill More People
 *          Kill Less People, Generate Same Money
 * Public Sector - Generates Money Slowly, boosts population regeneration rate and cap
 *      Upgrades:
 *          Generate More Moeny, Some People Die in a "revolution" or "purge", it can be both(think about dialog boxes)
 * Shield Booster - Grows the size and strength of the shield for the EarthZone
 *      No Upgrades
 *
 * Weapon Buildings:
 * Laser Turret - Shoots a laser beam at the target
 *      Upgrades:
 *          Firing Range
 *          Recharge Time
 *          Damage
 * Ballistic Missiles - Missile must reach target location before dealing damage
 *      Upgrades:
 *          Firing Range
 *          Recharge Time
 *          Damage
 *          Speed
 *          Cost Per Missile
 */
