using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    GameObject MenuButton;
    public ZoneBuilding DisplayItem;
    public Text Title;
    public Text Info;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    public void Open(ZoneBuilding item)
    {
        // Cache the item reference
        DisplayItem = item;
        // Set title of button
        Title.text = item.buildingType.ToString();
        Info.text = item.InfoText;

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

    private void CreateNewButton(Vector3 position)
    {
        MenuButton = Instantiate(Resources.Load("OptionButton") as GameObject, position, Quaternion.identity);
        MenuButton.transform.SetParent(transform, false);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}
