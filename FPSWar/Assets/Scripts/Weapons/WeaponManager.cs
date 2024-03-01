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
    Bullet,
    Melee,
    Grenade
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
    [SerializeField] private int maxWeapons = 2;
    [SerializeField] private RespawnManager respawnManager;
    public WeaponBase CurrentWeapon { get { return weapons[currentWeaponIndex]; } }
    public ShootWeaponBase CurrentShootWeapon
    {
        get
        {
            return CurrentWeapon as ShootWeaponBase;
        }
    }
    public MeleeWeaponBase CurrentMeleeWeapon
    {
        get
        {
            return CurrentWeapon as MeleeWeaponBase;
        }
    }
    public bool IsMaxWeaponCount { get { return weapons.Count == maxWeapons; } }
    public bool IsMaxShootWeapon
    {
        get
        {
            foreach(var weapon in weapons)
            {
                if(weapon is ShootWeaponBase && !weapon.IsDefaultWeapon)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public GrenadeWeaponBase FragGrenade
    {
        get
        {
            foreach(var weapon in weapons)
            {
                if(weapon is GrenadeWeaponBase)
                {
                    var grenade = weapon as GrenadeWeaponBase;
                    if(grenade.BoomType==BoomType.GRENADE)
                    {
                        return grenade;
                    }
                }
            }
            return null;
        }
    }
    public GrenadeWeaponBase SmokeGrenade
    {
        get
        {
            foreach(var weapon in weapons)
            {
                if(weapon is GrenadeWeaponBase)
                {
                    var grenade = weapon as GrenadeWeaponBase;
                    if(grenade.BoomType==BoomType.SMOKE)
                    {
                        return grenade;
                    }
                }
            }
            return null;
        }
    }
    public List<WeaponBase> Weapons { get { return weapons; } }
    [SyncVar(hook = nameof(OnChangeCurrentWeaponIndex))]
    private int currentWeaponIndex = 0;

    private void Start()
    {
        ChangeWeapon(currentWeaponIndex);
        StartCoroutine(SetInitialWeapon());
        if(isOwned)
        {
            respawnManager.OnRespawn+=ResetAllWeapons;
        }
    }
    public override void OnStartAuthority()
    {
        OnChangeCrossHair += UIManager.Instance.ChangeCrossHair;
        Team.OnReturnLobbyInGame+=ChangeToDefaultWeapon;

    }
    private void OnDestroy()
    {
        if (!isOwned) { return; }
        respawnManager.OnRespawn-=ResetAllWeapons;
        OnChangeCrossHair -= UIManager.Instance.ChangeCrossHair;
        Team.OnReturnLobbyInGame-=ChangeToDefaultWeapon;
    }
    private IEnumerator SetInitialWeapon()
    {
        yield return null;
        foreach (var weapon in weapons)
        {
            OnAddWeapon?.Invoke(weapon);
        }
        if (CurrentWeapon is ShootWeaponBase)
        {
            (CurrentWeapon as ShootWeaponBase).OnChangeBulletLeft += UIManager.Instance.Packs[0].ChangeBulletLeftAmountDisplay;
        }
        

    }
    private bool CheckCurrentWeaponIsReloading()
    {
        if (CurrentWeapon is ShootWeaponBase)
        {
            if ((CurrentWeapon as ShootWeaponBase).IsReloading) { return true; }
        }
        return false;
    }
    public bool CanTriggerReloadInTPP()
    {
        if (CurrentWeapon is ShootWeaponBase)
        {
            var shootWeapon = CurrentWeapon as ShootWeaponBase;
            if (shootWeapon.IsReloading) { return false; }
            if (shootWeapon.IsComplexReload) { return false; }
        }
        return true;
    }
    private void Update()
    {
        if (!isOwned) { return; }
        if (CheckCurrentWeaponIsReloading()) { return; }
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
    private void ChangeToDefaultWeapon()
    {
        ChangeWeaponIndex(0);
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
    public void ResetAllWeapons()
    {
        foreach(var weapon in weapons)
        {
            weapon.CanUse = true;
            if(weapon is GrenadeWeaponBase)
            {
                (weapon as GrenadeWeaponBase).GrenadeAmmount = 2;
            }
        }
    }
    public void ChangeCurrentWeaponIndex()
    {
        ChangeWeaponIndex(currentWeaponIndex);
    }
    public void ChangeWeaponIndex(int index)
    {
        if (index >= weapons.Count) { return; }
        index = GetWeaponIndexCanUse(index);
        currentWeaponIndex = index;
        ChangeWeapon(currentWeaponIndex);
    }
    private int GetWeaponIndexCanUse(int index)
    {
        if(weapons[index].CanUse){return index;}
        for(int i =weapons.Count-1;i>=0;i--)
        {
            if(index==i){continue;}
            if(weapons[i].CanUse)
            {
                if(weapons[i] is ShootWeaponBase)
                {
                    return i;
                }
            }
        }
        return index;
    }
    private void ChangeWeapon(int index)
    {
        DoChangeWeapon();
        if (!isOwned) { return; }
        OnChangeCrossHair?.Invoke(CurrentWeapon.ItemAttribute.Type);
        UpdateWeaponPackUI(index);
        CmdSetCurrentWeaponIndex(index);
    }
    private void UpdateWeaponPackUI(int index)
    {
        PackWeaponUI pack = UIManager.Instance.Packs[0];
        pack.ChangeWeaponDisplay(weapons[index].ItemAttribute.Icon);
        if (weapons[index] is ShootWeaponBase)
        {
            var shootWeapon = weapons[index] as ShootWeaponBase;
            pack.ChangeBulletLeftAmountDisplay(shootWeapon.BulletLeftInMag, shootWeapon.BulletLeft);
            shootWeapon.OnChangeBulletLeft += UIManager.Instance.Packs[0].ChangeBulletLeftAmountDisplay;
        }
        else if (weapons[index] is GrenadeWeaponBase)
        {
            var grenadeWeapon = weapons[index] as GrenadeWeaponBase;
            pack.SetTextDisplayInforWeapon(grenadeWeapon.GrenadeAmmount.ToString());
        }
        else if (weapons[index] is MeleeWeaponBase)
        {
            pack.SetTextDisplayInforWeapon("");
        }
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
            int index = 0;
            PackWeaponUI pack = UIManager.Instance.Packs[index];
            pack.ChangeWeaponDisplay(weapon.ItemAttribute.Icon);
            pack.SetTextDisplayInforWeapon("");
            if (weapon is ShootWeaponBase)
            {
                var shootWeapon = weapon as ShootWeaponBase;
                pack.ChangeBulletDisplay(shootWeapon.BulletIcon);
                pack.ChangeBulletLeftAmountDisplay(shootWeapon.BulletMaxInMag, shootWeapon.BulletLeft);
                shootWeapon.OnChangeBulletLeft += UIManager.Instance.Packs[index].ChangeBulletLeftAmountDisplay;
                OnChangeCrossHair?.Invoke(weapon.ItemAttribute.Type);
            }
            else if (weapon is GrenadeWeaponBase)
            {
                var grenadeWeapon = weapons[index] as GrenadeWeaponBase;
                pack.SetTextDisplayInforWeapon(grenadeWeapon.GrenadeAmmount.ToString());
            }



        }
    }
    public void ThrowFPSWeapon(string nameWeapon)
    {
        currentWeaponIndex = 0;
        DoChangeWeapon();
        CmdSetCurrentWeaponIndex(currentWeaponIndex);
        ThrowWeapon(nameWeapon);
        CmdRemoveWeapon(nameWeapon, false);
    }
    private void ThrowWeapon(string nameWeapon, bool canThrowDefaultWeapon = false)
    {
        int index = 0;
        foreach (var weapon in weapons)
        {
            if (weapon.ItemAttribute.Name == nameWeapon && (!weapon.IsDefaultWeapon || canThrowDefaultWeapon))
            {
                weapons.Remove(weapon);
                OnRemoveWeapon?.Invoke(weapon);
                if (weapon is ShootWeaponBase)
                {
                    (weapon as ShootWeaponBase).OnChangeBulletLeft -= UIManager.Instance.Packs[index].ChangeBulletLeftAmountDisplay;
                }

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
            if (weapon is ShootWeaponBase)
            {
                (weapon as ShootWeaponBase).SetFullBulletLeft();
            }

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
                ThrowWeapon(nameWeapon, true);
                CmdRemoveWeapon(nameWeapon, true);
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
    private void RpcRemoveWeapon(string nameWeapon, bool canThrowDefaultWeapon)
    {
        if (isOwned) { return; }
        ThrowWeapon(nameWeapon, canThrowDefaultWeapon);
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
    private void CmdRemoveWeapon(string nameWeapon, bool canThrowDefaultWeapon)
    {
        RpcRemoveWeapon(nameWeapon, canThrowDefaultWeapon);
    }
    #endregion

}
