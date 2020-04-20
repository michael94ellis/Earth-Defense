using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ARMEarthBehaviour : MonoBehaviour
{

    private bool isRotating = false;
    private Rigidbody rigidBody;
    private float radius = 0;



    [SerializeField] private VoidEvent OnObjectSelected;


    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        radius = transform.localScale.x / 4f;
    }

    public void TogglePause()
    {
        if (!isRotating)
        {
            isRotating = !isRotating;
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, ((360.985f / 100) * Time.deltaTime) * -1);
        }
    }

    private void OnMouseEnter()
    {
        OnObjectSelected.Raise();
    }
    private void OnMouseExit()
    {
        OnObjectSelected.Raise();
    }

   


}
