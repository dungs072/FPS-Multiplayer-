using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeaponBase : WeaponBase
{
    public event Action OnReadyThrow;
    public event Action OnThrow;
    [SerializeField] private Transform spawnPos;
    protected readonly int ReadyThrowHash = Animator.StringToHash("Ready_Throw");
    protected readonly int ThrowHash = Animator.StringToHash("Throw");
    protected readonly int ReadyThrowIdleHash = Animator.StringToHash("Ready_Throw_Idle");
    protected readonly int ReadyThrowWalkHash = Animator.StringToHash("Ready_Throw_Walk");
    [Header("Sounds")]
    [SerializeField] private AudioClip pullGrenadeSound;
    [SerializeField] private AudioClip throwGrenadeSound;
    [Header("Boom type")]
    [SerializeField] private BoomType boomType;
    private bool isReadyThrow = false;
    private bool isDoReadyThrow = false;
    private bool isThrow = false;
    public int GrenadeAmmount { get; set; } = 2;
    public BoomType BoomType { get { return boomType; } }
    private void ReadyThrowAnimation()
    {
        Animator.CrossFadeInFixedTime(ReadyThrowHash, CrossFadeTime);
    }
    public void ThrowAnimation()
    {
        Animator.CrossFadeInFixedTime(ThrowHash, CrossFadeTime);
    }
    public override void IdleAnimation()
    {
        if (isReadyThrow)
        {
            Animator.CrossFadeInFixedTime(ReadyThrowIdleHash, CrossFadeTime);
        }
        else
        {
            Animator.CrossFadeInFixedTime(IdleHash, CrossFadeTime);
        }
    }
    public override void WalkAnimation()
    {
        if (isReadyThrow)
        {
            Animator.CrossFadeInFixedTime(ReadyThrowWalkHash, CrossFadeTime);
        }
        else
        {
            Animator.CrossFadeInFixedTime(WalkHash, CrossFadeTime);
        }
    }
    private void OnEnable()
    {
        base.InitializeWeapon();
        isReadyThrow = false;
        isDoReadyThrow = false;
        isThrow = false;
    }
    public override void CheckAttack(bool isSpecial)
    {
        if (isTakingOut) { return; }
        if (GrenadeAmmount == 0) { return; }
        Attack(isSpecial);
    }
    protected override void Attack(bool isSpecial)
    {
        isReadyThrow = true;
        isDoReadyThrow = true;
        canAttack = false;
        ReadyThrowAnimation();
        playerController.SetForceToStopRunning(true);
        playerController.CanRun(false);

        OnReadyThrow?.Invoke();
    }

    public void ThrowGrenade()
    {
        if (!canAttack)
        {
            isThrow = true;
        }
        else
        {
            HandleThrowGrenade();
        }
    }

    private void HandleThrowGrenade()
    {
        if (!isDoReadyThrow)
        {
            isThrow = false;
            isReadyThrow = false;
            return;
        }
        isDoReadyThrow = false;
        isReadyThrow = false;
        canAttack = false;
        ThrowAnimation();
        playerController.SetForceToStopRunning(false);
        playerController.CanRun(true);
        OnThrow?.Invoke();
        GrenadeAmmount -= 1;
        UIManager.Instance.Packs[0].SetTextDisplayInforWeapon(GrenadeAmmount.ToString());
    }
    #region Animation
    public void FinishAttack()
    {
        canAttack = true;

    }
    public void ImmediateThrowGrenade()
    {
        if (!isThrow) { return; }
        isThrow = false;
        HandleThrowGrenade();
    }
    public void GenerateBoom()
    {
        playerController.GetComponent<BoomManager>().ThrowBoom(spawnPos.forward, spawnPos.position, boomType);
        if (GrenadeAmmount == 0)
        {
            CanUse = false;
            playerController.GetComponent<WeaponManager>().ChangeCurrentWeaponIndex();
        }
    }
    public void PlayPullPinSound()
    {
        audioSource.PlayOneShot(pullGrenadeSound);
    }
    public void PlayThrowSound()
    {
        audioSource.PlayOneShot(throwGrenadeSound);
    }
    #endregion
}
