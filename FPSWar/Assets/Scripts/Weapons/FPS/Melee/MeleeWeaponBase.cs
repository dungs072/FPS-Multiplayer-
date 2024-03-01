using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBase : WeaponBase
{
    public event Action OnAttack;
    [SerializeField] private float resetAttackTime = 0.5f;
    [SerializeField] private MeleeWeaponItem item;
    [Header("Effect")]
    [SerializeField] private MeleeEffectAttribute effect;
    [SerializeField] private Transform hitEffectTransform;
    [Header("Sound")]
    [SerializeField] private MeleeSoundAttribute meleeSoundAttribute;
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int CanBlockHash = Animator.StringToHash("CanBlock");
    private void Start()
    {
        item.OnCollideWithTarget += OnCollideWithTarget;
    }
    private void OnDestroy()
    {
        item.OnCollideWithTarget -= OnCollideWithTarget;
    }
    public void AttackAnimation()
    {
        Animator.CrossFadeInFixedTime(AttackHash, CrossFadeTime);
    }
    public void BlockAnimation(bool state)
    {
        Animator.SetBool(CanBlockHash, state);
    }
    private void OnEnable()
    {
        base.InitializeWeapon();
    }
    public override void CheckAttack(bool isSpecial)
    {
        if (isTakingOut) { return; }
        if (!canAttack) { return; }
        Attack(isSpecial);
    }
    protected override void Attack(bool isSpecial)
    {
        canAttack = false;
        OnAttack?.Invoke();
        AttackAnimation();
        audioSource.PlayOneShot(meleeSoundAttribute.LongTailAudio);
        StartCoroutine(ResetAttack());
    }
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(resetAttackTime);
        canAttack = true;
    }
    private void OnCollideWithTarget(Collider collider)
    {
        if (collider == playerController.GetComponent<Collider>()) { return; }
        if(collider.CompareTag("Player")){return;}
        if (collider.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage, playerController.transform);
            SpawnEffect(EffectNetworkType.BloodWithoutDecal,health.IsHead);
        }
        else
        {
            SpawnEffect(EffectNetworkType.WoodWithoutDecal);
        }

    }
    private void SpawnEffect(EffectNetworkType effectType, bool isSpecial = false)
    {
        if (effectType == EffectNetworkType.WoodWithoutDecal)
        {
            audioSource.PlayOneShot(meleeSoundAttribute.ObstacleHitAudio);
            var hitEffectInstance = Instantiate(effect.WoodEffectWithoutDecal,
                    hitEffectTransform.position, hitEffectTransform.rotation);
            Destroy(hitEffectInstance, 1f);
            if (playerController.TryGetComponent(out EffectNetworkManager effectNetwork))
            {
                if (effectNetwork.isOwned)
                {
                    effectNetwork.CmdSpawnWoodEffectWithoutDecal(hitEffectTransform.position,
                                                                hitEffectTransform.rotation,
                                                                effectType);
                }
            }
        }
        else if (effectType == EffectNetworkType.BloodWithoutDecal)
        {
            if(isSpecial)
            {
                audioSource.PlayOneShot(meleeSoundAttribute.PonkAudio);
            }
            else
            {
                audioSource.PlayOneShot(meleeSoundAttribute.BodyHitAudio);
            }
            
            var hitEffectInstance = Instantiate(effect.BloodEffectWithoutDecal,
                   hitEffectTransform.position, hitEffectTransform.rotation);
            Destroy(hitEffectInstance, 1f);
            if (playerController.TryGetComponent(out EffectNetworkManager effectNetwork))
            {
                if (effectNetwork.isOwned)
                {
                    effectNetwork.CmdSpawnWoodEffectWithoutDecal(hitEffectTransform.position,
                                                                hitEffectTransform.rotation,
                                                                effectType);
                }
            }
        }
    }
    #region Animation
    public void OnStartUsingMelee()
    {
        item.ToggleCollider(true);
    }
    public void OnStopUsingMelee()
    {
        item.ToggleCollider(false);
    }
    #endregion
    public void Block()
    {
        BlockAnimation(true);
    }
    public void CancelBlock()
    {
        BlockAnimation(false);
    }
}
