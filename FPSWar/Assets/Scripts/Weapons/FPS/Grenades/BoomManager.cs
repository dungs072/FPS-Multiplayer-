using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public enum BoomType
{
    GRENADE,
    SMOKE
}
public class BoomManager : NetworkBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private BoomProjectile grenade;
    [SerializeField] private BoomProjectile smokeGrenade;

    public void ThrowBoom(Vector3 direction, Vector3 spawnPoint, BoomType boomType)
    {
        if (!isOwned) { return; }
        CmdSpawnBoom(direction, spawnPoint, boomType);
    }

    public void TurnOnOneBoom(BoomType type)
    {
        // ShootWeaponBase currentWeapon = weaponManager.CurrentWeapon;
        // foreach (var boom in currentWeapon.Booms)
        // {
        //     if (boom.BoomType == type)
        //     {
        //         boom.gameObject.SetActive(true);
        //     }
        //     else
        //     {
        //         boom.gameObject.SetActive(false);
        //     }
        // }
    }
    public void TurnOffAllBooms()
    {
        // ShootWeaponBase currentWeapon = weaponManager.CurrentWeapon;
        // currentWeapon.BoomPack.gameObject.SetActive(false);
    }

    [Command]
    private void CmdSpawnBoom(Vector3 direction, Vector3 spawnPoint, BoomType boomType)
    {
        if (boomType == BoomType.GRENADE)
        {
            BoomProjectile boomInstance = Instantiate(grenade, spawnPoint, Quaternion.identity);
            boomInstance.SetOwner(GetComponent<PlayerController>());
            boomInstance.transform.forward = direction;
            NetworkServer.Spawn(boomInstance.gameObject);
        }
        else if(boomType==BoomType.SMOKE)
        {
            BoomProjectile boomInstance = Instantiate(smokeGrenade, spawnPoint, Quaternion.identity);
            boomInstance.SetOwner(GetComponent<PlayerController>());
            boomInstance.transform.forward = direction;
            NetworkServer.Spawn(boomInstance.gameObject);
        }



    }
}
