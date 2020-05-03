using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour, ZoneBuilding
{
    public bool isActive { get; set; } = false;
    public EarthZone ParentZone { get; set; }
    public int rechargeTime = 5;
    public bool shieldIsCharged = true;
    public float shieldRegenRate = 1f;
    public float ShieldBoost = 3000;

    void Update()
    {
        if (ParentZone != null)
        {
            if (ParentZone.Shield.enabled)
            {
                ParentZone.ShieldHealth += shieldRegenRate * Time.deltaTime;
            }
            else if (shieldIsCharged)
            {
                ParentZone.Shield.enabled = true;
            }
        }
    }
    public void ResetShield()
    {
        ParentZone.Shield.enabled = true;
        ParentZone.ShieldHealth += ShieldBoost;
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
        shieldIsCharged = true;
        ParentZone.ShieldHealth += ShieldBoost;
    }
}
