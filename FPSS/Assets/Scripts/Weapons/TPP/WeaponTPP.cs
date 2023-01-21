using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTPP : MonoBehaviour,ICanDrop
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PickUp pickup;
    [SerializeField] private ItemType weaponType;
    [SerializeField] private TypeObjectPoolManager typeObjectPoolManager;
    [SerializeField] private LayerMask layers;
    [Header("For projectile out")]
    [SerializeField] private GameObject rocket;
    [Header("Sound")]
    [SerializeField] private AudioClip shootSound;
    private PlayerController owner;
    public void SetOwner(PlayerController player)
    {
        owner = player;
    }

    public void ShootDirection(Vector3 targetPoint, Vector3 spread, int shootForce,float time,int damage)
    {
        Vector3 directionWithSpread = (targetPoint - attackPoint.position) + spread;
        if(!Physics.Raycast(attackPoint.position,directionWithSpread,out RaycastHit hit,float.MaxValue,layers))
        {
            hit.point = targetPoint;
        }
        ObjectPoolManager ojbPManager = ParentPoolManagers.Instance.GetObjPoolManager(typeObjectPoolManager);
        IProjectilePool prj = ojbPManager.GetReadyObject() as IProjectilePool;
        if (prj == null)
        {
            Projectile projectileInstance = Instantiate(projectile, attackPoint.position, Quaternion.identity);
            projectileInstance.transform.forward=  directionWithSpread;
            projectileInstance.SetRaycastHit(hit);
            projectileInstance.SetPlayerController(owner);
            projectileInstance.SetDamage(damage);
            projectileInstance.ShootProjectile();
        }
        else
        {
            prj.ReturnToNewState(attackPoint.position,directionWithSpread.normalized);
            prj.SetBeforeShoot(owner,damage,hit);
            prj.SetActive();
        }
        if(owner!=null)
        {
            owner.GetComponent<AudioSource>().PlayOneShot(shootSound);
        }
        if(muzzleFlash==null){return;}
        muzzleFlash.Play();

    }
    public GameObject GetRocketAtGun(){return rocket;}
    public PickUp GetPickupItem(){return pickup;}

    public ItemType GetWeaponType()
    {
        return weaponType;
    }

    public GameObject GetItem()
    {
        return gameObject;
    }
}
