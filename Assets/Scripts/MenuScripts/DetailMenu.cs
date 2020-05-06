using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DisplayItemType
{
    Alien,
    Earth,
    EarthZone,
    ZoneBuilding
}

public interface MenuDisplayItem
{
    string Title { get; }
    string InfoText { get; }
}

public class DetailMenu : MonoBehaviour
{
    GameObject MenuButton;
    public DisplayItemType _DisplayItemType;
    public MenuDisplayItem DisplayItem;
    public Text Title;
    public Text Info;
    public Button TopTab;
    public Button MiddleTab;
    public Button BottomTab;

    void Start()
    {
        TopTab.onClick.AddListener(() => ViewParent());

    }

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
                break;
        }
        // Set title of button
        DisplayItem = itemToDisplay.GetComponent<MenuDisplayItem>();
        Title.text = itemType.ToString();
        Info.text = DisplayItem.InfoText;
    }

    void ViewParent()
    {

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
