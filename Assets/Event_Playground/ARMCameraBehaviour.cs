using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraInput
{
    public float vertical;
    public float horizontal;
}
public class OrbitingObject
{
    public GameObject Parent;
    
}

public class ARMCameraBehaviour : MonoBehaviour
{
    private CameraInput _cameraInput;
    public CameraInput cameraInput {
        get {
            return _cameraInput;
        }
        private set {

            _cameraInput = value;
        } }

    
}
