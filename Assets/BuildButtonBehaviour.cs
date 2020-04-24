using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildButtonBehaviour : MonoBehaviour
{
    public TextMeshPro textMesh { get; private set; }
    public BuildingIndex buildingIndex { get; private set; }
   
    [SerializeField] private IntEvent OnSendButtonIndex;


    public void SetText(string title) => textMesh.text = title;
    public void SetBuildingIndex(BuildingIndex buildingIndex)
    {
        this.buildingIndex = buildingIndex;
    }
    public void OnButtonTapped()
    {
        OnSendButtonIndex.Raise(this.buildingIndex.value); 
    }
    
}
