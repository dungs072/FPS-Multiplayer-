using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Projectile>(out Projectile proj))
        {
            proj.gameObject.SetActive(false);
            
        }
    }
}
