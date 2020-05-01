using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthZone : MonoBehaviour, Damageable
{
    public MeshRenderer Shield;
    public City Capitol;
    public ShieldGenerator ShieldGenerator;
    public List<GameObject> ZoneBuildings = new List<GameObject>();

    public GameObject HealthBar;
    public float MaxShieldHealth = 100000;
    public float MaxPopulation = 500000;
    public float PopulationRegenRate = 1.000001f;
    public float Population { get; private set; } = 500000;
    private float _ShieldHealth = 10000;
    public float ShieldHealth
    {
        get
        {
            return _ShieldHealth;
        }
        private set
        {
            if (value <= 0)
                _ShieldHealth = 0;
            else
                _ShieldHealth = value;
            var theBarRectTransform = HealthBar.transform as RectTransform;
            theBarRectTransform.sizeDelta = new Vector2(value / MaxShieldHealth, 0.1f);
        }
    }

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
