using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour, ZoneBuilding
{
    private EarthZone _ParentZone;
    public EarthZone ParentZone { get { return _ParentZone; } set { _ParentZone = value; } }
    public int rechargeTime = 5;
    public float shieldRegenRate = 1f;
    public float ShieldBoost = 3000;

    void Start()
    {
        ResetShield();
    }
    void Update()
    {
        if (ParentZone != null && ParentZone.Shield.enabled)
            ParentZone.ShieldHealth += shieldRegenRate * Time.deltaTime;
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
        ParentZone.Shield.enabled = true;
        ParentZone.ShieldHealth += ShieldBoost;
    }
}
