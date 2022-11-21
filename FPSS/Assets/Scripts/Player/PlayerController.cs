using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private ReferenceManager referManager;

    [SyncVar(hook = nameof(OnChangeWalkingState))]
    private bool isWalking;
   
    [SyncVar(hook = nameof(OnChangeRunningState))]
    private bool isRunning;

    [SyncVar(hook = nameof(OnChangeIdleState))]
    private bool isIdle;
    [SyncVar(hook = nameof(OnChangeAimState))]
    private bool isAiming;

    private bool canInspect = true;

    public bool IsWalking 
    { 
        get 
        { 
            return isWalking; 
        } 
        set 
        {
            if(value!= isWalking&&value)
            {             
                DoWalk(value);
            }
            else if (value != isWalking && !value)
            {
                CmdCanWalk(value);
            }
            isWalking = value;
        } 
    }
    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            if(value!=isRunning&&value)
            {
                DoRun(value);
            }
            else if(value!=isRunning&&!value)
            {
                CmdCanRun(value);
            }
            isRunning = value;
        }
    }
    public bool IsIdle
    {
        get { return isIdle; }
        set
        {
            if(value!=isIdle&&value)
            {
                DoIdle(value);
            }
            else if (value != isIdle && !value)
            {
                CmdCanIdle(value);
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
            if(isAiming!=value)
            {
                if(value)
                {
                    DoAimIn(value);
                }
                else
                {
                    DoAimOut(value);
                }
            }
            isAiming = value;
            CmdAim(value);
        }
    }

    private void Update()
    {
        if (!isOwned) { return; }
        canInspect = true;
        bool isReloading = referManager.WeaponManager.CurrentWeapon.IsReloading;
        if(!isReloading)
        {
            HandleMovement();
            HandleAttack();
            HandleAim();
        }
        else
        {
            ResetMovementState();
        }
        HandleReload();
    }
    private void ResetMovementState()
    {
        IsIdle = false;
        IsWalking = false;
        IsRunning = false;
    }

    private void HandleMovement()
    {
        
        IsIdle = !IsRunning && !IsWalking&&!IsAiming;
        IsWalking = referManager.Controller.IsWalking&&!IsAiming;
        IsRunning = referManager.Controller.IsRunning;
        canInspect =  !isRunning;

    }
    private void HandleAttack()
    {
        WeaponBase weapon = referManager.WeaponManager.CurrentWeapon;
        if (Input.GetMouseButtonDown(0))
        {
            if(weapon.ShootType==ShootType.Single)
            {
                weapon.CheckShoot(IsAiming);
                CmdAttack();
            }
        }
        if(Input.GetMouseButton(0))
        {
            if(weapon.ShootType==ShootType.Continuous)
            {
                weapon.CheckShoot(IsAiming);
                CmdAttack();
            }
        }
       
    }
    private void HandleAim()
    {
        if (IsRunning) { return; }
        IsAiming = Input.GetMouseButton(1);
    }
    private void HandleReload()
    {
        
        if(Input.GetKeyDown(KeyCode.R))
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
        referManager.WeaponManager.CurrentWeapon.DoAimOut();
    }
    private void DoReload()
    {
        referManager.WeaponManager.CurrentWeapon.CheckReload();
        CmdReload();
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
    private void OnChangeIdleState(bool oldState,bool newState)
    {
        if (isOwned) { return; }
        if (!newState) { return; }
        referManager.WeaponManager.CurrentWeapon.IdleAnimation();
    }
    private void OnChangeAimState(bool oldState, bool newState)
    {
        if (isOwned) { return; }
        if(newState)
        {
            referManager.WeaponManager.CurrentWeapon.CheckAimIn();
        }
        else
        {
            referManager.WeaponManager.CurrentWeapon.DoAimOut();
        }
    }
    
    [ClientRpc]
    private void RpcFire()
    {
        if (isOwned) { return; }
        referManager.WeaponManager.CurrentWeapon.CheckShoot(isAiming);
    }
    [ClientRpc]
    private void RpcReload()
    {
        if (isOwned) { return; }
        referManager.WeaponManager.CurrentWeapon.CheckReload();
    }

    #endregion

    #region Server
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
    private void CmdAttack()
    {
        RpcFire();
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
    #endregion


}
