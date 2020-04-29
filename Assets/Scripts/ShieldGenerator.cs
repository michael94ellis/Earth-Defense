using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public EarthZone parentZone;
    public int rechargeTime = 5;
    public float shieldRegenRate = 1f;
    public float ShieldBoost = 3000;

    void Start()
    {
        parentZone.Shield.enabled = true;
        parentZone.AddShieldHealth(ShieldBoost);
    }

    void Update()
    {
        if (parentZone.Shield.enabled)
            parentZone.AddShieldHealth(shieldRegenRate * Time.deltaTime);
    }
    public void DoubleShieldRegenRate()
    {
        shieldRegenRate *= 2;
    }
    public void BoostStrength()
    {
        ShieldBoost += 1000;
    }
    public void ReduceRechargeTime()
    {
        if (rechargeTime > 1)
            rechargeTime -= 1;
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
