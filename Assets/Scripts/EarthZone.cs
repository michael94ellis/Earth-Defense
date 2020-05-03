using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthZone : MonoBehaviour, Damageable
{
    public MeshRenderer Shield;
    public City Capitol;
    public ShieldGenerator ShieldGenerator;
    public List<GameObject> ZoneBuildings = new List<GameObject>();
    // Keep track of targets so weapons in city work together better
    public List<GameObject> ActiveTargets = new List<GameObject>();

    public GameObject HealthBar;
    public GameObject ShieldBar;
    private float MaxPopulation = 5000;
    private float _Population = 5000;
    public float Population
    {
        get
        {
            return _Population;
        }
        private set
        {
            if (value <= 0)
                _Population = 0;
            else if (value > MaxPopulation)
                _Population = MaxPopulation;
            else
                _Population = value;
        }
    }
    public float MaxShieldHealth = 100000;
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
        }
    }

    public bool TakeDamage(int amount = 1)
    {
        //Debug.Log("Damage Taken");
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
                //Debug.Log("Disable shield");
                Shield.enabled = false;
                if (ShieldGenerator != null)
                    StartCoroutine(ShieldGenerator.Recharge());
            }
        }
        UpdateHealthbar();
        return true;
    }

    void UpdateHealthbar()
    {
        float populationPercentage = Population / (ShieldHealth + MaxPopulation) / 2;
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
