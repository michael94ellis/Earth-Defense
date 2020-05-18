using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthZone : MonoBehaviour, Damageable
{
    // This is the reference to the object that represents the shield, enable/disable to show and hide shield
    public MeshRenderer Shield;
    // Each earthzone has a capitol in the center of the prefab, this is a reference to its script
    public CapitolCity Capitol;
    // When the player adds things they ought to be in this list
    public List<ZoneBuilding> ZoneBuildings = new List<ZoneBuilding>();
    // Keep track of targets so weapons in city work together better
    public List<GameObject> ActiveTargets = new List<GameObject>();

    public GameObject HealthBar;
    public GameObject ShieldBar;
    public float MaxPopulation = 500000;
    private float _Population = 100000;
    public float PopulationRegenRate = 0.03f;
    public float Population
    {
        get
        {
            return _Population;
        }
        set
        {
            if (value <= 0)
                _Population = 0;
            else if (value > MaxPopulation)
                _Population = MaxPopulation;
            else
                _Population = value;
            UpdateHealthbar();
        }
    }
    public float MaxShieldHealth = 10000;
    private float _ShieldHealth = 0;
    public float ShieldHealth
    {
        get
        {
            return _ShieldHealth;
        }
        set
        {
            if (value <= 0)
                _ShieldHealth = 0;
            else if (value > MaxShieldHealth)
                _ShieldHealth = MaxShieldHealth;
            else
                _ShieldHealth = value;
            UpdateHealthbar();
        }
    }

    public bool TakeDamage(int amount = 1)
    {
        if (ShieldHealth <= 0)
        {
            if (Population > 0)
                Population--;
            else
                return false;
        }
        else
        {
            ShieldHealth -= amount;
            if (ShieldHealth <= 0)
            {
                Shield.enabled = false;
            }
        }
        return true;
    }

    public float MaxPowerUnits = 1000f;
    private float _PowerUnits = 100f;
    public float PowerUnits
    {
        get
        {
            return _PowerUnits;
        }
        set
        {
            if (value <= 0)
                _PowerUnits = 0;
            else if (value > MaxPowerUnits)
                _PowerUnits = MaxPowerUnits;
            else
                _PowerUnits = value;
        }
    }

    void UpdateHealthbar()
    {
        float populationPercentage = (Population / (ShieldHealth + MaxPopulation)) / 2;
        float shieldHealthPercentage = ShieldHealth / (ShieldHealth + MaxPopulation) / 2;
        RectTransform healthBarRect = HealthBar.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2(populationPercentage, healthBarRect.rect.height);
        RectTransform shieldBarRect = ShieldBar.GetComponent<RectTransform>();
        shieldBarRect.sizeDelta = new Vector2(shieldHealthPercentage, healthBarRect.rect.height);
        shieldBarRect.anchoredPosition = new Vector3(-1 * populationPercentage, shieldBarRect.anchoredPosition.y);
    }

    void Start()
    {
        UpdateHealthbar();
    }
}
