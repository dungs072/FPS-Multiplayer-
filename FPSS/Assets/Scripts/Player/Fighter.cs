using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] private ReferenceManager refer;


    public void Attack()
    {
        WeaponBase weapon = refer.WeaponManager.CurrentWeapon;
        weapon.CheckShoot(true);
    }
    public void Aim(bool state)
    {   
        refer.WeaponManager.CurrentWeapon.SetCanAim(state);
    }
    public void Reload()
    {
        refer.WeaponManager.CurrentWeapon.CheckReload();
    }
}
