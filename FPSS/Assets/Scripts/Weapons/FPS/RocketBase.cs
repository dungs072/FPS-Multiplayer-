using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBase : WeaponBase
{
    [SerializeField] private GameObject rocket;
    protected override void Shoot(bool isAiming)
    {
        base.Shoot(isAiming);
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
