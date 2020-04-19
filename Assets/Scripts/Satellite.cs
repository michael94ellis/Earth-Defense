using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 80.0f;
    Vector3 origin = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(origin, axis, rotationSpeed * Time.deltaTime);
    }
}
