using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneMenu : MonoBehaviour
{
    public GameObject OptionButton;
    public MenuManager mainMenuManager;
    public GameObject ScrollView;
    public Text Title;
    public List<Button> ZoneBuildingButtons = new List<Button>();
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    int yPlacement = 0;

    public void Display()
    {
        mainMenuManager.CurrentlyDisplayedMenu = MenuManager.MenuScreen.Zone;
        if (mainMenuManager.DisplayItem != null)
        {
            //TopTab.onClick.AddListener(() => { Display(zoneBuilding.ParentZone.gameObject); });
            TopTab.GetComponentInChildren<Text>().text = "Zone";
        }
        Title.text = mainMenuManager.DisplayItem.buildingType.ToString();
        int i = 0;
        foreach (ZoneBuilding building in mainMenuManager.DisplayItem.ParentZone.ZoneBuildings)
        {
            if (ZoneBuildingButtons.Count <= i)
            {
                GameObject newButton = Instantiate(OptionButton) as GameObject;
                newButton.transform.SetParent(ScrollView.transform, false);
                ZoneBuildingButtons.Add(newButton.GetComponent<Button>());
                newButton.transform.position = new Vector2(0, yPlacement);
            }
            ZoneBuildingButtons[i].gameObject.SetActive(true);
            ZoneBuildingButtons[i].GetComponentInChildren<Text>().text = building.buildingType.ToString();
            ZoneBuildingButtons[i].onClick.RemoveAllListeners();
            ZoneBuildingButtons[i].onClick.AddListener(() =>
            {
                mainMenuManager.DisplayItem = building;
                mainMenuManager.OpenDetailMenu();
            });
            i++;
        }
        for (; i > mainMenuManager.DisplayItem.ParentZone.ZoneBuildings.Count; i++)
        {
            ZoneBuildingButtons[i].gameObject.SetActive(false);
        }
    }
}
