using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    public float rotationSpeed = 80.0f;

    Object DestructionEffect;
    public AudioSource ExplosionSound;

    Vector3 currentOrbit;
    Vector3 currentOrbit2;
    Vector3[] orbits = new Vector3[] { Vector3.left, Vector3.right, Vector3.back, Vector3.forward, Vector3.up, Vector3.down };

    // Start is called before the first frame update
    void Start()
    {
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
        AudioSource[] soundSources = gameObject.GetComponents<AudioSource>();
        ExplosionSound = soundSources[0];
        currentOrbit = orbits[Random.Range(0, orbits.Length - 1)];
        currentOrbit2 = orbits[Random.Range(0, orbits.Length - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        // Orbit the earth
        transform.RotateAround(Vector3.zero, currentOrbit, 50f * Time.deltaTime);
        transform.RotateAround(Vector3.zero, currentOrbit2, 50f * Time.deltaTime);
    }
}
