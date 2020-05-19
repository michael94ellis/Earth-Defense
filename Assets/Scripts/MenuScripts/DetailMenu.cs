using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    public MenuManager mainMenuManager;
    public Text Title;
    public Text[] DetailLabels;
    public Text[] DetailValues;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    public void Display()
    {
        mainMenuManager.CurrentlyDisplayedMenu = MenuManager.MenuScreen.Detail;
        if (mainMenuManager.DisplayItem != null)
        {
            TopTab.onClick.AddListener(() =>
            {
                mainMenuManager.ZoneMenu.Display();
            });
            TopTab.GetComponentInChildren<Text>().text = "Zone";
        }
        Title.text = mainMenuManager.DisplayItem.buildingType.ToString();
        int i = 0;
        foreach (BuildingStat stat in mainMenuManager.DisplayItem.Stats)
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
}