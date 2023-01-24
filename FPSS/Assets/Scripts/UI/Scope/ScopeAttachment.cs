using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeAttachment : MonoBehaviour
{
    [field:SerializeField] public ScopeInfor ScopeInfor{get;private set;}
    [field:SerializeField] public AnimatorOverrideController AnimatorOverride{get;private set;}
    [SerializeField] private GameObject scopeInGun;
    

}
