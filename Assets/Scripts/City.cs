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

    public int Health { get; private set; }
    public bool TakeDamage()
    {
        //Debug.Log("Damage");
        Health--;
        if (Health == 0)
        {
            ExplosionSound.Play();
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            DestructionAnimation.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            Destroy(gameObject);
            Earth.Cities.Remove(this);
            return false;
        }
        return true;
    }

    void Start()
    {
        Health = 200;
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
        AudioSource[] soundSources = gameObject.GetComponents<AudioSource>();
        ExplosionSound = soundSources[0];
    }

    void Update()
    {
        if (generateCurrency)
        {
            StartCoroutine(GeneratGlobalCurrency());
        }
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