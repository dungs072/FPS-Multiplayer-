using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBullet : MonoBehaviour, IObjectPool
{
    private Quaternion defaultRotation;
    private void Start() {
        defaultRotation = transform.rotation;
    }
    private void OnEnable() {
        
    }
    public bool IsReadyForTakeOut()
    {
        throw new System.NotImplementedException();
    }

    public void ReturnToNewState(Vector3 position, Vector3 rotation)
    {
        throw new System.NotImplementedException();
    }

    public void SetActive()
    {
        throw new System.NotImplementedException();
    }
}
