using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public EarthZone parentZone;
    public bool shieldIsUp = true;
    public int rechargeTime = 5;
    public float ShieldBoost = 3000;


    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        parentZone.AddShieldHealth(ShieldBoost);
    }
}
