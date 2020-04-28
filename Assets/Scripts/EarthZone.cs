using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthZone : MonoBehaviour, Damageable
{
    private MeshRenderer shield;
    public City Capitol;
    public List<City> MinorCities = new List<City>();
    public List<Weapon> Weapons = new List<Weapon>();
    public int ShieldHealth { get; private set; } = 500;
    public int Population { get; private set; } = 500;
    public bool TakeDamage()
    {
        //Debug.Log("Damage");
        if (ShieldHealth == 0)
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
        ShieldHealth--;
        if (ShieldHealth == 0)
            shield.enabled = false;
        return true;
    }

    public void RegenerateShield()
    {
        shield.enabled = true;
    }

    void Start()
    {
        shield = GetComponent<MeshRenderer>();
    }
}
