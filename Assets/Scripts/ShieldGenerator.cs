using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public EarthZone parentZone;
    public int rechargeTime = 5;
    public float ShieldBoost = 3000;

    void Start()
    {
        parentZone.Shield.enabled = true;
        parentZone.AddShieldHealth(ShieldBoost);
    }

    void Update()
    {
        if (parentZone.Shield.enabled)
            parentZone.AddShieldHealth(Time.deltaTime);
    }
    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        Debug.Log("Restore shield");
        parentZone.Shield.enabled = true;
        parentZone.AddShieldHealth(ShieldBoost);
    }
}
