using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingObject : MonoBehaviour, IOrbitable
{
    private bool _isOrbiting { get; set; }
    private GameObject _objectToOrbit { get; set; }
    private Vector3 _orbitAngle { get; set; }
    private float _orbitSpeed { get; set; }

    public bool IsOrbiting { get => _isOrbiting; set => _isOrbiting = value; }
    public GameObject ObjectToOrbit { get => _objectToOrbit; set => _objectToOrbit = value; }
    public Vector3 OrbitAngle { get => _orbitAngle; set => _orbitAngle = value; }
    public float OrbitSpeed { get => _orbitSpeed; set => _orbitSpeed = value; }

    public void CeaseOrbit()
    {
        IsOrbiting = false;
    }

    public void ClearOrbitObject()
    {
        _objectToOrbit = null;
    }

    public void Orbit()
    {
        if (IsOrbiting && ObjectToOrbit != null)
            transform.RotateAround(ObjectToOrbit.transform.position, _orbitAngle, Time.deltaTime * _orbitSpeed);
    }

    public void ResumeOrbit()
    {
        _isOrbiting = true;
    }

    public void SetOrbitAngle(Vector3 NewAngle)
    {
        _orbitAngle = NewAngle;
    }

    public void SetOrbitObject(GameObject NewParent)
    {
        _objectToOrbit = NewParent;
    }

    public void SetOrbitSpeed(float NewSpeed)
    {
        _orbitSpeed = NewSpeed;
    }
}