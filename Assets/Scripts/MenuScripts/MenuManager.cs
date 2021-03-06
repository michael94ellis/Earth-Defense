﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface Weapon
{
    void FireAt(Vector3 target);
    IEnumerator Fire();
    IEnumerator Recharge();

    //float ReloadTime { get; set; }
    //float Range { get; set; }
    //float Damage { get; set; }
}
public interface Damageable
{
    bool TakeDamage(int amount);
}

public enum ZoneBuildingType
{
    City,
    Capitol,
    Factory,
    PowerPlant,
    Bunker,
    ShieldGenerator,
    MissileSilo,
    RailGun,
    LaserTurret,
}

public class BuildingUpgrade
{
    public string name;
    public delegate void UpgradeDelegate();
    public UpgradeDelegate performUpgrade;

    public BuildingUpgrade(string newName, UpgradeDelegate method)
    {
        name = newName;
        performUpgrade = method;
    }
}
public class BuildingStat
{
    public string name;
    public float value;

    public BuildingStat(string newName, float newValue)
    {
        name = newName;
        value = newValue;
    }
}

public interface ZoneBuilding
{
    bool isActive { get; set; }
    Transform buildingTransform { get; }
    EarthZone ParentZone { get; set; }
    ZoneBuildingType buildingType { get; set; }
    List<BuildingUpgrade> upgrades { get; }
    List<BuildingStat> Stats { get; }
    int PowerCost { get; set; }
    int PopulationCost { get; set; }
}

public class MenuManager : MonoBehaviour
{
    public Earth earth;
    // Menu Panel Refs
    public GameObject DetailPanel;
    public DetailMenu DetailMenu;
    public GameObject UpgradePanel;
    public UpgradeMenu UpgradeMenu;
    public GameObject ShopPanel;
    public BuildMenu ShopMenu;
    public GameObject ZonePanel;
    public ZoneMenu ZoneMenu;

    public ZoneBuilding DisplayItem;

    // Menu Panel enum to control which panels can be displayed
    public enum MenuScreen
    {
        None,
        Zone,
        Detail,
        Upgrade,
        Shop
    }
    // Private variables for displaying a screen to the user
    private GameObject _CurrentScreenPanel
    {
        get
        {
            switch (_CurrentScreen)
            {
                case MenuScreen.Detail:
                    return DetailPanel;
                case MenuScreen.Zone:
                    return ZonePanel;
                case MenuScreen.Shop:
                    return ShopPanel;
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
            ZonePanel.SetActive(false);
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
                ZoneBuilding zoneBuildingHit = hit.collider.GetComponent<ZoneBuilding>();
                if (zoneBuildingHit != null)
                {
                    DisplayItem = zoneBuildingHit;
                    DetailMenu.Display();
                    CurrentlyDisplayedMenu = MenuScreen.Detail;
                    return;
                }
            }
        }
    }

    public void OpenShopMenu()
    {
        ShopMenu.Display();
    }

    public void OpenZoneMenu()
    {
        ZoneMenu.Display();
    }

    public void DismissShopMenu()
    {
        CurrentlyDisplayedMenu = MenuScreen.None;
    }

    public void OpenDetailMenu()
    {
        DetailMenu.Display();
    }

    public void OpenUpgradeMenu()
    {
        UpgradeMenu.Display();
    }

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
