using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScopeSelection : MonoBehaviour
{
    [SerializeField] private GameObject ring;
    public event Action<bool> OnSelect;
    public event Action OnDeactive;
    public event Action<int,bool> OnToggleScope;
    private bool isSwitchOn = false;
    public int Index{get;set;}

    public void OnClick()
    {
        isSwitchOn = !isSwitchOn;
        OnDeactive?.Invoke();
        OnSelect?.Invoke(isSwitchOn);
        OnToggleScope?.Invoke(Index,isSwitchOn);
        ToggleRing(true);
    }
    public void ToggleRing(bool state)
    {
        ring.gameObject.SetActive(state);
    }
}
