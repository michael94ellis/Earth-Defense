using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Vector3 orbitAngle = new Vector3(0, 1, 0.001f);

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, orbitAngle, Time.deltaTime);
    }
}
