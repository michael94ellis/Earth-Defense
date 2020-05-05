using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    GameObject MenuButton;
    public ZoneBuilding DisplayItem;

    public void Open(ZoneBuilding item)
    {
        // Cache the item reference
        DisplayItem = item;
        if (MenuButton == null)
        {
            // Create button at 0,0,0 if it hasnt been created yet, this way we can create a new set of buttons each time we open a new object
            // We should create a new set of buttons, but we can replace some text or pool the ui elements or something to save memory
            CreateNewButton();
        }
        // Set title of button
        MenuButton.GetComponentInChildren<Text>().text = item.buildingType.ToString();

        //
        // We could create different buttons that invoke different funcs on the ZoneBuilding object here, like upgrades and GetStat methods/vars
        //
        // e.g.
        // ZoneBuilding.OpenParentMenu

        // Zonebuilding.GetTitle
        // Zonebuilding.GetStats

        // Zonebuilding.UpgradeStat1
        // Zonebuilding.UpgradeStat2
        // Zonebuilding.UpgradeStat3
        //
    }

    private void CreateNewButton()
    {
        MenuButton = Instantiate(Resources.Load("OptionButton") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        MenuButton.transform.SetParent(transform, false);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}
