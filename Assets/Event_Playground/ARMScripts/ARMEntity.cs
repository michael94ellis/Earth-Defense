using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ARMIConstructable<T> where T: ARMEntity 
{
    float armor { get; set; }
    float health { get; set; }

    bool Equals(T obj);
    void Erect(T obj);


    void Withstand();
    void TakeDamage();
    void Succomb();
}

public class ARMEntity : MonoBehaviour
{

    [SerializeField] private VoidEvent OnTakeDamage;
    [SerializeField] private VoidEvent OnDespawn;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
