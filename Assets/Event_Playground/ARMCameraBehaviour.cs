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

[RequireComponent(typeof(CharacterController))]
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

    private CharacterController CharacterController;
    private void Awake()
    {
        CharacterController = gameObject.GetComponent<CharacterController>();
        
    }
    private OrbitingObject OrbitingObject;
    private OrbitingObject cachesOrbitingObject;

    [SerializeField] private GameObject _earth;

    public void ToggleEarthParent()
    {
        if (transform.parent == _earth) {
            transform.SetParent(_earth.transform);
        }
        else
        {
            transform.SetParent(transform);
        };

    }
}
