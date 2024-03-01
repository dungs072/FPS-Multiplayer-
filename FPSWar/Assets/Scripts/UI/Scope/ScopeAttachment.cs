using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeAttachment : MonoBehaviour
{
    [field:SerializeField] public ScopeInfor ScopeInfor{get;private set;}
    [field:SerializeField] public AnimatorOverrideController AnimatorOverride{get;private set;}

    [Header("some accessory of scope in gun")]
    [SerializeField] private GameObject scopeAccessory;

    
    public void ToggleDistanceScope(bool state)
    {
        if(scopeAccessory==null){return;}
        scopeAccessory.SetActive(state);
    }

}
