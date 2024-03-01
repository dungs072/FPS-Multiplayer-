using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBase : ShootWeaponBase
{
    [SerializeField] private GameObject rocket;
    protected override void Attack(bool isAiming)
    {
        base.Attack(isAiming);
        rocket.SetActive(false);
    }
    protected override void Reload()
    {
        base.Reload();
        rocket.SetActive(true);
    }
    protected override ObjectPoolManager GetObjectPoolManager()
    {
        return ParentPoolManagers.Instance.GetObjPoolManager(TypeObjectPoolManager.ROCKET_FPS);
    }

}
