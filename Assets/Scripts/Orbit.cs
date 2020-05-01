using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Vector3 orbitAngle = new Vector3(0, 1, 0.001f);
    public GameObject center;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.transform.position, orbitAngle, Time.deltaTime);
    }
}
