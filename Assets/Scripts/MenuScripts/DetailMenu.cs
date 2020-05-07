using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    GameObject MenuButton;
    public DisplayItemType _DisplayItemType;
    public IMenuDisplayable DisplayItem;
    public Text Title;
    public Text Info;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    public void Display(DisplayItemType itemType, GameObject itemToDisplay)
    {
        // Cache the item reference
        _DisplayItemType = itemType;
        switch (itemType)
        {
            case DisplayItemType.Alien:
                break;
            case DisplayItemType.Earth:
                break;
            case DisplayItemType.EarthZone:
                break;
            case DisplayItemType.ZoneBuilding:
                TopTab.GetComponentInChildren<Text>().text = "Zone";
                break;
        }
        // Set title of button
        DisplayItem = itemToDisplay.GetComponent<IMenuDisplayable>();
        Title.text = DisplayItem.Title;
        Info.text = DisplayItem.InfoText;
        IZoneable zoneBuilding = itemToDisplay.GetComponent<IZoneable>();
        if (zoneBuilding != null)
        {
            int y = -250;
            foreach (BuildingUpgrade upgrade in zoneBuilding.upgrades)
            {
                CreateNewButton(new Vector3(0, y, 0));
                MenuButton.GetComponentInChildren<Text>().text = upgrade.name;
                MenuButton.GetComponent<Button>().onClick.AddListener(() => upgrade.performUpgrade());
                MenuButton.GetComponent<Button>().onClick.AddListener(() => {
                    Info.text = DisplayItem.InfoText;
                } );
            }
        }
    }

    //void ViewParent()
    //{
    //}

    private void CreateNewButton(Vector3 position)
    {
        if (MenuButton == null)
        {
            MenuButton = Instantiate(Resources.Load("OptionButton") as GameObject, position, Quaternion.identity);
            MenuButton.transform.SetParent(transform, false);
        }

    }
}
