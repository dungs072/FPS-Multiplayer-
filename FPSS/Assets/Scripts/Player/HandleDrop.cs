using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public interface ICanDrop
{
    ItemType GetWeaponType();
    GameObject GetItem();
}
public class HandleDrop : NetworkBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WeaponTPPManager weaponTPPManager;
    [SerializeField] private HealthManager healthManager;
    private Dictionary<ItemType,ICanDrop> items = new Dictionary<ItemType, ICanDrop>();
    private void Start() {
        if(!isOwned){return;}
        healthManager.OnDie+=ThrowItem;
    }
    private void OnDestroy() {
        if(!isOwned){return;}
        healthManager.OnDie-=ThrowItem;
    }
    private void Update() {
        if(!isOwned){return;}
        if(Input.GetKeyDown(KeyCode.B))
        {
            ThrowItem();
        }
    }
    public void ThrowItem()
    {
        if(weaponManager.CurrentWeapon.IsDefaultWeapon){return;}

        ItemType type = weaponManager.CurrentWeapon.WeaponType;
        weaponManager.ThrowFPSWeapon(type);
        CmdThrowItem(type);
        
    }
    public void AddItemCanDrop(ICanDrop item)
    {
        items[item.GetWeaponType()] = item;
    }
    public void RemoveItemCanDrop(ICanDrop item)
    {
        items.Remove(item.GetWeaponType());
    }
    [Command]
    private void CmdThrowItem(ItemType type)
    {
        RpcThrowItem(type);
    }
    [ClientRpc]
    private void RpcThrowItem(ItemType type)
    {
        GameObject item = items[type].GetItem();
        items.Remove(type);
        weaponTPPManager.ThrowWeapon((item.GetComponent<WeaponTPP>()));
        Destroy(item);
    }
}
