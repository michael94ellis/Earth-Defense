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

public struct MenuText
{
    public GameObject gameObject;
    public Text title;
}

public class DetailMenu : MonoBehaviour
{
    List<MenuButton> Buttons = new List<MenuButton>();
    List<MenuText> Texts = new List<MenuText>();
    public GameObject DisplayItem;
    public Text Title;
    public Text Info;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;
    // Starting point for placing buttons
    int UIPlacementY = -250;

    public void Display(GameObject itemToDisplay)
    {
        UIPlacementY = -250;
        // Set title of button
        DisplayItem = itemToDisplay;
        Title.text = DisplayItem.GetComponent<MenuDisplayItem>().Title;
        ZoneBuilding zoneBuilding = itemToDisplay.GetComponent<ZoneBuilding>();
        EarthZone earthZone = itemToDisplay.GetComponent<EarthZone>();
        if (earthZone != null)
        {
            // + 1 for Capitol
            UpdateTextsCount(2);
            UpdateButtonsCount(earthZone.ZoneBuildings.Count + 1);
            SetZoneBuildingButtons(earthZone);
        }
        if (zoneBuilding != null)
        {
            //UpdateTextsCount(zoneBuilding.Stats.Count);
            //SetItemStatRows(zoneBuilding);
            // Handle the amount of upgrade buttons shown
            //UpdateButtonsCount(zoneBuilding.upgrades.Count);
            //// Show the upgrade buttons
            //SetUpgradeButtons(zoneBuilding);
            string info = "";
            foreach (BuildingStat stat in zoneBuilding.Stats)
            {
                info += stat.name + ": " + stat.value + " / " + stat.maxValue + "/n";
            }
            Info.text = info;
            TopTab.onClick.AddListener(() => { Display(zoneBuilding.ParentZone.gameObject); });
            TopTab.GetComponentInChildren<Text>().text = "Zone";
        }
    }

    void UpdateTextsCount(int countNeeded)
    {
        if (countNeeded > Texts.Count)
        {
            int missingTexts = countNeeded - Texts.Count;
            // Create as many buttons as needed
            for (int i = 0; i < missingTexts; i++)
            {
                CreateNewText();
            }
        }
        else if (countNeeded < Texts.Count)
        {
            // deactivate extra buttons
            for (int i = Texts.Count - 1; i >= countNeeded; i--)
            {
                Texts[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateButtonsCount(int countNeeded)
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
            // Doing a for(int i =0;..) loop is not easier than this 
            buttonIndex++;
        }
    }
    void SetItemStatRows(ZoneBuilding zoneBuilding)
    {
        int textIndex = 0;
        foreach (BuildingStat stat in zoneBuilding.Stats)
        {
            Texts[textIndex].gameObject.SetActive(true);
            // Replace the text of button
            Texts[textIndex].title.text = stat.name + stat.value;
            // Doing a for(int i =0;..) loop is not easier than this 
            textIndex++;
        }
    }

    void CreateNewButton()
    {
        GameObject newButton = Instantiate(Resources.Load("OptionButton") as GameObject);
        newButton.transform.position = new Vector3(0, UIPlacementY, 0);
        newButton.transform.SetParent(transform, false);
        UIPlacementY -= 130;
        MenuButton newMenuButton = new MenuButton();
        newMenuButton.gameObject = newButton;
        newMenuButton.button = newButton.GetComponent<Button>();
        newMenuButton.title = newButton.GetComponentInChildren<Text>();
        Buttons.Add(newMenuButton);
    }
    void CreateNewText()
    {
        GameObject NewText = new GameObject("MenuText");
        Text TextComponent = NewText.AddComponent<Text>();
        TextComponent.transform.SetParent(transform, false);
        TextComponent.transform.position = new Vector3(0, UIPlacementY, 0);
        UIPlacementY -= 60;
        MenuText newMenuText = new MenuText();
        newMenuText.gameObject = NewText;
        newMenuText.title = TextComponent;
        Texts.Add(newMenuText);
    }
}
