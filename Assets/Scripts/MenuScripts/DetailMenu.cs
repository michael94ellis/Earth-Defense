using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    public GameObject DisplayItem;
    public Text Title;
    public Text[] DetailLabels;
    public Text[] DetailValues;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    public void Display(GameObject itemToDisplay)
    {
        DisplayItem = itemToDisplay;
        ZoneBuilding zoneBuilding = itemToDisplay.GetComponent<ZoneBuilding>();
        if (zoneBuilding != null)
        {
            TopTab.onClick.AddListener(() => { Display(zoneBuilding.ParentZone.gameObject); });
            TopTab.GetComponentInChildren<Text>().text = "Zone";
        }
        Title.text = DisplayItem.name;
        int i = 0;
        foreach (BuildingStat stat in zoneBuilding.Stats)
        {
            DetailLabels[i].text = stat.name;
            DetailValues[i].text = stat.value.ToString();
            i++;
        }
        for (; i < 4; i++)
        {
            DetailLabels[i].gameObject.SetActive(false);
            DetailValues[i].gameObject.SetActive(false);
        }
    }

    public void OpenUpgradeMenu()
    {

    }
}