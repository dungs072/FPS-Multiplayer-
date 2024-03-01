using UnityEngine;
using Mirror;
using System;
using InfimaGames.LowPolyShooterPack;
public class PlayerController : NetworkBehaviour
{
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action<bool> OnIsReadyInLobbyUpdated;
    public static event Action<int> OnUpdateMaxDeaths;
    public static event Action<int> OnUpdateCurrentRuleIndex;
    public static event Action<bool> OnMatchToRob;
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
    private bool isForceToStopRunning = false;

    private CustomInputManager InputManager;

    private bool previousCanRobState = false;

    public bool GetReadyInLobby() { return isReadyInLoby; }
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
    public bool CanRob { get; set; } = false;
    public override void OnStartAuthority()
    {
        referManager.HealthManager.OnNearlyDie += referManager.FPSController.HandleWound;
        referManager.HealthManager.OnNormal += referManager.FPSController.HandleNormal;
        DISystem.Instance.SetMainCamera(referManager.FPSController.FirstPersonCamera);
        DISystem.Instance.SetPlayerTransform(transform);
        CrossHair.FindPlayer(this);
        MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        myNetworkManager.AddPlayers(this);
        OnHitTarget += UIManager.Instance.ToggleHitCrossHair;
        InputManager = CustomInputManager.Instance;

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
            TurnOffCrouch();
            TurnOffLean();
            UIManager.Instance.ToggleFButtonUI(false);
            UIManager.Instance.ToggleFButtonUIRobberManager(false,false);
            referManager.TPPController.CmdSetIdleAnimation();
            referManager.WeaponManager.ChangeWeaponIndex(4);
            SetForceToStopRunning(false);
        }
        this.enabled = false;

    }
    private void TurnOffLean()
    {
        referManager.LeanManager.StandStraight();
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
        HandleEnergy();
        if (isPause) { return; }
        HandleFPSControl();
        HandleTPPMovement();

    }
    private void HandleEnergy()
    {
        if (referManager.FPSController.IsRunning)
        {
            referManager.EnergyManager.DecreaseEnergy(Time.deltaTime);
        }
        if (Input.GetKeyUp(InputManager.Input.RunningKeyCode))
        {
            referManager.EnergyManager.StartIncreaseEnergy();
        }
        CanRun(referManager.EnergyManager.CurrentEnergy > 0 && !isForceToStopRunning);
    }
    private void HandleUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            PauseGame(isPause);
        }
    }
    public void PauseGame(bool state)
    {
        UIManager.Instance.TogglePauseMenu(state);
        ToggleControll(state);
    }
    public void ToggleControll(bool state)
    {
        referManager.FPSController.enabled = !state;
        referManager.FPSController.SetCursorLock(!state);
        if (!state) { return; }
        if (referManager.WeaponManager.CurrentWeapon is ShootWeaponBase)
        {
            if (!referManager.WeaponManager.CurrentShootWeapon.IsReloading)
            {
                IsIdle = true;
            }
        }
        else if (referManager.WeaponManager.CurrentWeapon.CanAttack)
        {
            IsIdle = true;
        }

    }
    public void CanRun(bool state)
    {
        referManager.FPSController.CanRun(state);
    }
    public void SetForceToStopRunning(bool state)
    {
        isForceToStopRunning = state;
    }

    private void HandleFPSControl()
    {
        if (referManager.WeaponManager.CurrentWeapon is ShootWeaponBase)
        {
            ShootWeaponBase currentWeapon = referManager.WeaponManager.CurrentShootWeapon;
            if (currentWeapon.IsTakingOut) { return; }

            bool isReloading = currentWeapon.IsReloading;
            bool isShooting = !currentWeapon.CanAttack;
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
        }
        else if (referManager.WeaponManager.CurrentWeapon is MeleeWeaponBase)
        {
            var currentWeapon = referManager.WeaponManager.CurrentMeleeWeapon;
            HandleAttack();
            bool isAttacking = !currentWeapon.CanAttack;
            if (!isAttacking)
            {
                HandleFPSMovement();
            }
            else
            {
                ResetMovementState();
            }
            if (Input.GetMouseButton(1))
            {
                var currentMeleeWeapon = referManager.WeaponManager.CurrentMeleeWeapon;
                currentMeleeWeapon.Block();
            }
            if (Input.GetMouseButtonUp(1))
            {
                var currentMeleeWeapon = referManager.WeaponManager.CurrentMeleeWeapon;
                currentMeleeWeapon.CancelBlock();
            }
        }
        else if (referManager.WeaponManager.CurrentWeapon is GrenadeWeaponBase)
        {
            var currentWeapon = referManager.WeaponManager.CurrentWeapon;
            HandleAttack();
            bool isAttacking = !currentWeapon.CanAttack;
            if (!isAttacking)
            {
                HandleFPSMovement();
            }
            else
            {
                ResetMovementState();
            }

        }
        HandleRob();
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
        if (Input.GetKey(InputManager.Input.LeaningLeftKeyCode))
        {
            referManager.LeanManager.LeanLeft();
        }
        else if (Input.GetKey(InputManager.Input.LeaningRightKeyCode))
        {
            referManager.LeanManager.LeanRight();
        }
        else if (Input.GetKeyUp(InputManager.Input.LeaningLeftKeyCode) ||
                Input.GetKeyUp(InputManager.Input.LeaningRightKeyCode))
        {
            referManager.LeanManager.StandStraight();
        }

    }
    private void HandleRob()
    {
        if (!CanRob)
        {
            ChangeIsMatchState(false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            var team = GetComponent<Team>();
            if (team.TeamName == TeamName.Terrorist)
            {
                ChangeIsMatchState(true);
            }
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            var team = GetComponent<Team>();
            if (team.TeamName == TeamName.Terrorist)
            {
                ChangeIsMatchState(false);
            }
        }

    }
    private void ChangeIsMatchState(bool state)
    {
        if (previousCanRobState == state) { return; }
        CmdChangeIsMatch(state);
        previousCanRobState = state;
    }
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(InputManager.Input.CrouchKeyCode))
        {
            isCrouch = !isCrouch;
            referManager.FPSController.CanJump(!isCrouch);
            UIManager.Instance.ChangePosture(isCrouch);
            referManager.TPPController.CmdToggleCrouch(isCrouch);
            referManager.FPSController.HandleCrouch(isCrouch);
        }
    }
    private void TurnOffCrouch()
    {
        if(!isCrouch){return;}
        isCrouch = false;
        referManager.FPSController.CanJump(!isCrouch);
        UIManager.Instance.ChangePosture(isCrouch);
        referManager.TPPController.CmdToggleCrouch(isCrouch);
        referManager.FPSController.HandleCrouch(isCrouch);
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
        WeaponBase currentWeapon = referManager.WeaponManager.CurrentWeapon;
        bool isAttacking = false;
        if (currentWeapon is ShootWeaponBase)
        {
            if (IsRunning) { return; }
            var weapon = referManager.WeaponManager.CurrentShootWeapon;
            if (Input.GetMouseButtonDown(0))
            {
                if (weapon.ShootType == ShootType.Single)
                {
                    weapon.CheckAttack(IsAiming);
                    isAttacking = true;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (weapon.ShootType == ShootType.Continuous)
                {
                    weapon.CheckAttack(IsAiming);
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
            IsAttacking = !weapon.CanAttack;
        }
        if (currentWeapon is MeleeWeaponBase)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentWeapon.CheckAttack(IsAiming);
                isAttacking = true;
            }

        }
        if (currentWeapon is GrenadeWeaponBase)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentWeapon.CheckAttack(IsAiming);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var grenadeWeapon = currentWeapon as GrenadeWeaponBase;
                grenadeWeapon.ThrowGrenade();
            }
        }

        canInspect = !isAttacking && !IsRunning && !IsAiming && !IsWalking;
        //HandleInspect(weapon);
    }
    public bool CurrentWeaponIsSingleFire()
    {
        if (referManager.WeaponManager.CurrentWeapon is not ShootWeaponBase)
        {
            return true;
        }
        return referManager.WeaponManager.CurrentShootWeapon.ShootType == ShootType.Single;
    }
    private void HandleInspect(ShootWeaponBase weapon)
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

        if (Input.GetKeyDown(InputManager.Input.ReloadingKeyCode))
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
        referManager.WeaponManager.CurrentShootWeapon.CheckAimIn();
    }
    private void DoAimOut(bool state)
    {
        referManager.WeaponManager.CurrentShootWeapon.DoAimOut();
    }
    private void DoReload()
    {
        if (referManager.WeaponManager.CurrentShootWeapon.IsFullBulletInMag()) { return; }
        if (referManager.WeaponManager.CurrentShootWeapon.BulletLeft == 0) { return; }
        referManager.TPPController.CheckReload();
        referManager.WeaponManager.CurrentShootWeapon.CheckReload();
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
            referManager.WeaponManager.CurrentShootWeapon.CheckAimIn();
        }
        else
        {
            referManager.WeaponManager.CurrentShootWeapon.DoAimOut();
        }
    }

    [ClientRpc]
    private void RpcReload()
    {
        if (isOwned) { return; }
        referManager.WeaponManager.CurrentShootWeapon.CheckReload();
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
    public void HandleResultInMatch()
    {
        referManager.FPSController.SetCursorLock(false);
        referManager.FPSController.enabled = false;
        IsInLobby = true;
    }
    public void SetCursorLock(bool state)
    {
        referManager.FPSController.SetCursorLock(state);
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
    [ClientRpc]
    private void RpcChangeChangeMaxDeaths(int maxDeathsIndex)
    {
        OnUpdateMaxDeaths?.Invoke(maxDeathsIndex);
    }
    [ClientRpc]
    private void RpcChanceCurrentRuleIndex(int index)
    {
        OnUpdateCurrentRuleIndex?.Invoke(index);
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
    public void CmdStartGame(string mapName)
    {
        if (!isPartyOwner) { return; }
        ((MyNetworkManager)NetworkManager.singleton).StartGame(mapName);
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
    [Command]
    public void CmdChangeMaxDeaths(int maxDeathsIndex)
    {
        RpcChangeChangeMaxDeaths(maxDeathsIndex);
    }
    [Command]
    public void CmdChangeCurrentRuleIndex(int index)
    {
        RpcChanceCurrentRuleIndex(index);
    }
    [Command]
    public void CmdChangeIsMatch(bool state)
    {
        OnMatchToRob?.Invoke(state);
    }

    #endregion


}
