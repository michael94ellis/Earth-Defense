using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour, ICameraFacing
{

    [SerializeField] private Camera _MainCamera;

    public void FaceCamera()
    {
        if (_MainCamera == null)
            _MainCamera = Camera.main;


        transform.LookAt(_MainCamera.transform.position);
    }

    void Update()
    {
        FaceCamera();
    }
}
