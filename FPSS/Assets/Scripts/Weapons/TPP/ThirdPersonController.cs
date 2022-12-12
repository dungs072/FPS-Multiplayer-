using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;

public class ThirdPersonController : NetworkBehaviour
{
    private const float CrossFadeFixedTime = 0.1f;
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int ForwardHash = Animator.StringToHash("Forward");
    private readonly int RightHash = Animator.StringToHash("Right");
    private readonly int FireHash = Animator.StringToHash("Fire");
    private readonly int ReloadHash = Animator.StringToHash("Reload");
    private readonly int InspectHash = Animator.StringToHash("IsInspecting");
    private readonly int OpenHash = Animator.StringToHash("ReloadOpen");
    private readonly int InsertHash = Animator.StringToHash("ReloadInsert");
    private readonly int CloseHash = Animator.StringToHash("ReloadClose");


    private readonly int IsHandgunHash = Animator.StringToHash("IsHandgun");
    private readonly int IsARHash = Animator.StringToHash("IsAR");
    private readonly int IsShotGunHash = Animator.StringToHash("IsShotGun");
    private readonly int IsSniperHash = Animator.StringToHash("IsSniper");
    private readonly int IsRocketHash = Animator.StringToHash("IsRocket");
    private readonly int IsSMGHash = Animator.StringToHash("IsSMG");

    private Dictionary<WeaponType, int> weaponHash = new Dictionary<WeaponType, int>();

    [SerializeField] private Animator Animator;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WeaponTPPManager weaponTPPManager;
    [SerializeField] private RigManager rigManager;
    [SerializeField] private RagdollManager ragdollManager;

    private WeaponTPP currentWeapon;

    [SyncVar(hook = nameof(OnChangeLocomotionValue))]
    private float locomotionValue = 0f;

    [SyncVar(hook = nameof(OnChangeRightValue))]
    private float rightValue = 0f;
    [SyncVar(hook = nameof(OnChangeForwardValue))]
    private float forwardValue = 0f;

    public WeaponTPP CurrentWeapon{get{return currentWeapon;}}
    public float RightValue
    {
        get { return rightValue; }
        set
        {
            if (value != rightValue)
            {
                Animator.SetFloat(RightHash, value);
                CmdSetRightValue(value);
            }
            rightValue = value;
        }
    }
    public float ForwardValue
    {
        get { return forwardValue; }
        set
        {
            if (value != forwardValue)
            {
                Animator.SetFloat(ForwardHash, value);
                CmdSetForwardValue(value);
            }
            forwardValue = value;
        }
    }
    public float LocomotionValue
    {
        get { return locomotionValue; }
        set
        {
            if (value != locomotionValue)
            {
                Animator.SetFloat(LocomotionHash, value);
                if(value>0)
                {
                    ragdollManager.ToggleFoot(false);
                }
                else
                {
                    ragdollManager.ToggleFoot(true);
                }
                CmdSetLocomotionValue(value);
            }
            locomotionValue = value;
        }
    }

    private void Start()
    {
        AddHashWeapon();
        weaponManager.OnChangeWeapon += ChangeWeapon;
        weaponManager.OnAddWeapon += SubcribeShootEvent;
        weaponManager.OnRemoveWeapon += UnSubscribeShootEvent;
        ChangeWeapon(0);
    }
    private void OnDestroy()
    {
        weaponManager.OnChangeWeapon -= ChangeWeapon;
        weaponManager.OnAddWeapon -= SubcribeShootEvent;
        weaponManager.OnRemoveWeapon -= UnSubscribeShootEvent;
    }
    private void SubcribeShootEvent(WeaponBase weapon)
    {
        weapon.OnShoot += DoShoot;
    }
    private void UnSubscribeShootEvent(WeaponBase weapon)
    {
        weapon.OnShoot -= DoShoot;
    }
    private void AddHashWeapon()
    {
        weaponHash.Add(WeaponType.HandGun, IsHandgunHash);
        weaponHash.Add(WeaponType.Assault, IsARHash);
        weaponHash.Add(WeaponType.ShotGun, IsShotGunHash);
        weaponHash.Add(WeaponType.Sniper, IsSniperHash);
        weaponHash.Add(WeaponType.RocketLaucher, IsRocketHash);
        weaponHash.Add(WeaponType.SMG,IsSMGHash);
    }
    public void ChangeWeapon(int index)
    {
        if (index == 0)
        {
            StartCoroutine(TakingOutWeapon(0f));
        }
        else
        {
            StartCoroutine(TakingOutWeapon(1f));
        }
        for (int i = 0; i < weaponManager.Weapons.Count; i++)
        {
            int hash = weaponHash[weaponManager.Weapons[i].WeaponType];
            if (i == index)
            {
                Animator.SetBool(hash, true);
                currentWeapon = weaponTPPManager.ToggleWeapon(i, true);
            }
            else
            {
                Animator.SetBool(hash, false);
                weaponTPPManager.ToggleWeapon(i, false);
            }
        }

    }
    private IEnumerator TakingOutWeapon(float value)
    {
        rigManager.SetSecondHandGrabWeight(0f);
        yield return new WaitForSeconds(weaponManager.CurrentWeapon.TakeOutTimeTPP);
        rigManager.SetSecondHandGrabWeight(value);

    }
    private void DoShoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time,int damage)
    {
        //Shoot();
        CmdShoot(targetPoint, spread, shootForce, time,damage);
    }
    private void Shoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time,int damage)
    {
        currentWeapon.ShootDirection(targetPoint, spread, shootForce, time,damage);
        Animator.SetTrigger(FireHash);

    }
    public void CheckInspect()
    {
        bool canInspect = weaponManager.CurrentWeapon.CanInspect;
        Inspect(canInspect);
        CmdInspect(canInspect);
    }
    public void Inspect(bool state)
    {
        Animator.SetBool(InspectHash, state);
        if (!state) { return; }
        TurnOffHandsWeight();

    }
    public void CheckReload()
    {
        if (weaponManager.CurrentWeapon.IsReloading) { return; }
        if (weaponManager.CurrentWeapon.IsComplexReload) { return; }
        Reload();
        CmdReload();
    }
    private void Reload()
    {
        Animator.SetTrigger(ReloadHash);
        TurnOffHandsWeight();
    }
    private void OpenReload()
    {
        TurnOffHandsWeight();
        Animator.CrossFadeInFixedTime(OpenHash, CrossFadeFixedTime);
    }
    private void InsertReload()
    {
        Animator.CrossFadeInFixedTime(InsertHash, CrossFadeFixedTime);
    }
    private void CloseReload()
    {
        Animator.CrossFadeInFixedTime(CloseHash, CrossFadeFixedTime);
    }
    private void TurnOffHandsWeight()
    {
        rigManager.SetHandWeight(0f);
        rigManager.SetSecondHandGrabWeight(0f);
    }
    public void DoOpenReload()
    {
        if (!isOwned) { return; }
        OpenReload();
        CmdDoOpenReload();
    }
    public void DoInsertReload()
    {
        if (!isOwned) { return; }
        InsertReload();
        CmdDoInsertReload();
    }
    public void DoCloseReload()
    {
        if (!isOwned) { return; }
        CloseReload();
        CmdDoCloseReload();
    }
    #region Server
    [Command]
    private void CmdShoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time,int damage)
    {
        RpcShoot(targetPoint, spread, shootForce, time,damage);
    }
    [Command]
    private void CmdInspect(bool state)
    {
        RpcInspect(state);
    }
    [Command]
    private void CmdReload()
    {
        RpcReload();
    }
    [Command]
    private void CmdSetLocomotionValue(float value)
    {
        locomotionValue = value;
    }
    [Command]
    private void CmdSetRightValue(float value)
    {
        rightValue = value;
    }
    [Command]
    private void CmdSetForwardValue(float value)
    {
        forwardValue = value;
    }
    [Command]
    private void CmdDoOpenReload()
    {
        RpcDoOpenReload();
    }
    [Command]
    private void CmdDoInsertReload()
    {
        RpcDoInsertReload();
    }
    [Command]
    private void CmdDoCloseReload()
    {
        RpcDoCloseReload();
    }
    #endregion
  
    #region Client
    [ClientRpc]
    private void RpcShoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time,int damage)
    {
        if (isOwned) { return; }
        Shoot(targetPoint, spread, shootForce, time,damage);
    }
    [ClientRpc]
    private void RpcInspect(bool state)
    {
        if (isOwned) { return; }
        Inspect(state);
    }
    [ClientRpc]
    private void RpcReload()
    {
        if (isOwned) { return; }
        Reload();
    }
    [ClientRpc]
    private void RpcDoOpenReload()
    {
        if (isOwned) { return; }
        OpenReload();
    }
    [ClientRpc]
    private void RpcDoInsertReload()
    {
        if (isOwned) { return; }
        InsertReload();
    }
    [ClientRpc]
    private void RpcDoCloseReload()
    {
        if (isOwned) { return; }
        CloseReload();
    }

    private void OnChangeLocomotionValue(float oldValue, float newValue)
    {
        if (isOwned) { return; }
        Animator.SetFloat(LocomotionHash, newValue);
        if(newValue>0)
        {
            ragdollManager.ToggleFoot(false);
        }
        else
        {
            ragdollManager.ToggleFoot(true);
        }
    }
    private void OnChangeForwardValue(float oldValue, float newValue)
    {
        if (isOwned) { return; }
        Animator.SetFloat(ForwardHash, newValue);
    }
    private void OnChangeRightValue(float oldValue, float newValue)
    {
        if (isOwned) { return; }
        Animator.SetFloat(RightHash, newValue);
    }
    #endregion
}
