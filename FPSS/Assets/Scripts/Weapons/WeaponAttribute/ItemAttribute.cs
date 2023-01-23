using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Attribute", menuName = "Attributes", order = 1)]
public class ItemAttribute : ScriptableObject
{
    [field:SerializeField] public string Name{get;private set;}

    [field:SerializeField] public ItemType Type{get;private set;}
    [field:SerializeField] public Sprite Icon{get;private set;}
}
