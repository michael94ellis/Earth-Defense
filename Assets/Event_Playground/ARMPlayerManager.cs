using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ARMPlayerManager : MonoBehaviour
{
   


    [SerializeField] GameObject[] _buildingPrefabs;


    private int selectedBuildingBlueprintIndex = 0;


    public GameObject Target { get; private set; }
    public GameObject Parent { get; private set; }
    public bool readyToSetNewParent { get; private set; }

    public bool InstantiateCityMode { get; private set; }

    [SerializeField] private GameObjectArrayEvent OnSendBuildingsToUI;



    public void SetTarget(GameObject newTarget)
    {
        Target = newTarget;
    }
    public void SetParent(GameObject newParent)
    {
        Parent = newParent;
        transform.SetParent(Parent.transform);
    }
    public void PrepareToSetNewParent()
    {
        readyToSetNewParent = true;
    }
    public void FinishedSettingNewParent()
    {
        readyToSetNewParent = false;
    }


    [SerializeField] private UIStateEvent OnUIStateChanged;

    public void BuildButtonTapped()
    {
        OnSendBuildingsToUI.Raise(_buildingPrefabs);
    }
    public void SetBuildingBluePrintIndex(int index)
    {
        selectedBuildingBlueprintIndex = index;
    }
    public void ToggleBuildCityMode()
    {

        InstantiateCityMode = !InstantiateCityMode;
        if (InstantiateCityMode)
        {
            print("Player May Build");
        }
        else
        {
            print("Player May not Build");
        }
    }

    public void SendBuildingsToUIViaEventListener()
    {
        OnSendBuildingsToUI.Raise(_buildingPrefabs);
    }
   
    public void InstatiateBuildingFromRaycastHit(RaycastHit raycastHit)
    {
        if (InstantiateCityMode)
        {
           
            GameObject buildingInstance = Instantiate(_buildingPrefabs[selectedBuildingBlueprintIndex],raycastHit.collider.gameObject.transform);
            buildingInstance.name = _buildingPrefabs[selectedBuildingBlueprintIndex].name;
        
            // Find the line from the gun to the point that was clicked.
            Vector3 incomingVec = raycastHit.point - Camera.main.transform.position;

            // Use the point's normal to calculate the reflection vector.
            Vector3 reflectVec = Vector3.Reflect(incomingVec, raycastHit.normal);

            buildingInstance.transform.position = raycastHit.point;
            buildingInstance.transform.localRotation = Quaternion.FromToRotation(Vector3.zero, incomingVec);
            buildingInstance.transform.localScale /= 100;


            Debug.DrawLine(Camera.main.transform.position, raycastHit.point, Color.red);
            Debug.DrawRay(raycastHit.point, reflectVec, Color.green);

            print("Building Done: " + buildingInstance.name + "built at " + buildingInstance.transform.position + ". Belongs to " + buildingInstance.transform.parent.gameObject.name);
        }
    }

    private Transform GetTransformFromHitObject(RaycastHit raycastHit)
    {
        return gameObject.transform;
    }
}
