using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrbitButtonBehaviour : MonoBehaviour
{
    [SerializeField] private VoidEvent OnButtonTapped;


    public void ButtonTapped()
    {
        OnButtonTapped.Raise();
    }
}
