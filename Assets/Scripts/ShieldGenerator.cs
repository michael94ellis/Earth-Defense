using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public EarthZone parentZone;
    public int rechargeTime = 5;
    public float shieldRegenRate = 1f;
    public float ShieldBoost = 3000;

    void Update()
    {
        if (parentZone != null && parentZone.Shield.enabled)
            parentZone.ShieldHealth += shieldRegenRate * Time.deltaTime;
    }
    public void ResetShield()
    {
        parentZone.Shield.enabled = true;
        parentZone.ShieldHealth += ShieldBoost;
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
    /// Must be called like so: StartCoroutine(Recharge());
    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        Debug.Log("Restore shield");
        parentZone.Shield.enabled = true;
        parentZone.ShieldHealth += ShieldBoost;
    }
}
