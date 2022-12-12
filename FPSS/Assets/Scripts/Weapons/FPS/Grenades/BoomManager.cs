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

    public void ThrowBoom(Vector3 direction,Vector3 spawnPoint)
    {
        if (!isOwned) { return; }
        CmdSpawnBoom(direction,spawnPoint);
    }

    public void TurnOnOneBoom(BoomType type)
    {
        WeaponBase currentWeapon = weaponManager.CurrentWeapon;
        foreach (var boom in currentWeapon.Booms)
        {
            if (boom.BoomType == type)
            {
                boom.gameObject.SetActive(true);
            }
            else
            {
                boom.gameObject.SetActive(false);
            }
        }
    }
    public void TurnOffAllBooms()
    {
        WeaponBase currentWeapon = weaponManager.CurrentWeapon;
        currentWeapon.BoomPack.gameObject.SetActive(false);
    }
    [Command]
    private void CmdSpawnBoom(Vector3 direction,Vector3 spawnPoint)
    {
        BoomProjectile boomInstance = Instantiate(grenade, spawnPoint, Quaternion.identity);
        boomInstance.transform.forward = direction;
        NetworkServer.Spawn(boomInstance.gameObject);

    }
}
