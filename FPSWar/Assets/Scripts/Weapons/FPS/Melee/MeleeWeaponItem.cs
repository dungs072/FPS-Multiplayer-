using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MeleeWeaponItem : MonoBehaviour
{
    public event Action<Collider> OnCollideWithTarget;
    [SerializeField] private BoxCollider boxCollider;
    private void OnEnable() {
        ToggleCollider(false);
    }

    public void ToggleCollider(bool state)
    {
        boxCollider.enabled = state;
    }
    private void OnTriggerEnter(Collider other) {
        OnCollideWithTarget?.Invoke(other);
    }
}
