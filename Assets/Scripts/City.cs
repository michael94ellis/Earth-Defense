using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, Damageable
{
    public string CityName;
    private float currencyGenerationTime = 5f;
    private bool generateCurrency = true;
    float age = 1;
    float maxAge = 10;
    Object DestructionEffect;
    public AudioSource ExplosionSound;
    GameObject Shield;

    public int ShieldHealth { get; private set; } = 0;
    private int BaseHealth = 500;
    public int Health { get; private set; }
    public bool TakeDamage()
    {
        //Debug.Log("Damage");
        if (Health == 0)
        {
            ExplosionSound.Play();
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            DestructionAnimation.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            Destroy(gameObject);
            Earth.Cities.Remove(this);
            return false;
        }
        if (ShieldHealth > 0 && Shield.activeInHierarchy)
            ShieldHealth--;
        else if (Health < BaseHealth && Shield.activeInHierarchy)
            Shield.SetActive(false);
        else
            Health--;
        return true;
    }

    void Start()
    {
        Health = BaseHealth;
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
        AudioSource[] soundSources = gameObject.GetComponents<AudioSource>();
        ExplosionSound = soundSources[0];
        Shield = transform.Find("Shield").gameObject;
        Shield.SetActive(false);
    }

    void Update()
    {
        if (generateCurrency)
        {
            StartCoroutine(GeneratGlobalCurrency());
        }
    }

    public void AddShield()
    {
        Shield.SetActive(true);
        Health += BaseHealth;
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator GeneratGlobalCurrency()
    {
        generateCurrency = false;
        Earth.AddGlobalCurrency(age * 5);
        yield return new WaitForSeconds(currencyGenerationTime);
        if (age < maxAge)
            age++;
        generateCurrency = true;
    }
}