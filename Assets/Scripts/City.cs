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

    void Start()
    {
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