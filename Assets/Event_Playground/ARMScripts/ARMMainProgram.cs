using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMMainProgram : MonoBehaviour
{


    [SerializeField] private VoidEvent OnGameStart;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        OnGameStart.Raise();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    
}
