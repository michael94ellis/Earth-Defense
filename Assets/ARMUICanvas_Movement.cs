using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMUICanvas_Movement : MonoBehaviour, IUICanvas
{
    public void HideSelf()
    {
        gameObject.SetActive(false);
    }

    public void ShowSelf()
    {
        gameObject.SetActive(true);
    }

    
}
