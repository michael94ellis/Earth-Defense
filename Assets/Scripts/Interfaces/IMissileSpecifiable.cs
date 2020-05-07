using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMissileSpecifiable
{
        string name { get; }
        float launchSpeed { get; set; }
        float moveSpeed { get; set; }
        float damage { get; set; }
    
}
