using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class WeaponTPPManager : NetworkBehaviour
{
    public event Action<WeaponType> OnRemoveWeapon;
    [SerializeField] private List<WeaponTPP> weapons = new List<WeaponTPP>();
    [SerializeField] private Transform weaponPack;
    [SerializeField] private NetworkPlayerManager networkPlayerManager;
    [SerializeField] private HandleDrop handleDrop;
    private void Awake()
    {
        LoadWeaponFromResources("HandGun");
    }
    public void LoadTppWeapon(string name)
    {
        LoadWeaponFromResources(name);
        CmdLoadWeaponFromResources(name);
    }
    private void LoadWeaponFromResources(string name)
    {
        name = "TPPWeapons/"+name;
        WeaponTPP weapon = Resources.Load<WeaponTPP>(name);
        EquipWeapon(weapon);
    }
    private void EquipWeapon(WeaponTPP weapon)
    {
        WeaponTPP weaponInstance = Instantiate(weapon, weaponPack);
        weaponInstance.SetOwner(GetComponent<PlayerController>());
        weapons.Add(weaponInstance);
        handleDrop.AddItemCanDrop(weaponInstance);
        if(isOwned){return;}
        networkPlayerManager.ChangeObjectToTppsLayer(weaponInstance.gameObject);
    }
    public void ThrowWeapon(WeaponTPP weapon)
    {
        if(!weapons.Contains(weapon)){return;}
        OnRemoveWeapon?.Invoke(weapon.GetWeaponType());
        weapons.Remove(weapon);
    }
    public WeaponTPP ToggleWeapon(int index,bool state)
    {
        if(index>=weapons.Count){return null;}
        weapons[index].gameObject.SetActive(state);
        return weapons[index];
    }
    [Command]
    private void CmdLoadWeaponFromResources(string name)
    {
        RpcLoadWeaponFromResources(name);
    }
    [ClientRpc]
    private void RpcLoadWeaponFromResources(string name)
    {
        if(isOwned){return;}
        LoadWeaponFromResources(name);
    }

}
