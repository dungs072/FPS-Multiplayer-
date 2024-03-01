using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInvoking : MonoBehaviour
{
    public event Action OnEnableEvent;
    private void OnEnable() {
        OnEnableEvent?.Invoke();
    }
}
