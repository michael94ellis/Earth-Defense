using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMPlayer
{
    public bool instantiateCityMode = false;

    void ToggleInstateiateCityMode()
    {
        instantiateCityMode = !instantiateCityMode;
    }
}

public class ARMPlayerManager : MonoBehaviour
{
    ARMPlayer Player = new ARMPlayer();

    [SerializeField] GameObject[] _buildingPrefabs;


    private int selectedBuildingBlueprintIndex = 0;




    public void ToggleBuildCityMode()
    {

        Player.instantiateCityMode = !Player.instantiateCityMode;
        if (Player.instantiateCityMode)
        {
            print("Player May Build");
        }
        else
        {
            print("Player May not Build");
        }
    }

    public void InstatiateBuildingFromRaycastHit(RaycastHit raycastHit)
    {
        if (Player.instantiateCityMode)
        {
           
            GameObject buildingInstance = Instantiate(_buildingPrefabs[selectedBuildingBlueprintIndex],raycastHit.collider.gameObject.transform);
        
            // Find the line from the gun to the point that was clicked.
            Vector3 incomingVec = raycastHit.point - Camera.main.transform.position;

            // Use the point's normal to calculate the reflection vector.
            Vector3 reflectVec = Vector3.Reflect(incomingVec, raycastHit.normal);

            buildingInstance.transform.position = raycastHit.point;
            buildingInstance.transform.localRotation = Quaternion.FromToRotation(Vector3.zero, incomingVec);
            buildingInstance.transform.localScale /= 100;


            Debug.DrawLine(Camera.main.transform.position, raycastHit.point, Color.red);
            Debug.DrawRay(raycastHit.point, reflectVec, Color.green);

            print("Building Done");
        }
    }

    private Transform GetTransformFromHitObject(RaycastHit raycastHit)
    {
        return gameObject.transform;
    }
}
