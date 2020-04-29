using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;
    private float launchSpeed = 1;

    Vector3 origin;

    void Start()
    {
        origin = transform.position;
        transform.up = transform.position * 2;
    }

    void Update()
    {
        Debug.Log(Vector3.Distance(target.transform.position, transform.position));
        if (target != null)
        {
            if (Vector3.Distance(origin, transform.position) < 15)
            {
                launchSpeed += Time.deltaTime * 20;
                transform.position = Vector3.MoveTowards(transform.position, transform.position * 10, launchSpeed * Time.deltaTime);
            }
            else if (Vector3.Distance(target.transform.position, transform.position) > 25)
            {
                transform.position = Vector3.Slerp(transform.position, target.transform.position, 2 * Time.deltaTime);
                transform.up = target.transform.position;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 100f * Time.deltaTime);
                transform.up = target.transform.position;
            }
        }
        else
        {
            Debug.Log("No Target");
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
