using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Text Title;
    public Text[] OptionTitles;
    public Button[] Options;

    List<MenuButton> Buttons = new List<MenuButton>();
    List<MenuText> Texts = new List<MenuText>();
    public MenuDisplayItem DisplayItem { get; set; }

    public void Display(GameObject itemToDisplay)
    {
        // Set title of button
        DisplayItem = itemToDisplay.GetComponent<MenuDisplayItem>();
        Title.text = DisplayItem.Title;
        ZoneBuilding zoneBuilding = itemToDisplay.GetComponent<ZoneBuilding>();
        EarthZone earthZone = itemToDisplay.GetComponent<EarthZone>();
        if (earthZone != null)
        {
        }
        if (zoneBuilding != null)
        { // TODO FIXME DONT USE FOR i loops, use foreach, the for i loop fucks up because the index variable isnt captured inside the loop so it calles out of bounds on the array it iterates over its so dumb
            for (int i = 0; i < 3; i++)
            {
                if (zoneBuilding.upgrades.Count > i && zoneBuilding.upgrades[i] != null)
                {
                    OptionTitles[i].enabled = true;
                    Options[i].enabled = true;
                    Options[i].onClick.RemoveAllListeners();
                    OptionTitles[i].text = zoneBuilding.upgrades[i].name;
                    Options[i].onClick.AddListener(() => zoneBuilding.upgrades[i].performUpgrade());
                }
                else
                {
                    Options[i].onClick.RemoveAllListeners();
                    OptionTitles[i].enabled = false;
                    Options[i].enabled = false;
                }
            }
        }
    }
}
