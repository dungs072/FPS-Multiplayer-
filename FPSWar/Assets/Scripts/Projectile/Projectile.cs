using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SurfaceMaterial
{
    BLOOD,
    METAL
}
public class Projectile : MonoBehaviour, IProjectilePool
{
    [SerializeField] private GameObject stoneEffect;
    [SerializeField] private GameObject metalEffect;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private TypeObjectPoolManager Type;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] protected float timeToDeactive = 5f;

    protected PlayerController owner;
    protected int damage = 10;
    protected RaycastHit hit;
    protected
    private void Awake()
    {
        ObjectPoolManager poolManager = ParentPoolManagers.Instance.GetObjPoolManager(Type);
        poolManager.AddObjPool(this);
        transform.SetParent(poolManager.transform);
    }
    public virtual void ShootProjectile()
    {
        trail.AddPosition(transform.position);
        HandleCollideWithHealthObject();
        transform.position = hit.point;
        StartCoroutine(DeactiveObject());
    }
    private IEnumerator DeactiveObject()
    {
        yield return new WaitForSeconds(timeToDeactive);
        DoBoom();
    }
    protected virtual void DoBoom()
    {
        gameObject.SetActive(false);
    }
    public void SetRaycastHit(RaycastHit hit)
    {
        this.hit = hit;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public void SetPlayerController(PlayerController player)
    {
        this.owner = player;
    }
    protected virtual void HandleCollideWithHealthObject()
    {
        if (hit.transform == null) { return; }
        if (!hit.transform.TryGetComponent<Health>(out Health health)) { HandleCollision(); return; }
        GameObject bloodInstance = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
        bloodInstance.transform.SetParent(hit.transform);

        if (owner != null)
        {
            owner.TriggerHitCrossHair();
            if (owner == health.Owner) { return; }
        }
        health.TakeDamage(damage, owner?.transform);
    }

    protected virtual void HandleCollision()
    {
        if (hit.transform.CompareTag("Concrete"))
        {
            GameObject stoneEffectInstance = Instantiate(stoneEffect, hit.point,
                                            Quaternion.LookRotation(hit.normal));
            stoneEffectInstance.transform.SetParent(hit.transform);
        }
        else
        {
            GameObject metalEffectInstance = Instantiate(metalEffect, hit.point,
                                            Quaternion.LookRotation(hit.normal));
            metalEffectInstance.transform.SetParent(hit.transform);
        }

    }
    public bool IsReadyForTakeOut()
    {
        return !gameObject.activeSelf;
    }

    public virtual void ReturnToNewState(Vector3 position, Vector3 rotation)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(rotation);
        if (trail == null) { return; }
        trail.Clear();
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
        ShootProjectile();
    }

    public void SetBeforeShoot(PlayerController owner, int damage, RaycastHit hit)
    {
        this.owner = owner;
        this.damage = damage;
        this.hit = hit;
    }
}
