using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour, Damageable
{
    public float rotationSpeed = 80.0f;

    Object DestructionEffect;
    public AudioSource ExplosionSound;

    public int Health { get; private set; }
    public void TakeDamage()
    {
        //Debug.Log("Damage");
        Health--;
        if (Health == 0)
        {
            ExplosionSound.Play();
            GameObject DestructionAnimation = Instantiate(DestructionEffect, transform.position, transform.rotation) as GameObject;
            DestructionAnimation.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            Destroy(gameObject);
            Earth.Children.Remove(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 120;
        int explosionNumber = Random.Range(1, 10);
        DestructionEffect = Resources.Load("Explosion" + explosionNumber);
        AudioSource[] soundSources = gameObject.GetComponents<AudioSource>();
        ExplosionSound = soundSources[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
