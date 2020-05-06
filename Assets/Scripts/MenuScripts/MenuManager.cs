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
    string InfoText { get; }
}

public class MenuManager : MonoBehaviour
{
    // Menu Panel Refs
    public GameObject ZoneBuildingDetailPanel;
    public GameObject ZoneDetailPanel;
    public GameObject EarthDetailPanel;
    public GameObject BuildZoneBuildingPanel;
    public Earth earth;

    void Update()
    {
        if (BuildMenu.PurchasedZoneBuilding == null)
        {
            RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
            foreach (RaycastHit hit in hitsInOrder)
            {
                ZoneBuilding zoneBuilding = hit.collider.GetComponent<ZoneBuilding>();
                if (zoneBuilding != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log(zoneBuilding.buildingType);
                        ZoneDetailPanel.SetActive(!ZoneDetailPanel.activeInHierarchy);
                        ZoneDetailPanel.GetComponent<DetailMenu>().Open(zoneBuilding);
                    }
                }
            }
        }
    }

    //-----Open Build Menu-------
    public void OpenMenu()
    {
        BuildZoneBuildingPanel.SetActive(!BuildZoneBuildingPanel.activeInHierarchy);
    }

    //sends new aliens
    public void SendAlienWave()
    {
        AlienSpawner.BeginInvasion();
    }
}
