using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public event Action<int,bool,Transform,Transform, bool> OnTakeDamage;
    [SerializeField] private float multipleDamageAmount;
    [SerializeField] private bool isHead;
    [field:SerializeField] public PlayerController Owner{get;private set;}
    public bool IsHead{get{return isHead;}}
    public void TakeDamage(int amount,Transform attacker)
    {
        OnTakeDamage?.Invoke((int)(amount*multipleDamageAmount),isHead,attacker,attacker,false);
    }

}
