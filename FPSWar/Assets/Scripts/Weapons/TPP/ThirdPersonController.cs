using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;

public class ThirdPersonController : NetworkBehaviour
{
    private const float CrossFadeFixedTime = 0.1f;
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int CrouchDownHash = Animator.StringToHash("CrouchDown");
    private readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private readonly int LeftTurnHash = Animator.StringToHash("Left Turn");
    private readonly int RightTurnHash = Animator.StringToHash("Right Turn");
    private readonly int CrouchUpHash = Animator.StringToHash("CrouchUp");
    private readonly int ForwardHash = Animator.StringToHash("Forward");
    private readonly int RightHash = Animator.StringToHash("Right");
    private readonly int FireHash = Animator.StringToHash("Fire");
    private readonly int ReloadHash = Animator.StringToHash("Reload");
    private readonly int InspectHash = Animator.StringToHash("IsInspecting");
    private readonly int OpenHash = Animator.StringToHash("ReloadOpen");
    private readonly int InsertHash = Animator.StringToHash("ReloadInsert");
    private readonly int CloseHash = Animator.StringToHash("ReloadClose");

    private readonly int OpenSnipeHash = Animator.StringToHash("ReloadOpen 0");
    private readonly int InsertSnipeHash = Animator.StringToHash("ReloadInsert 0");
    private readonly int CloseSnipeHash = Animator.StringToHash("ReloadClose 0");

    private readonly int ReadyThrowHash = Animator.StringToHash("Ready_Throw");
    private readonly int ThrowHash = Animator.StringToHash("Throw");

    private readonly int IsHandgunHash = Animator.StringToHash("IsHandgun");
    private readonly int IsARHash = Animator.StringToHash("IsAR");
    private readonly int IsShotGunHash = Animator.StringToHash("IsShotGun");
    private readonly int IsSniperHash = Animator.StringToHash("IsSniper");
    private readonly int IsRocketHash = Animator.StringToHash("IsRocket");
    private readonly int IsSMGHash = Animator.StringToHash("IsSMG");
    private readonly int IsMeleeHash = Animator.StringToHash("IsMelee");
    private readonly int IsGrenadeHash = Animator.StringToHash("IsGrenade");

    private Dictionary<ItemType, int> weaponHash = new Dictionary<ItemType, int>();

    [SerializeField] private Animator Animator;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WeaponTPPManager weaponTPPManager;
    [SerializeField] private RigManager rigManager;
    [SerializeField] private RagdollManager ragdollManager;
    [SerializeField] private PlayerSound playerSound;

    // [SerializeField] private ReferenceManager referenceManager;

    private WeaponTPP currentWeapon;

    [SyncVar(hook = nameof(OnChangeLocomotionValue))]
    private float locomotionValue = 0f;

    [SyncVar(hook = nameof(OnChangeRightValue))]
    private float rightValue = 0f;
    [SyncVar(hook = nameof(OnChangeForwardValue))]
    private float forwardValue = 0f;

    [SyncVar(hook = nameof(OnChangeIsMoving))]
    private bool isMoving = false;

    private Coroutine takeOutWeaponCoroutine;
    public WeaponTPP CurrentWeapon { get { return currentWeapon; } }
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
                if (value > 0)
                {
                    ragdollManager.ToggleFoot(false);
                }
                else
                {
                    ragdollManager.ToggleFoot(true);
                }
                SetIsRunningAnimation(value == 1f);
                CmdSetLocomotionValue(value);

                if (currentWeapon.IsMelee||currentWeapon.IsGrenade)
                {
                    rigManager.SetHandWeight(0f);
                    rigManager.SetSecondHandGrabWeight(0f);
                }
                else
                {
                    rigManager.SetHandWeight(1f);
                    rigManager.SetSecondHandGrabWeight(1f);
                }
            }
            locomotionValue = value;

        }
    }

    private void Start()
    {
        AddHashWeapon();
        weaponManager.OnChangeWeapon += ChangeWeapon;
        weaponManager.OnAddWeapon += SubscribeWeaponEvent;
        weaponManager.OnRemoveWeapon += UnSubscribeWeaponEvent;
        weaponTPPManager.OnRemoveWeapon += TurnOffOldHashWeapon;
        ChangeWeapon(0);
        if(isOwned)
        {
            GetComponent<HealthManager>().OnDie+=ToggleRagdollOn;
            GetComponent<RespawnManager>().OnRespawn+=ToggleRagdollOff;
            ToggleRagdollOff();
        }
    }
    private void ToggleRagdollOn()
    {
        ragdollManager.ToggleColliders(true);
    }
    private void ToggleRagdollOff()
    {
        ragdollManager.ToggleColliders(false);
    }

    private void OnDestroy()
    {
        weaponManager.OnChangeWeapon -= ChangeWeapon;
        weaponManager.OnAddWeapon -= SubscribeWeaponEvent;
        weaponManager.OnRemoveWeapon -= UnSubscribeWeaponEvent;
        weaponTPPManager.OnRemoveWeapon -= TurnOffOldHashWeapon;
        if(isOwned)
        {
            GetComponent<HealthManager>().OnDie-=ToggleRagdollOn;
            GetComponent<RespawnManager>().OnRespawn-=ToggleRagdollOff;
            
        }
    }
    private void SubscribeWeaponEvent(WeaponBase weapon)
    {
        if (weapon is ShootWeaponBase)
        {
            (weapon as ShootWeaponBase).OnShoot += DoShoot;
        }
        if (weapon is MeleeWeaponBase)
        {
            (weapon as MeleeWeaponBase).OnAttack += DoAttack;
        }
        if (weapon is GrenadeWeaponBase)
        {
            (weapon as GrenadeWeaponBase).OnReadyThrow += DoReadyThrow;
            (weapon as GrenadeWeaponBase).OnThrow += DoThrow;
        }

    }
    private void UnSubscribeWeaponEvent(WeaponBase weapon)
    {
        if (weapon is ShootWeaponBase)
        {
            (weapon as ShootWeaponBase).OnShoot -= DoShoot;
        }
        if (weapon is MeleeWeaponBase)
        {
            (weapon as MeleeWeaponBase).OnAttack -= DoAttack;
        }
        if (weapon is GrenadeWeaponBase)
        {
            (weapon as GrenadeWeaponBase).OnReadyThrow -= DoReadyThrow;
            (weapon as GrenadeWeaponBase).OnThrow -= DoThrow;
        }

    }
    private void AddHashWeapon()
    {
        weaponHash.Add(ItemType.HandGun, IsHandgunHash);
        weaponHash.Add(ItemType.Assault, IsARHash);
        weaponHash.Add(ItemType.ShotGun, IsShotGunHash);
        weaponHash.Add(ItemType.Sniper, IsSniperHash);
        weaponHash.Add(ItemType.RocketLaucher, IsRocketHash);
        weaponHash.Add(ItemType.SMG, IsSMGHash);
        weaponHash.Add(ItemType.Melee, IsMeleeHash);
        weaponHash.Add(ItemType.Grenade, IsGrenadeHash);
    }
    private void SetIsMovingAnimation(bool isMoving)
    {
        Animator.SetBool(IsMovingHash, isMoving);
    }
    private void SetIsRunningAnimation(bool isRunning)
    {
        rigManager.SetRigWeightOnlyLocalPlayer(isRunning ? 0f : 1f);
        Animator.SetBool(IsRunningHash, isRunning);
    }
    public void ChangeWeapon(int index)
    {
        if (takeOutWeaponCoroutine != null) { StopCoroutine(takeOutWeaponCoroutine); }
        if (index == 0)
        {
            takeOutWeaponCoroutine = StartCoroutine(TakingOutWeapon(0f));
        }
        else
        {
            takeOutWeaponCoroutine = StartCoroutine(TakingOutWeapon(1f));
        }
        int currentHash = weaponHash[weaponManager.Weapons[index].ItemAttribute.Type];
        for (int i = 0; i < weaponManager.Weapons.Count; i++)
        {
            int hash = weaponHash[weaponManager.Weapons[i].ItemAttribute.Type];
            if (i == index)
            {
                Animator.SetBool(hash, true);
                currentWeapon = weaponTPPManager.ToggleWeapon(i, true);
                if (currentWeapon.IsMelee||currentWeapon.IsGrenade)
                {
                    rigManager.SetHandWeight(0f);
                    rigManager.SetSecondHandGrabWeight(0f);
                    //rigManager.SetHandWeight(0f);
                }
            }
            else
            {
                if(currentHash!=hash)
                {
                    Animator.SetBool(hash, false);
                }
                weaponTPPManager.ToggleWeapon(i, false);
            }
        }

    }
    private void ToggleCrouch(bool isCrouch)
    {
        if (isCrouch)
        {
            Animator.CrossFadeInFixedTime(CrouchDownHash, CrossFadeFixedTime);
        }
        else
        {
            Animator.CrossFadeInFixedTime(CrouchUpHash, CrossFadeFixedTime);
        }
    }
    private void TurnOffOldHashWeapon(ItemType type)
    {
        int hash = weaponHash[type];
        Animator.SetBool(hash, false);
    }
    private IEnumerator TakingOutWeapon(float value)
    {
        rigManager.SetSecondHandGrabWeight(0f);
        yield return new WaitForSeconds(weaponManager.CurrentWeapon.TakeOutTimeTPP);
        if (currentWeapon.IsMelee||CurrentWeapon.IsGrenade)
        {
            value = 0f;
        }
        rigManager.SetSecondHandGrabWeight(value);

    }
    // shoot
    private void DoShoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time, int damage)
    {
        //Shoot();
        CmdShoot(targetPoint, spread, shootForce, time, damage);
    }
    private void Shoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time, int damage)
    {
        currentWeapon.ShootDirection(targetPoint, spread, shootForce, time, damage);
        Animator.SetTrigger(FireHash);

    }
    // melee
    private void DoAttack()
    {
        Attack();
        CmdAttack();
    }
    private void Attack()
    {
        Animator.SetTrigger(FireHash);
    }
    // grenade

    private void DoReadyThrow()
    {
        ReadyThrow();
        CmdReadyThrow();
    }
    private void ReadyThrow()
    {
        Animator.CrossFadeInFixedTime(ReadyThrowHash, CrossFadeFixedTime);
    }
    private void IdleAnimation()
    {
        Animator.CrossFadeInFixedTime(IdleHash, CrossFadeFixedTime);
    }
    private void DoThrow()
    {
        Throw();
        CmdThrow();
    }
    private void Throw()
    {
        Animator.CrossFadeInFixedTime(ThrowHash, CrossFadeFixedTime);
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
        if (!weaponManager.CanTriggerReloadInTPP()) { return; }
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
        if (currentWeapon.ItemAttribute.Type == ItemType.Sniper)
        {
            Animator.CrossFadeInFixedTime(OpenSnipeHash, CrossFadeFixedTime);
        }
        else
        {
            Animator.CrossFadeInFixedTime(OpenHash, CrossFadeFixedTime);
        }

    }
    private void InsertReload()
    {
        if (currentWeapon.ItemAttribute.Type == ItemType.Sniper)
        {
            Animator.CrossFadeInFixedTime(InsertSnipeHash, CrossFadeFixedTime);
        }
        else
        {
            Animator.CrossFadeInFixedTime(InsertHash, CrossFadeFixedTime);
        }
    }
    private void CloseReload()
    {
        if (currentWeapon.ItemAttribute.Type == ItemType.Sniper)
        {
            Animator.CrossFadeInFixedTime(CloseSnipeHash, CrossFadeFixedTime);
        }
        else
        {
            Animator.CrossFadeInFixedTime(CloseHash, CrossFadeFixedTime);
        }
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
    private void CmdShoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time, int damage)
    {
        RpcShoot(targetPoint, spread, shootForce, time, damage);
    }
    [Command]
    private void CmdAttack()
    {
        RpcAttack();
    }
    [Command]
    private void CmdReadyThrow()
    {
        RpcReadyThrow();
    }
    [Command]
    private void CmdThrow()
    {
        RpcThrow();
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
    [Command]
    public void CmdToggleCrouch(bool isCrouch)
    {
        RpcToggleCrouch(isCrouch);
    }
    [Command]
    public void CmdSetIsMovingAnimation(bool isMoving)
    {
        this.isMoving = isMoving;
    }
    [Command]
    public void CmdSetIdleAnimation()
    {
        RpcIdleAnimation();
    }
    #endregion

    #region Client

    [ClientRpc]
    private void RpcToggleCrouch(bool isCrouch)
    {
        ToggleCrouch(isCrouch);
    }
    [ClientRpc]
    private void RpcShoot(Vector3 targetPoint, Vector3 spread, int shootForce, float time, int damage)
    {
        if (isOwned) { return; }
        Shoot(targetPoint, spread, shootForce, time, damage);
    }
    [ClientRpc]
    private void RpcAttack()
    {
        if (isOwned) { return; }
        Attack();
    }
    [ClientRpc]
    private void RpcReadyThrow()
    {
        if (isOwned) { return; }
        ReadyThrow();
    }
    [ClientRpc]
    private void RpcThrow()
    {
        if (isOwned) { return; }
        Throw();
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
        playerSound.PlayReloading();
        Reload();
    }
    [ClientRpc]
    private void RpcDoOpenReload()
    {
        if (isOwned) { return; }
        playerSound.PlayReloading();
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
    [ClientRpc]
    private void RpcIdleAnimation()
    {
        IdleAnimation();
    }
    private void OnChangeIsMoving(bool oldState, bool newState)
    {
        SetIsMovingAnimation(newState);
    }

    private void OnChangeLocomotionValue(float oldValue, float newValue)
    {
        if (isOwned) { return; }
        Animator.SetFloat(LocomotionHash, newValue);
        SetIsRunningAnimation(newValue == 1f);
        if (newValue > 0)
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
