using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public struct UIButtonInstance_Building
{
    public GameObject[] buildingPrefabs;
    public string[] buttonText;
    public Button[] buttons;

}

public class ARMBuildingOptionsCanvasBehaviour : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    UIButtonInstance_Building UI;
    VerticalLayoutGroup verticalLayoutGroup;


    public void MakeBuildingLabelsFrom(GameObject[] availableBuildings)
    {
        UI.buildingPrefabs = availableBuildings;

        if (UI.buildingPrefabs.Length <= 0)
            return;

        AddLayoutGroup();

        foreach (var item in UI.buildingPrefabs)
        {
            TextMeshPro text = new TextMeshPro { text = ""+item.name };
            Button button = gameObject.AddComponent<Button>();
            text.transform.SetParent(button.transform);
        }
    }

    void AddLayoutGroup()
    {
        verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();

    }

    public void Hide()
    {
        canvas.enabled = false;   
    }
    public void Appear()
    {
        canvas.enabled = true;
    }   

}
