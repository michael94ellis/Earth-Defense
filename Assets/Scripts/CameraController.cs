using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    float mainSpeed = 0.5f; //regular speed
    float zoomSpeed = 0.2f;
    float boundary = 800.0f;

    void Start()
    {
        this.transform.LookAt(Vector3.zero);
    }

    void Update()
    {
        // MARK: - Keyboard Controls

        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, mainSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(Vector3.zero, Vector3.down, mainSpeed);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.RotateAround(Vector3.zero, Vector3.right, mainSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.RotateAround(Vector3.zero, Vector3.left, mainSpeed);
        }
        if (Input.GetKey(KeyCode.E) && transform.position.y < boundary)
        {
            transform.Translate(Vector3.forward * zoomSpeed);
            if (zoomSpeed >= 0.001f)
                zoomSpeed = 0.01f * Vector3.Distance(Vector3.zero, transform.position);
        }
        if (Input.GetKey(KeyCode.Q) && transform.position.y > -boundary)
        {
            zoomSpeed = 0.1f;
            transform.Translate(Vector3.back * mainSpeed);
        }
        this.transform.LookAt(Vector3.zero);
    }
}