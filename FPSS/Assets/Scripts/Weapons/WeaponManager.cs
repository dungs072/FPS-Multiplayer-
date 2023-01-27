using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using UnityStandardAssets.Characters.FirstPerson;
public enum ItemType
{
    HandGun,
    Assault,
    SMG,
    ShotGun,
    Sniper,
    RocketLaucher,
    Bullet
}
public class WeaponManager : NetworkBehaviour
{
    public event Action<WeaponBase> OnAddWeapon;
    public event Action<WeaponBase> OnRemoveWeapon;
    public event Action<int> OnChangeWeapon;
    public event Action<ItemType> OnChangeCrossHair;
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();
    [SerializeField] private Transform weaponPackTransform;
    [SerializeField] private NetworkPlayerManager networkPlayerManager;
    [SerializeField] private RigManager rigManager;
    public WeaponBase CurrentWeapon { get { return weapons[currentWeaponIndex]; } }

    public List<WeaponBase> Weapons { get { return weapons; } }
    [SyncVar(hook = nameof(OnChangeCurrentWeaponIndex))]
    private int currentWeaponIndex = 0;

    private void Start()
    {
        ChangeWeapon(currentWeaponIndex);
        StartCoroutine(SetInitialWeapon());
    }
    public override void OnStartAuthority()
    {
        OnChangeCrossHair += UIManager.Instance.ChangeCrossHair;
    }
    private void OnDestroy()
    {
        if (!isOwned) { return; }
        OnChangeCrossHair -= UIManager.Instance.ChangeCrossHair;
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
    public void ChangeWeaponIndex(int index)
    {
        if (index >= weapons.Count) { return; }
        currentWeaponIndex = index;
        ChangeWeapon(currentWeaponIndex);
    }
    private void ChangeWeapon(int index)
    {
        DoChangeWeapon();
        if (!isOwned) { return; }
        OnChangeCrossHair?.Invoke(CurrentWeapon.ItemAttribute.Type);
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
    public void EquipFpsWeapon(string name)
    {
        name = "FPSWeapons/" + name;
        EquipWeapon(name);
        CmdAddWeapon(name);
    }
    private void EquipWeapon(string name)
    {
        WeaponBase w = Resources.Load<WeaponBase>(name);
        WeaponBase weapon = Instantiate(w, weaponPackTransform);
        networkPlayerManager.ChangeLayerFPSWeapon(weapon.gameObject);
        FirstPersonController fps = GetComponent<FirstPersonController>();
        weapon.SetPlayerControllerAndFPS(GetComponent<PlayerController>(), fps);
        if (weapon.TryGetComponent<Scope>(out Scope scope))
        {
            scope.SetFPSController(fps);
        }
        if (weapon is ShotgunBase)
        {
            (weapon as ShotgunBase).SetTppController(GetComponent<ThirdPersonController>());
        }
        if (weapon.ItemAttribute.Type == ItemType.RocketLaucher)
        {
            rigManager.ChangeSecondHandGrabSourceTarget(1);
        }
        else
        {
            rigManager.ChangeSecondHandGrabSourceTarget();
        }
        weapons.Add(weapon);
        OnAddWeapon?.Invoke(weapon);
        if (isOwned)
        {
            int index = weapon.IsDefaultWeapon?0:1;
            PackWeaponUI pack = UIManager.Instance.Packs[index];
            pack.ChangeWeaponDisplay(weapon.ItemAttribute.Icon);
            pack.ChangeBulletDisplay(weapon.BulletIcon);
            pack.ChangeBulletLeftAmountDisplay(weapon.BulletMaxInMag, weapon.BulletLeft);
            weapon.OnChangeBulletLeft += UIManager.Instance.Packs[index].ChangeBulletLeftAmountDisplay;
            OnChangeCrossHair?.Invoke(weapon.ItemAttribute.Type);
        }
    }
    public void ThrowFPSWeapon(string nameWeapon)
    {
        currentWeaponIndex = 0;
        DoChangeWeapon();
        CmdSetCurrentWeaponIndex(currentWeaponIndex);
        ThrowWeapon(nameWeapon);
        CmdRemoveWeapon(nameWeapon,false);
    }
    private void ThrowWeapon(string nameWeapon,bool canThrowDefaultWeapon = false)
    {
        int index = canThrowDefaultWeapon?0:1;
        foreach (var weapon in weapons)
        {
            if (weapon.ItemAttribute.Name == nameWeapon && (!weapon.IsDefaultWeapon || canThrowDefaultWeapon))
            {
                weapons.Remove(weapon);
                OnRemoveWeapon?.Invoke(weapon);
                weapon.OnChangeBulletLeft -= UIManager.Instance.Packs[index].ChangeBulletLeftAmountDisplay;
                Destroy(weapon.gameObject);
                break;
            }
        }
        if (isOwned)
        {
            UIManager.Instance.ClearPackWeapon();
        }
        OnChangeCrossHair?.Invoke(CurrentWeapon.ItemAttribute.Type);
    }
    public void SetFullBulletLeft()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetFullBulletLeft();
        }
    }
    public void ThrowDefaultWeapon()
    {
        foreach (var weapon in weapons)
        {
            if (weapon.IsDefaultWeapon)
            {
                string nameWeapon = weapon.ItemAttribute.Name;
                currentWeaponIndex = 0;
                DoChangeWeapon();
                CmdSetCurrentWeaponIndex(currentWeaponIndex);
                ThrowWeapon(nameWeapon,true);
                CmdRemoveWeapon(nameWeapon,true);
                print(currentWeaponIndex);
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
    private void RpcAddWeapon(string name)
    {
        if (isOwned)
        {
            currentWeaponIndex = weapons.Count - 1;
            DoChangeWeapon();
            CmdSetCurrentWeaponIndex(currentWeaponIndex);
            return;
        }
        EquipWeapon(name);
    }
    [ClientRpc]
    private void RpcRemoveWeapon(string nameWeapon,bool canThrowDefaultWeapon)
    {
        if (isOwned) { return; }
        ThrowWeapon(nameWeapon,canThrowDefaultWeapon);
    }
    #endregion
    #region Server
    [Command]
    private void CmdSetCurrentWeaponIndex(int index)
    {
        currentWeaponIndex = index;
    }
    [Command]
    private void CmdAddWeapon(string name)
    {
        RpcAddWeapon(name);
    }
    [Command]
    private void CmdRemoveWeapon(string nameWeapon,bool canThrowDefaultWeapon)
    {
        RpcRemoveWeapon(nameWeapon,canThrowDefaultWeapon);
    }
    #endregion

}
