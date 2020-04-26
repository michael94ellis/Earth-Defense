using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Vector3 orbitAngle = new Vector3(0, 1, 0.001f);

    GameObject sun;

    void Start()
    {
        sun = GameObject.Find("Sun");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(sun.transform.position, orbitAngle, Time.deltaTime);
    }
}
