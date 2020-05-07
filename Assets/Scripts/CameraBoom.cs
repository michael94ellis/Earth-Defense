using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoom : MonoBehaviour, IOrbitable
{
    private bool _isOrbiting;
    private GameObject _objectToOrbit;
    private Vector3 _orbitAngle;
    private float _orbitSpeed;

   public bool IsOrbiting { get => _isOrbiting; set => _isOrbiting = value; }
   public GameObject ObjectToOrbit { get => _objectToOrbit; set => _objectToOrbit = value; }
   public Vector3 OrbitAngle { get => _orbitAngle; set => _orbitAngle = value; }
   public float OrbitSpeed { get => _orbitSpeed; set => _orbitSpeed = value; }
   

    private void Update()
    {
        Orbit();
    }

    public void CeaseOrbit()
    {
        IsOrbiting = false;
    }

    public void ResumeOrbit()
    {
        IsOrbiting = true;
    }
    public void Orbit()
    {
        if (IsOrbiting && ObjectToOrbit != null) 
            transform.RotateAround(ObjectToOrbit.transform.position, OrbitAngle, Time.deltaTime * OrbitSpeed);
    }

    public void SetOrbitObject(GameObject NewParent)
    {
        ObjectToOrbit = NewParent;
    }
    public void ClearOrbitObject()
    {
        ObjectToOrbit = null;
    }
    public void SetOrbitAngle(Vector3 NewAngle)
    {
        OrbitAngle = NewAngle;
    }
    public void SetOrbitSpeed(float NewSpeed)
    {
        OrbitSpeed = NewSpeed;
    }

    public void setOrbitSpeed(float NewSpeed)
    {
        throw new System.NotImplementedException();
    }
}
