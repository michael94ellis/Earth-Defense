using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Missile : MonoBehaviour, MissileSpec
{
    public GameObject target;
    public float launchSpeed { get; set; } = 1;
    public float launchAcceleration = 10;
    public float moveSpeed { get; set; } = 10;
    public float damage { get; set; } = 200;

    private Vector3 origin;
    void Start()
    {
        origin = transform.position;
        transform.up = transform.position * 2;
    }

    void Update()
    {
        //Debug.Log(Vector3.Distance(target.transform.position, transform.position));
        if (target != null && target.activeInHierarchy)
        {
            transform.position = Vector3.Slerp(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            transform.up = target.transform.position;
        }
        else
        {
            // TODO: Replace with Object Pooling
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        AlienShip alienScript = other.gameObject.GetComponent<AlienShip>();
        if (alienScript != null)
        {
            Debug.Log("Boom");
            alienScript.TakeDamage((int)damage);
            // TODO: Replace with Object Pooling
            Destroy(gameObject);
        }
    }
}
