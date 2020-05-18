using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public MenuManager mainMenuManager;
    public Text Title;
    public Text[] UpgradeLabels;
    public Button[] UpgradeButtons;
    public Text[] StatValues;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    public void Display()
    {
        if (mainMenuManager.DisplayItem != null)
        {
            //TopTab.onClick.AddListener(() => { Display(zoneBuilding.ParentZone.gameObject); });
            TopTab.GetComponentInChildren<Text>().text = "Zone";
        }
        Title.text = mainMenuManager.DisplayItem.buildingType.ToString();
        int i = 0;
        foreach (BuildingUpgrade upgrade in mainMenuManager.DisplayItem.upgrades)
        {
            UpgradeLabels[i].text = upgrade.name;
            UpgradeButtons[i].onClick.RemoveAllListeners();
            UpgradeButtons[i].onClick.AddListener(() => { upgrade.performUpgrade(); });
            i++;
        }
        for (; i < 3; i++)
        {
            UpgradeLabels[i].gameObject.SetActive(false);
            UpgradeButtons[i].gameObject.SetActive(false);
            StatValues[i].gameObject.SetActive(false);
        }
    }
}
