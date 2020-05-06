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

public class MenuManager : MonoBehaviour
{
    public Earth earth;
    // Menu Panel Refs
    public GameObject UpgradePanel;
    public GameObject DetailPanel;
    public GameObject ShopPanel;
    // Menu Panel enum to control which panels can be displayed
    public enum MenuScreen
    {
        Upgrade,
        Detail,
        Shop
    }
    // Private variables for displaying a screen to the user
    private bool isDisplayingMenu = false;
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
    private MenuScreen _CurrentScreen = MenuScreen.Shop;
    public MenuScreen CurrentlyDisplayedMenu
    {
        get { return _CurrentScreen; }
        set
        {
            _CurrentScreen = value;
            // Disable all screens
            ShopPanel.SetActive(false);
            DetailPanel.SetActive(false);
            UpgradePanel.SetActive(false);
            // Show Correct Screen, null means dont show a screen
            if (_CurrentScreenPanel != null)
                _CurrentScreenPanel.SetActive(true);
        }
    }

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

    public void MenuButtonPress()
    {
        isDisplayingMenu = !isDisplayingMenu;
        _CurrentScreenPanel.SetActive(isDisplayingMenu);
    }

    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }
}
