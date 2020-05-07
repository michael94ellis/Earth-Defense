using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZoneable
    {
        bool isActive { get; set; }
        Transform buildingTransform { get; }
        EarthZone ParentZone { get; set; }
        ZoneBuildingType buildingType { get; set; }
        List<BuildingUpgrade> upgrades { get; }
    
}
