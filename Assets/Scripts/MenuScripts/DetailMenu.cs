using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MenuButton
{
    public GameObject gameObject;
    public Button button;
    public Text title;
}

public class DetailMenu : MonoBehaviour
{
    List<MenuButton> Buttons = new List<MenuButton>();
    public MenuDisplayItem DisplayItem;
    public Text Title;
    public Text Info;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;
    // Starting point for placing buttons
    int buttonPlacementY = -250;

    public void Display(GameObject itemToDisplay)
    {
        // Set title of button
        DisplayItem = itemToDisplay.GetComponent<MenuDisplayItem>();
        Title.text = DisplayItem.Title;
        Info.text = DisplayItem.InfoText;
        ZoneBuilding zoneBuilding = itemToDisplay.GetComponent<ZoneBuilding>();
        EarthZone earthZone = itemToDisplay.GetComponent<EarthZone>();
        if (earthZone != null)
        {
            // + 1 for Capitol
            UpdateButtonCount(earthZone.ZoneBuildings.Count + 1);
            SetZoneBuildingButtons(earthZone);
        }
        if (zoneBuilding != null)
        {
            // Handle the amount of upgrade buttons shown
            UpdateButtonCount(zoneBuilding.upgrades.Count);
            // Show the upgrade buttons
            SetUpgradeButtons(zoneBuilding);
            TopTab.onClick.AddListener(() => { Display(zoneBuilding.ParentZone.gameObject); });
            TopTab.GetComponentInChildren<Text>().text = "Zone";
        }
    }

    void UpdateButtonCount(int countNeeded)
    {
        if (countNeeded > Buttons.Count)
        {
            int missingButtons = countNeeded - Buttons.Count;
            // Create as many buttons as needed
            for (int i = 0; i < missingButtons; i++)
            {
                CreateNewButton();
            }
        }
        else if (countNeeded < Buttons.Count)
        {
            int extraButtons = Buttons.Count - countNeeded;
            // deactivate extra buttons
            for (int i = Buttons.Count - 1; i >= countNeeded; i--)
            {
                Buttons[i].gameObject.SetActive(false);
            }
        }
    }

    void SetZoneBuildingButtons(EarthZone zone)
    {
        int buttonIndex = 0;
        // Replace the text of button
        Buttons[buttonIndex].title.text = "Capitol";
        // Remove any previous actions
        Buttons[buttonIndex].button.onClick.RemoveAllListeners();
        // Pass in the code for performing the upgrade
        Buttons[buttonIndex].button.onClick.AddListener(() =>
        {
            Display(zone.Capitol.buildingTransform.gameObject);
        });
        // Doing a for(int i =0;..) loop is not easier than this 
        buttonIndex++;
        foreach (ZoneBuilding zoneBuilding in zone.ZoneBuildings)
        {
            Buttons[buttonIndex].gameObject.SetActive(true);
            // Replace the text of button
            Buttons[buttonIndex].title.text = zoneBuilding.buildingType.ToString();
            // Remove any previous actions
            Buttons[buttonIndex].button.onClick.RemoveAllListeners();
            // Pass in the code for performing the upgrade
            Buttons[buttonIndex].button.onClick.AddListener(() =>
            {
                Display(zoneBuilding.buildingTransform.gameObject);
            });
            // Doing a for(int i =0;..) loop is not easier than this 
            buttonIndex++;
        }
    }

    void SetUpgradeButtons(ZoneBuilding zoneBuilding)
    {
        int buttonIndex = 0;
        foreach (BuildingUpgrade upgrade in zoneBuilding.upgrades)
        {
            Buttons[buttonIndex].gameObject.SetActive(true);
            // Replace the text of button
            Buttons[buttonIndex].title.text = upgrade.name;
            // Remove any previous actions
            Buttons[buttonIndex].button.onClick.RemoveAllListeners();
            // Pass in the code for performing the upgrade
            Buttons[buttonIndex].button.onClick.AddListener(() =>
            {
                upgrade.performUpgrade();
            });
            // If the button is clicked the stats get updated here
            Buttons[buttonIndex].button.onClick.AddListener(() =>
            {
                Info.text = DisplayItem.InfoText;
            });
            // Doing a for(int i =0;..) loop is not easier than this 
            buttonIndex++;
        }
    }

    void CreateNewButton()
    {
        GameObject newButton = Instantiate(Resources.Load("OptionButton") as GameObject);
        newButton.transform.position = new Vector3(0, buttonPlacementY, 0);
        newButton.transform.SetParent(transform, false);
        buttonPlacementY -= 130;
        MenuButton newMenuButton = new MenuButton();
        newMenuButton.gameObject = newButton;
        newMenuButton.button = newButton.GetComponent<Button>();
        newMenuButton.title = newButton.GetComponentInChildren<Text>();
        Buttons.Add(newMenuButton);
    }
}
