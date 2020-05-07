using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IOrbitable
{
    bool IsOrbiting { get; set; }
    GameObject ObjectToOrbit { get; set; }
    Vector3 OrbitAngle { get; set; }
    float OrbitSpeed { get; set; }

    // Update is called once per frame
    void ResumeOrbit();
    void CeaseOrbit();
    void Orbit();
    void SetOrbitObject(GameObject NewParent);
    void ClearOrbitObject();
    void SetOrbitAngle(Vector3 NewAngle);
    void SetOrbitSpeed(float NewSpeed);

}

