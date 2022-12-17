using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private float explodeForce = 10f;
    [SerializeField] private LayerMask blockLayers;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private int maxColliders = 50;
    private Collider[] colliders;
    private void Awake() {
        colliders = new Collider[maxColliders];
    }
    public void Explode(int damage,Transform attackingOwner,Transform attacker)
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, hitLayers);
        for (int i = 0; i < hits; i++)
        {
            if (colliders[i].TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                if (!Physics.Raycast(transform.position, (colliders[i].transform.position - transform.position).normalized, distance, blockLayers.value))
                {
                    rb.AddExplosionForce(explodeForce, transform.position, radius);
                    if (colliders[i].TryGetComponent<HealthManager>(out HealthManager healthManager))
                    {
                        int finalDamage = Mathf.Abs(damage - (int)(distance));
                        healthManager.TakeDamage(finalDamage,false,attackingOwner,attacker);
                    }
                }

            }
        }
        Array.Clear(colliders,0,colliders.Length);
    }
    private float CalculateDistanceBetweenCenterExplodeAndTarget(Vector3 target)//for improving performance
    {
        return (target - transform.position).sqrMagnitude;
    }

}
