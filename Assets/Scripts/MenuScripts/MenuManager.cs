using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface Weapon
{
    void FireAt(Vector3 target);
    IEnumerator Fire();
    IEnumerator Recharge();
}
public interface Damageable
{
    bool TakeDamage(int amount);
}

public enum ZoneBuildingType
{
    LaserTurret,
    MissileSilo,
    City,
    ShieldGenerator
}

public interface ZoneBuilding
{
    bool isActive { get; set; }
    Transform buildingTransform { get; }
    EarthZone ParentZone { get; set; }
    ZoneBuildingType buildingType { get; set; }
}

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

public class MenuManager : MonoBehaviour
{
    public Earth earth;
    // Menu Panel Refs
    public GameObject UpgradePanel;
    public GameObject DetailPanel;
    public DetailMenu _DetailMenu;
    public GameObject ShopPanel;
    // Menu Panel enum to control which panels can be displayed
    public enum MenuScreen
    {
        None,
        Upgrade,
        Detail,
        Shop
    }
    // Private variables for displaying a screen to the user
    private GameObject _CurrentScreenPanel
    {
        get
        {
            switch (_CurrentScreen)
            {
                case MenuScreen.Shop:
                    return ShopPanel;
                case MenuScreen.Detail:
                    return DetailPanel;
                case MenuScreen.Upgrade:
                    return UpgradePanel;
            }
            return null;
        }
    }
    // Set this from other scripts to control which menu is showing
    private MenuScreen _LastScreen = MenuScreen.Shop;
    private MenuScreen _CurrentScreen = MenuScreen.None;
    public MenuScreen CurrentlyDisplayedMenu
    {
        get { return _CurrentScreen; }
        set
        {
            DetailPanel.SetActive(false);
            ShopPanel.SetActive(false);
            UpgradePanel.SetActive(false);
            _CurrentScreen = value;
            if (_CurrentScreen != MenuScreen.None)
                _CurrentScreenPanel.SetActive(true);
        }
    }

    //void Start()
    //{
    //    _DetailMenu = DetailPanel.GetComponent<DetailMenu>();
    //}

    void Update()
    {
        if (BuildMenu.PurchasedZoneBuilding == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
                foreach (RaycastHit hit in hitsInOrder)
                {
                    EarthZone earthZoneHit = hit.collider.GetComponent<EarthZone>();
                    ZoneBuilding zoneBuildingHit = hit.collider.GetComponent<ZoneBuilding>();
                    if (zoneBuildingHit != null)
                    {
                        _DetailMenu.Display(DisplayItemType.ZoneBuilding, hit.collider.gameObject);
                        CurrentlyDisplayedMenu = MenuScreen.Detail;
                        return;
                    }
                    else if (earthZoneHit != null)
                    {
                        CurrentlyDisplayedMenu = MenuScreen.Detail;
                        return;
                    }
                }
            }
        }
    }

    public void OpenShopMenu()
    {
        CurrentlyDisplayedMenu = MenuScreen.Shop;
    }

    public void OpenDetailMenu()
    {
        CurrentlyDisplayedMenu = MenuScreen.Detail;
    }

    //public void OpenUpgradeMenu()
    //{
    //    CurrentlyDisplayedMenu = MenuScreen.Upgrade;
    //}

    public void MenuButtonPress()
    {
        if (_CurrentScreen == MenuScreen.None)
            CurrentlyDisplayedMenu = _LastScreen;
        else
        {
            _LastScreen = _CurrentScreen;
            CurrentlyDisplayedMenu = MenuScreen.None;
        }
    }

    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }
}
