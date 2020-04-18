using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    float mainSpeed = 0.5f; //regular speed
    float boundary = 800.0f;

    public Vector3 origin = new Vector3(0, 0, 0);
    void Start()
    {
        this.transform.LookAt(origin);
    }
    void Update()
    {
        this.transform.LookAt(origin);

        if (Input.GetKey(KeyCode.A) && transform.position.x > -boundary)
        {
            transform.Translate(Vector3.left * mainSpeed);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < boundary)
        {
            transform.Translate(Vector3.right * mainSpeed);
        }
        if (Input.GetKey(KeyCode.W) && transform.position.z < boundary)
        {
            transform.Translate(Vector3.up * mainSpeed);
        }
        if (Input.GetKey(KeyCode.S) && transform.position.z > -boundary)
        {
            transform.Translate(Vector3.down * mainSpeed);
        }
        if (Input.GetKey(KeyCode.E) && transform.position.y < boundary)
        {
            transform.Translate(Vector3.forward * mainSpeed);
            if (mainSpeed >= 0.001f)
                mainSpeed = 0.01f * Vector3.Distance(origin, transform.position);
        }
        if (Input.GetKey(KeyCode.Q) && transform.position.y > -boundary)
        {
            transform.Translate(Vector3.back * mainSpeed);
            if (mainSpeed <= 0.3f)
                mainSpeed = 1.01f * Vector3.Distance(origin, transform.position);
        }
    }
}