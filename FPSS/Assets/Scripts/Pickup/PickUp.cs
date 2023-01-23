using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PickUp : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Sprite Icon{get;private set;}
    [field:SerializeField] public ItemType ItemType{get;private set;}
    [field:SerializeField] public bool CanPickup {get;private set;} = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<HandlePickUp>(out HandlePickUp pickup))
            {
                pickup.AddPickUpItem(this);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<HandlePickUp>(out HandlePickUp pickup))
            {
                pickup.RemovePickUpItem(this);
            }
        }
    }

}
