using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponBase : MonoBehaviour
{
    protected const float CrossFadeTime = 0.1f;
    protected readonly int IdleHash = Animator.StringToHash("Idle");
    protected readonly int RunHash = Animator.StringToHash("Run");
    protected readonly int WalkHash = Animator.StringToHash("Walk");

    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected FirstPersonController fps;
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ItemAttribute ItemAttribute { get; private set; }
    [field: SerializeField] public bool IsDefaultWeapon { get; private set; } = false;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected float takeOutTime = 0.5f;
    [SerializeField] protected float takeOutTimeTPP = 0.5f;
    [SerializeField] protected int damage = 10;
    [Header("Base sound")]
    [SerializeField] private AudioClip takeOutSound;
    [Header("Is gun owned by player ?")]
    [SerializeField] protected bool isOwnedByPlayer;

    public float TakeOutTimeTPP { get { return takeOutTimeTPP; } }
    public bool CanInspect { get; protected set; } = false;
    public bool CanAttack { get { return canAttack; } }
    public bool IsTakingOut { get { return isTakingOut; } }
    protected bool canAttack = true;
    protected bool isTakingOut = true;

    public bool CanUse{get;set;} = true;

    public void RunAnimation()
    {
        Animator.CrossFadeInFixedTime(RunHash, CrossFadeTime);
    }
    public virtual void WalkAnimation()
    {
        Animator.CrossFadeInFixedTime(WalkHash, CrossFadeTime);
    }
    public virtual void IdleAnimation()
    {
        Animator.CrossFadeInFixedTime(IdleHash, CrossFadeTime);
    }
    // is special here indicate that if this weapon can shoot. isSpecial = isAiming, 
    // if it is a melee weapon. isSpecial = isBlocking
    public virtual void CheckAttack(bool isSpecial) { }
    protected virtual void Attack(bool isSpecial) { }
    protected virtual void InitializeWeapon()
    {
        isTakingOut = true;
        canAttack = true;
        audioSource.PlayOneShot(takeOutSound);
        Invoke(nameof(TakeOutWeapon), takeOutTime);

    }
    protected virtual void TakeOutWeapon()
    {
        isTakingOut = false;
        playerController.ResetMovementState();
    }
    public void SetPlayerControllerAndFPS(PlayerController playerController, FirstPersonController fps)
    {
        this.playerController = playerController;
        this.fps = fps;
    }

    public void ChangeCurrentAnimatorIntoNewAnimator(AnimatorOverrideController animatorOverrideController)
    {
        if (animatorOverrideController == null)
        {
            var overrideAnimator = Animator.runtimeAnimatorController
                                   as AnimatorOverrideController;
            if (overrideAnimator != null)
            {
                Animator.runtimeAnimatorController = overrideAnimator.runtimeAnimatorController;
            }
        }
        else
        {
            Animator.runtimeAnimatorController = animatorOverrideController;
        }
        //Animator.Update(Time.deltaTime);
    }
}
