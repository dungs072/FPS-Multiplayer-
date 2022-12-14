using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float throwForce = 40f;
    [SerializeField] private float timeExplode = 4;
    [SerializeField] private int damage = 20;
    [SerializeField] private GameObject boomEffectPrefab;
    [SerializeField] private BoomEffect boom;
    [field:SerializeField] public BoomType BoomType{get;private set;}
    private void Start() {
        ThrowBoom();
    }
    private void ThrowBoom()
    {
        rb.AddForce(transform.forward*throwForce, ForceMode.Impulse);
        StartCoroutine(Explode());
    }
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(timeExplode);
        Instantiate(boomEffectPrefab,transform.position,Quaternion.identity);
        boom.Explode(damage,this.transform);
        Destroy(gameObject);
    }
}
