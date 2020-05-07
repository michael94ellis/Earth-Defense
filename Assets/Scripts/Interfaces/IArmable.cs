using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArmable
{
    
        void FireAt(Vector3 target);
        IEnumerator Fire();
        IEnumerator Recharge();

        //float ReloadTime { get; set; }
        //float Range { get; set; }
        //float Damage { get; set; }
    
}
