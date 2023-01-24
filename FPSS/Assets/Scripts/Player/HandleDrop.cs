using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public interface ICanDrop
{
    string GetNameItem();
    GameObject GetItem();
}
public class HandleDrop : NetworkBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WeaponTPPManager weaponTPPManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private Transform posDrop;
    private Dictionary<string,ICanDrop> items = new Dictionary<string, ICanDrop>();
    private PickUp[] pickUpItems;
    public override void OnStartServer()
    {
        pickUpItems = Resources.LoadAll<PickUp>("PickUpItems/");
    }
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
            ThrowItem(true);
        }
    }
    private void ThrowItem()
    {
        ThrowItem(true);
    }
    public void ThrowItem(bool canSpawn = false)
    {
        if(weaponManager.CurrentWeapon.IsDefaultWeapon){return;}

        string nameWeapon = weaponManager.CurrentWeapon.ItemAttribute.Name;
        weaponManager.ThrowFPSWeapon(nameWeapon);
        CmdThrowItem(nameWeapon,canSpawn);
    }
    public void AddItemCanDrop(ICanDrop item)
    {
        items[item.GetNameItem()] = item;
    }
    public void RemoveItemCanDrop(ICanDrop item)
    {
        items.Remove(item.GetNameItem());
    }
    [Command]
    private void CmdThrowItem(string nameWeapon,bool canSpawn)
    {
        RpcThrowItem(nameWeapon);
        if(!canSpawn){return;}
        foreach(var item in pickUpItems)
        {
            if(item.ItemAttribute==null){continue;}
            if(item.ItemAttribute.Name==nameWeapon)
            {
                GameObject pickupItem = Instantiate(item.gameObject,posDrop.position,Quaternion.identity);
                NetworkServer.Spawn(pickupItem);
                break;
            }
        }
    }
    [ClientRpc]
    private void RpcThrowItem(string nameWeapon)
    {
        GameObject item = items[nameWeapon].GetItem();
        items.Remove(nameWeapon);
        weaponTPPManager.ThrowWeapon((item.GetComponent<WeaponTPP>()));
        Destroy(item);
    }
}
