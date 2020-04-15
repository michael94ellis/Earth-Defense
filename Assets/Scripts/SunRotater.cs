using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotater : MonoBehaviour
{
    public float minRotation = 20f;
    public float maxRotation = 160f;
    public float XRotationIncrement = 1f;
    public float YRotationIncrement = 1f;
    // Min Y is 0
    GameObject Earth;

    // Start is called before the first frame update
    void Start()
    {
        Earth = GameObject.FindGameObjectWithTag("Earth");
        transform.eulerAngles = new Vector3(20, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // The x and y axis wobble between the min and max values
        if (transform.rotation.x <= minRotation || transform.rotation.x >= maxRotation)
        {
            XRotationIncrement *= -1;
        }
        transform.Rotate(XRotationIncrement * Time.deltaTime, YRotationIncrement * Time.deltaTime, 0);
    }
}
