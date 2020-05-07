using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
            _LastScreen = _CurrentScreen;
            _CurrentScreen = value;
            if (_CurrentScreen != MenuScreen.None)
                _CurrentScreenPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (BuildMenu.PurchasedZoneBuilding == null && Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
            foreach (RaycastHit hit in hitsInOrder)
            {
                IZoneable zoneBuildingHit = hit.collider.GetComponent<IZoneable>();
                if (zoneBuildingHit != null)
                {
                    _DetailMenu.Display(DisplayItemType.ZoneBuilding, hit.collider.gameObject);
                    CurrentlyDisplayedMenu = MenuScreen.Detail;
                    return;
                }
            }
        }
    }

    public void OpenShopMenu()
    {
        _LastScreen = MenuScreen.Shop;
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
        {
            CurrentlyDisplayedMenu = _LastScreen;
        }
        else
        {
            CurrentlyDisplayedMenu = MenuScreen.None;
        }
    }

    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }
}
