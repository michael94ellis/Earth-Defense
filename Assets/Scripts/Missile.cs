using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;
    public float launchSpeed = 1;
    public float launchAcceleration = 10;
    public float moveSpeed = 10;

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
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        AlienShip alienScript = other.gameObject.GetComponent<AlienShip>();
        if (alienScript != null)
        {
            Debug.Log("Boom");
            alienScript.TakeDamage(200);
            Destroy(gameObject);
        }
    }
}
