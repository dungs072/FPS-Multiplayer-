using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
public enum WeaponType
{
    HandGun,
    Assault,
    SMG,
    ShotGun,
    Sniper,
    RocketLaucher
}
public class WeaponManager : NetworkBehaviour
{
    public event Action<WeaponBase> OnAddWeapon;
    public event Action<WeaponBase> OnRemoveWeapon;
    public event Action<int> OnChangeWeapon;
    [SerializeField] private List<WeaponBase> totalWeapons;
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();

    public WeaponBase CurrentWeapon { get { return weapons[currentWeaponIndex]; } }

    public List<WeaponBase> Weapons { get { return weapons; } }
    [SyncVar(hook = nameof(OnChangeCurrentWeaponIndex))]
    private int currentWeaponIndex = 0;

    private void Start()
    {
        ChangeWeapon(currentWeaponIndex);
        StartCoroutine(SetInitialWeapon());
    }
    private IEnumerator SetInitialWeapon()
    {
        yield return null;
        OnAddWeapon?.Invoke(weapons[0]);
        CurrentWeapon.OnChangeBulletLeft += UIManager.Instance.Packs[weapons.Count - 1].ChangeBulletLeftAmountDisplay;
    }
    private void Update()
    {
        if (!isOwned) { return; }
        if (CurrentWeapon.IsReloading) { return; }
        HandleKeyInput();
        HandleMouseScroll();
    }
    private void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeaponIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeaponIndex(1);
        }
    }

    private void HandleMouseScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ScrollUpWeapon();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ScrollDownWeapon();
        }
    }
    private void ScrollUpWeapon()
    {
        int index = (currentWeaponIndex + 1) % weapons.Count;
        ChangeWeaponIndex(index);
    }
    private void ScrollDownWeapon()
    {
        int index = currentWeaponIndex - 1;
        if (index < 0)
        {
            index = weapons.Count - 1;
        }
        ChangeWeaponIndex(index);
    }
    private void ChangeWeaponIndex(int index)
    {
        if (index >= weapons.Count) { return; }
        currentWeaponIndex = index;
        ChangeWeapon(currentWeaponIndex);
    }
    private void ChangeWeapon(int index)
    {
        DoChangeWeapon();
        if (!isOwned) { return; }
        CmdSetCurrentWeaponIndex(index);
    }

    private void DoChangeWeapon()
    {
        TriggerCurrentWeapon();
        OnChangeWeapon?.Invoke(currentWeaponIndex);
    }

    private void TriggerCurrentWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == currentWeaponIndex)
            {
                weapons[i].gameObject.SetActive(true);
                continue;
            }
            weapons[i].gameObject.SetActive(false);
        }
    }
    public void EquipFpsWeapon(WeaponType type)
    {
        EquipWeapon(type);
        CmdAddWeapon(type);


    }
    private void EquipWeapon(WeaponType type)
    {
        foreach (var weapon in totalWeapons)
        {
            if (weapon.WeaponType == type)
            {
                weapons.Add(weapon);
                OnAddWeapon?.Invoke(weapon);
                if (isOwned)
                {
                    PackWeaponUI pack = UIManager.Instance.Packs[weapons.Count - 1];
                    pack.ChangeWeaponDisplay(weapon.GunIcon);
                    pack.ChangeBulletDisplay(weapon.BulletIcon);
                    pack.ChangeBulletLeftAmountDisplay(weapon.BulletMaxInMag, weapon.BulletLeft);
                    weapon.OnChangeBulletLeft += UIManager.Instance.Packs[weapons.Count - 1].ChangeBulletLeftAmountDisplay;
                }

                break;
            }
        }
    }
    public void ThrowFPSWeapon(WeaponType type)
    {
        currentWeaponIndex = 0;
        DoChangeWeapon();
        CmdSetCurrentWeaponIndex(currentWeaponIndex);
        CmdRemoveWeapon(type);
    }
    private void ThrowWeapon(WeaponType type)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.WeaponType == type && !weapon.IsDefaultWeapon)
            {
                weapons.Remove(weapon);
                OnRemoveWeapon?.Invoke(weapon);
                weapon.OnChangeBulletLeft -= UIManager.Instance.Packs[weapons.Count - 1].ChangeBulletLeftAmountDisplay;
                break;
            }
        }
    }
    #region Client
    private void OnChangeCurrentWeaponIndex(int oldIndex, int newIndex)
    {
        if (isOwned) { return; }
        TriggerCurrentWeapon();
        OnChangeWeapon?.Invoke(newIndex);
    }
    [ClientRpc]
    private void RpcAddWeapon(WeaponType type)
    {
        if (isOwned)
        {
            currentWeaponIndex = weapons.Count - 1;
            DoChangeWeapon();
            CmdSetCurrentWeaponIndex(currentWeaponIndex);
            return;
        }
        EquipWeapon(type);
    }
    [ClientRpc]
    private void RpcRemoveWeapon(WeaponType type)
    {
        ThrowWeapon(type);
    }
    #endregion
    #region Server
    [Command]
    private void CmdSetCurrentWeaponIndex(int index)
    {
        currentWeaponIndex = index;
    }
    [Command]
    private void CmdAddWeapon(WeaponType type)
    {
        RpcAddWeapon(type);
    }
    [Command]
    private void CmdRemoveWeapon(WeaponType type)
    {
        RpcRemoveWeapon(type);
    }
    #endregion

}
