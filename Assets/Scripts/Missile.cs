using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        if (target != null)
        {
            transform.up = target.transform.position - transform.position;
            Vector3.MoveTowards(transform.position, target.transform.position, 100 * Time.deltaTime);
        }
    }
}
