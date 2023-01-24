using UnityEngine;
using Mirror;
using System;
public class PlayerController : NetworkBehaviour
{
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action<bool> OnIsReadyInLobbyUpdated;
    public event Action OnHitTarget;
    [SerializeField] private ReferenceManager referManager;
    [SerializeField] private GameObject fpsModel;
    [SerializeField] private AudioListener audioListener;
    [field: SerializeField] public PlayerSound PlayerSound { get; private set; }
    [SyncVar(hook = nameof(OnChangeWalkingState))]
    private bool isWalking;

    [SyncVar(hook = nameof(OnChangeRunningState))]
    private bool isRunning;

    [SyncVar(hook = nameof(OnChangeIdleState))]
    private bool isIdle;
    [SyncVar(hook = nameof(OnChangeAimState))]
    private bool isAiming;

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;
    [SyncVar(hook = nameof(AuthorityHandleReadyInLobbyStateUpdated))]
    private bool isReadyInLoby = true;
    private bool canInspect = false;
    private bool isCrouch = false;
    private bool isPause = false;
    

    public bool GetReadyInLobby(){return isReadyInLoby;}
    public bool GetIsPartyOwner() { return isPartyOwner; }
    public bool IsInLobby { get; set; } = false;
    public bool IsWalking
    {
        get
        {
            return isWalking;
        }
        set
        {
            if (value != isWalking && value)
            {
                DoWalk(value);
            }
            else if (value != isWalking && !value)
            {
                if (isOwned)
                {
                    CmdCanWalk(value);
                }

            }
            isWalking = value;
        }
    }
    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            if (value != isRunning && value)
            {
                DoRun(value);
            }
            else if (value != isRunning && !value)
            {
                if (isOwned)
                {
                    CmdCanRun(value);
                }

            }
            isRunning = value;
        }
    }
    public bool IsIdle
    {
        get { return isIdle; }
        set
        {
            if (value != isIdle && value)
            {
                DoIdle(value);
                referManager.TPPController.CmdSetIsMovingAnimation(!value);
            }
            else if (value != isIdle && !value)
            {
                if (isOwned)
                {
                    referManager.TPPController.CmdSetIsMovingAnimation(!value);
                    CmdCanIdle(value);
                }

            }
            isIdle = value;
        }
    }
    public bool IsAiming
    {
        get
        {
            return isAiming;
        }
        set
        {
            if (isAiming != value)
            {
                if (value)
                {
                    DoAimIn(value);
                }
                else
                {
                    DoAimOut(value);
                }
                UIManager.Instance.ToggleDynamicCrossHair(value);
            }
            isAiming = value;
            CmdAim(value);
        }
    }

    public bool IsAttacking { get; set; }
    public override void OnStartAuthority()
    {
        referManager.HealthManager.OnNearlyDie += referManager.FPSController.HandleWound;
        referManager.HealthManager.OnNormal += referManager.FPSController.HandleNormal;
        DISystem.Instance.SetMainCamera(referManager.FPSController.FirstPersonCamera);
        DISystem.Instance.SetPlayerTransform(transform);
        MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        myNetworkManager.AddPlayers(this);
        OnHitTarget += UIManager.Instance.ToggleHitCrossHair;
    }
    private void Start()
    {
        referManager.HealthManager.OnDie += OnDie;
    }
    private void OnDestroy()
    {
        referManager.HealthManager.OnDie -= OnDie;
        if (!isOwned) { return; }
        OnHitTarget -= UIManager.Instance.ToggleHitCrossHair;
        referManager.HealthManager.OnNearlyDie -= referManager.FPSController.HandleWound;
        referManager.HealthManager.OnNormal -= referManager.FPSController.HandleNormal;
    }
    private void OnDie()
    {
        referManager.FPSController.enabled = false;
        referManager.TPPController.enabled = false;
        if (isOwned)
        {
            referManager.NetworkPlayerManager.ToggleMeshRenderer(true);
        }
        this.enabled = false;

    }
    public void OnOutLobbyInGame()
    {
        referManager.FPSController.enabled = true;
        referManager.TPPController.enabled = true;
        if (isOwned)
        {
            referManager.NetworkPlayerManager.ToggleMeshRenderer(false);
        }
        this.enabled = true;
    }
    private void Update()
    {
        if (!isOwned) { return; }
        if (IsInLobby) { return; }
        HandleUI();
        if(isPause){return;}
        HandleFPSControl();
        HandleTPPMovement();
        
    }
    private void HandleUI()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            UIManager.Instance.TogglePauseMenu(isPause);
            referManager.FPSController.SetCursorLock(!isPause);
            referManager.FPSController.enabled = !isPause;
        }
    }

    private void HandleFPSControl()
    {
        WeaponBase currentWeapon = referManager.WeaponManager.CurrentWeapon;
        if (currentWeapon.IsTakingOut) { return; }

        bool isReloading = currentWeapon.IsReloading;
        bool isShooting = !currentWeapon.CanShoot;
        bool isThrowing = currentWeapon.IsThrowing;
        HandleAttack();
        if (currentWeapon.CanDelayInShoot)
        {
            if (!isReloading && !isShooting && !isThrowing)
            {
                HandleFPSMovement();
                HandleAim();
            }
            else
            {
                ResetMovementState();
            }
        }
        else
        {
            if (!isReloading && !isThrowing)
            {
                HandleFPSMovement();
                HandleAim();
            }
            else
            {
                ResetMovementState();
            }
        }

        HandleReload();
        HandleThrowGrenade();
        HandleCrouch();
        HandleLean();
    }
    public void ResetMovementState()
    {
        IsIdle = false;
        IsWalking = false;
        IsRunning = false;
    }
    private void HandleLean()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            referManager.LeanManager.LeanLeft();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            referManager.LeanManager.LeanRight();
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouch = !isCrouch;
            UIManager.Instance.ChangePosture(isCrouch);
            referManager.TPPController.CmdToggleCrouch(isCrouch);
            referManager.FPSController.HandleCrouch(isCrouch);
        }
    }
    private void HandleThrowGrenade()
    {
        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     referManager.WeaponManager.CurrentWeapon.CheckReadyThrowGrenade();
        // }
        // if (Input.GetKeyUp(KeyCode.G))
        // {
        //     referManager.WeaponManager.CurrentWeapon.CheckThrowGrenade();
        // }
    }
    private void HandleFPSMovement()
    {
        IsWalking = referManager.FPSController.IsWalking && !IsAiming;
        IsRunning = referManager.FPSController.IsRunning;
        IsIdle = !IsRunning && !IsWalking && !IsAiming;

    }
    private void HandleTPPMovement()
    {
        referManager.TPPController.RightValue = referManager.FPSController.RightValue;
        referManager.TPPController.ForwardValue = referManager.FPSController.ForwardValue;
        if (referManager.FPSController.IsWalking)
        {
            referManager.TPPController.LocomotionValue = 0.5f;
        }
        else if (referManager.FPSController.IsRunning)
        {
            referManager.TPPController.LocomotionValue = 1f;
        }
        else
        {
            referManager.TPPController.LocomotionValue = 0f;
        }
    }
    private void HandleAttack()
    {
        if (IsRunning) { return; }
        WeaponBase weapon = referManager.WeaponManager.CurrentWeapon;
        bool isAttacking = false;
        if (Input.GetMouseButtonDown(0))
        {
            if (weapon.ShootType == ShootType.Single)
            {
                weapon.CheckShoot(IsAiming);
                isAttacking = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (weapon.ShootType == ShootType.Continuous)
            {
                weapon.CheckShoot(IsAiming);
                isAttacking = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (weapon.ShootType == ShootType.Continuous || !weapon.CanDelayInShoot)
            {
                ResetMovementState();
            }
        }
        IsAttacking = !weapon.CanShoot;
        canInspect = !isAttacking && !IsRunning && !IsAiming && !IsWalking;
        //HandleInspect(weapon);
    }
    public bool CurrentWeaponIsSingleFire()
    {
        return referManager.WeaponManager.CurrentWeapon.ShootType == ShootType.Single;
    }
    private void HandleInspect(WeaponBase weapon)
    {
        referManager.TPPController.CheckInspect();
        weapon.CheckInspect(Time.deltaTime, canInspect);
    }
    private void HandleAim()
    {
        if (IsRunning) { return; }
        IsAiming = Input.GetMouseButton(1);
    }
    private void HandleReload()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            DoReload();
        }

    }
    private void DoWalk(bool state)
    {
        referManager.WeaponManager.CurrentWeapon.WalkAnimation();
        CmdCanWalk(state);
    }
    private void DoRun(bool state)
    {
        referManager.WeaponManager.CurrentWeapon.RunAnimation();
        CmdCanRun(state);
    }
    private void DoIdle(bool state)
    {
        referManager.WeaponManager.CurrentWeapon.IdleAnimation();

        CmdCanIdle(state);
    }
    private void DoAimIn(bool state)
    {
        referManager.WeaponManager.CurrentWeapon.CheckAimIn();
    }
    private void DoAimOut(bool state)
    {
        ResetMovementState();
        referManager.WeaponManager.CurrentWeapon.DoAimOut();
    }
    private void DoReload()
    {
        if (referManager.WeaponManager.CurrentWeapon.IsFullBulletInMag()) { return; }
        if (referManager.WeaponManager.CurrentWeapon.BulletLeft == 0) { return; }
        referManager.TPPController.CheckReload();
        referManager.WeaponManager.CurrentWeapon.CheckReload();
        CmdReload();
    }
    public void TriggerHitCrossHair()
    {
        if (!isOwned) { return; }
        OnHitTarget?.Invoke();
    }
    #region Client
    private void OnChangeRunningState(bool oldState, bool newState)
    {
        if (isOwned) { return; }
        if (!newState) { return; }
        referManager.WeaponManager.CurrentWeapon.RunAnimation();

    }
    private void OnChangeWalkingState(bool oldState, bool newState)
    {
        if (isOwned) { return; }
        if (!newState) { return; }
        referManager.WeaponManager.CurrentWeapon.WalkAnimation();
    }
    private void OnChangeIdleState(bool oldState, bool newState)
    {
        if (isOwned) { return; }
        if (!newState) { return; }
        referManager.WeaponManager.CurrentWeapon.IdleAnimation();
    }
    private void OnChangeAimState(bool oldState, bool newState)
    {
        if (isOwned) { return; }
        if (newState)
        {
            referManager.WeaponManager.CurrentWeapon.CheckAimIn();
        }
        else
        {
            referManager.WeaponManager.CurrentWeapon.DoAimOut();
        }
    }

    [ClientRpc]
    private void RpcReload()
    {
        if (isOwned) { return; }
        referManager.WeaponManager.CurrentWeapon.CheckReload();
    }
    public override void OnStartClient()//fixed there
    {
        if (isOwned)
        {
            SetInGameProgress(false);
        }
        if (NetworkServer.active) { return; }
        DontDestroyOnLoad(gameObject);
        ((MyNetworkManager)NetworkManager.singleton).Players.Add(this);
    }
    public void SetInGameProgress(bool state)
    {
        referManager.FPSController.SetCursorLock(state);
        referManager.FPSController.enabled = state;
        referManager.NetworkPlayerManager.ChangeShadowOnlyMesh(state);
        fpsModel.SetActive(state);
        IsInLobby = !state;
        UIManager.Instance.ToggleParentUI(state);
        //audioListener.enabled = state;
    }
    public override void OnStopClient()
    {
        if (!isClientOnly) { return; }
        ((MyNetworkManager)NetworkManager.singleton).Players.Remove(this);
    }
    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!isOwned) { return; }
        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }
    private void AuthorityHandleReadyInLobbyStateUpdated(bool oldState, bool newState)
    {
        OnIsReadyInLobbyUpdated?.Invoke(newState);
    }
    [ClientRpc]
    private void RpcInGameProgress()
    {
        if (!isOwned) { return; }
        SetInGameProgress(true);
    }
    #endregion

    #region Server
    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }
    [Command]
    private void CmdCanRun(bool state)
    {
        isRunning = state;
    }
    [Command]
    private void CmdCanWalk(bool state)
    {
        isWalking = state;
    }
    [Command]
    private void CmdCanIdle(bool state)
    {
        isIdle = state;
    }
    [Command]
    private void CmdAim(bool state)
    {
        isAiming = state;
    }
    [Command]
    private void CmdReload()
    {
        RpcReload();
    }
    [Command]
    public void CmdStartGame()
    {
        if (!isPartyOwner) { return; }
        ((MyNetworkManager)NetworkManager.singleton).StartGame();
    }
    [Server]
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state;
    }
    [Command]
    public void CmdSetReadyInLobby()
    {
        isReadyInLoby = !isReadyInLoby;
    }
    [Server]
    public void SetIsInGameProgress()
    {
        RpcInGameProgress();
        SetInGameProgress(true);
    }
    #endregion


}
