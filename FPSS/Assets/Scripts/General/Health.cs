using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public event Action<int,Transform> OnTakeDamage;
    [SerializeField] private int multipleDamageAmount;
    [field:SerializeField] public PlayerController Owner{get;private set;}
    public void TakeDamage(int amount,Transform attacker)
    {
        OnTakeDamage?.Invoke(amount*multipleDamageAmount,attacker);
    }
}
