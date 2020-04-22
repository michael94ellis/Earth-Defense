using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public string CityName;
    private float currencyGenerationTime = 5f;
    private bool generateCurrency = true;
    float age = 1;
    float maxAge = 20;
    Object DestructionEffect;

    public int Health { get; private set; }
    public void TakeDamage()
    {
        //Debug.Log("Damage");
        Health--;
        if (Health == 0)
        {
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
            Earth.Children.Remove(gameObject);
        }
    }

    void Start()
    {
        Health = 200;
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
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
        Earth.AddGlobalCurrency(age);
        yield return new WaitForSeconds(currencyGenerationTime);
        if (age < maxAge)
            age++;
        generateCurrency = true;
    }
}