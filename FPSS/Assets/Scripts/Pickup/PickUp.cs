using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PickUp : MonoBehaviour
{
    public static event Action<GameObject> OnDestroyPickup; 
    [field:SerializeField] public ItemAttribute ItemAttribute{get;private set;}
    [field:SerializeField] public bool CanPickup {get;private set;} = true;
    [SerializeField] private float timeToDestroy = 20f;
    [SerializeField] private bool canDestroy = true;
    private void Start() {
        if(!canDestroy){return;}
        Destroy(gameObject,timeToDestroy);   
    }
    private void OnDestroy() {
        OnDestroyPickup?.Invoke(gameObject);
    }
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
