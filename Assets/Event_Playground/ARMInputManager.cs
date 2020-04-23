using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMInputManager : MonoBehaviour
{


    // Exposed Private Fields
    [SerializeField] Camera mainCamera;

    // Hidden Private Fields
    private Vector2 MousePosition;



    // Exposed Events
    [SerializeField] private VoidEvent OnToggleBuildMode;
    [SerializeField] private Vector3Event OnNewLocationClicked;
    [SerializeField] private RaycastHitEvent OnNewRaycastHitEarth;
    [SerializeField] private VoidEvent OnEscapeButtonPressed;
    

    // Runtime
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].gameObject.GetComponent<Camera>();
    }
    private void Update()
    {
        MousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastFromCamera();
        }

       
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEscapeButtonPressed.Raise();
        }
    }
    
    // API
    public void RaycastFromCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.gameObject.name == "Big_Earth")
            {
                OnNewRaycastHitEarth.Raise(hit);
            }
                 
        }

        
    }
  
    
}
