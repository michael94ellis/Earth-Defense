using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : OrbitingObject
{

    private void Update()
    {
        var earth = FindObjectOfType<Earth>() as Earth;
        SetOrbitObject(earth.gameObject);
        SetOrbitAngle(Vector3.up);
        SetOrbitSpeed(10);
        ResumeOrbit();
        Orbit();
    }
}
