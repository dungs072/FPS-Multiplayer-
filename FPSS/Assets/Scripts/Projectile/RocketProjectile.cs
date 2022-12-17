using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : Projectile, IObjectPool
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject boomEffectPrefab;
    [SerializeField] private BoomEffect boom;
    [SerializeField] private float shootForce = 40f;
    private Coroutine coroutine;
    public override void ShootProjectile()
    {
        rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        coroutine = StartCoroutine(DeactiveRocket());
    }
    public override void ReturnToNewState(Vector3 position, Vector3 rotation)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        base.ReturnToNewState(position, rotation);

    }
    private void OnCollisionEnter(Collision other)
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        DoBoom();
    }
    private IEnumerator DeactiveRocket()
    {
        yield return new WaitForSeconds(timeToDeactive);
        DoBoom();
    }
    protected override void DoBoom()
    {
        GameObject boomEffectInstance = Instantiate(boomEffectPrefab, transform.position, Quaternion.identity);
        boom.Explode(damage,owner.transform,this.transform);//change this.transform to attacker's transform
        gameObject.SetActive(false);
    }


}
