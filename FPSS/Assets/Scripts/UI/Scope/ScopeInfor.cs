using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scope Information", menuName = "Attributes/Scope", order = 1)]
public class ScopeInfor : ScriptableObject
{
    [field:SerializeField] public Sprite Icon{get;private set;}
    [field:SerializeField] public string Name{get;private set;}
}
