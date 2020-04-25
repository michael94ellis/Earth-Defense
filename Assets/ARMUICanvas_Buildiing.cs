using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[RequireComponent(typeof(TextMeshPro))]
public class ARMUICanvas_Buildiing : MonoBehaviour, IUICanvas
{

    [SerializeField] private UIStateEvent OnUIStateChanged;

    private GameObject[] currentBuildingPrefabs;


    [SerializeField] private GameObject _buildingOptionButtonPrefab;

    private List<GameObject> CurrentButtonsOnScreen;

    private void Awake()
    {
        CurrentButtonsOnScreen = new List<GameObject>();
    }

    public void HideSelf()
    {
        gameObject.SetActive(false);
    }

    public void ShowSelf()
    {
        gameObject.SetActive(true);
    }

    public void ShowBuildingOptions()
    {
        OnUIStateChanged.Raise(UIState.BuildingBuilding);
    }

    public void AcceptNewBuildings(GameObject[] buildingPrefabs)
    {
        currentBuildingPrefabs = buildingPrefabs;
        if (currentBuildingPrefabs.Length < 1)
            return;
        ClearCurrentButtonsOnScreen();
        RenderBuildingOptionsOnScreen();
    }

    private void ClearCurrentButtonsOnScreen()
    {
        if (CurrentButtonsOnScreen.Count > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                Destroy(child);
            }
            CurrentButtonsOnScreen.Clear();
        }
    }
    private void RenderBuildingOptionsOnScreen()
    {
        
        CurrentButtonsOnScreen = new List<GameObject>();
        
        for (int i = 0; i < currentBuildingPrefabs.Length; i++)
        {
            GameObject newButtonInstance = Instantiate(_buildingOptionButtonPrefab, this.transform);
            var name = currentBuildingPrefabs[i].gameObject.name;
            var refinedName = name.Remove(0, 3);
            newButtonInstance.GetComponentInChildren<UnityEngine.UI.Button>().GetComponentInChildren<TextMeshProUGUI>().text = refinedName;
            BuildingIndex buildingIndex = new BuildingIndex { value = i };
            var buildBehaviour = newButtonInstance.GetComponent<BuildButtonBehaviour>();
            buildBehaviour.SetBuildingIndex(buildingIndex);
            CurrentButtonsOnScreen.Add(newButtonInstance);       
        }
    }
}
