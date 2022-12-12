using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public event Action<int> OnTakeDamage;
    [SerializeField] private int multipleDamageAmount;
    [field:SerializeField] public PlayerController Owner{get;private set;}
    public void TakeDamage(int amount)
    {
        OnTakeDamage?.Invoke(amount*multipleDamageAmount);
    }
}
