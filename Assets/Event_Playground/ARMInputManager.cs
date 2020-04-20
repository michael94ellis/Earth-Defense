using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMInputManager : MonoBehaviour
{


    // Exposed Private Fields
    [SerializeField] Camera mainCamera;

    // Hidden Private Fields
    private Vector2 MousePosition;

    // Public / Private Set Fields
    private RaycastHit _lastRaycastHit;
    public RaycastHit lastRaycastHit
    {
        get
        {
            if (_lastRaycastHit.point == null)
                RaycastFromCamera();
                
            OnNewLocationClicked.Raise(_lastRaycastHit.point);
            OnNewClickedLocationRaycastHit.Raise(_lastRaycastHit);
            
            return _lastRaycastHit;
        }
        set
        {
            _lastRaycastHit = value;
         
        }
    }

    // Exposed Events
    [SerializeField] private VoidEvent OnToggleBuildMode;
    [SerializeField] private Vector3Event OnNewLocationClicked;
    [SerializeField] private RaycastHitEvent OnNewClickedLocationRaycastHit;



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

        if (Input.GetKeyUp(KeyCode.B))
        {
            OnToggleBuildMode.Raise();
        }
    }
    
    // API
    public void RaycastFromCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            OnNewClickedLocationRaycastHit.Raise(hit);     
        }

        
    }
  
    
}
