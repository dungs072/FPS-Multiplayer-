using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBase : WeaponBase
{
    private readonly int ReloadOpenHash = Animator.StringToHash("Reload Open");
    private readonly int ReloadInsertHash = Animator.StringToHash("Reload Insert");
    private readonly int ReloadCloseHash = Animator.StringToHash("Reload Close");
    [SerializeField] private ThirdPersonController tppController;
    [SerializeField] private float openReloadAnimationTime = 1f;
    [SerializeField] private float insertReloadAnimationTime = 0.8f;
    [SerializeField] private float closeReloadAnimationTime = 0.9f;

    [Header("Sound")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip insertSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip boltSound;
    protected override void Reload()
    {
        canReload = false;
        StartCoroutine(DoReload());
    }
    private IEnumerator DoReload()
    {
        ReloadOpenAnimation();
        tppController.DoOpenReload();
        yield return new WaitForSeconds(openReloadAnimationTime);
        int needAmmorReload = maxBulletInMag - currentBulletInMag;
        int ammoCanReload = maxBullet - needAmmorReload > 0 ? needAmmorReload : maxBullet;
        for (int i =0;i<ammoCanReload;i++)
        {
            if (!canShoot&&currentBulletInMag>0) { canReload = true; yield break; }
            ReloadInsertAnimation();
            tppController.DoInsertReload();
            maxBullet -= 1;
            currentBulletInMag += 1;
            ChangeAmountBullet(currentBulletInMag,maxBullet);
            yield return new WaitForSeconds(insertReloadAnimationTime);
           
        }
        if (!canShoot && currentBulletInMag > 0) { canReload = true; yield break; }
        ReloadCloseAnimation();
        tppController.DoCloseReload();
        yield return new WaitForSeconds(closeReloadAnimationTime);
        canReload = true;
    }
    private void ReloadOpenAnimation()
    {
        Animator.CrossFadeInFixedTime(ReloadOpenHash, CrossFadeTime);
        audioSource.PlayOneShot(openSound);
    }
    private void ReloadInsertAnimation()
    {
        Animator.CrossFadeInFixedTime(ReloadInsertHash, CrossFadeTime);
        audioSource.PlayOneShot(insertSound);
    }
    private void ReloadCloseAnimation()
    {
        Animator.CrossFadeInFixedTime(ReloadCloseHash,CrossFadeTime);
        audioSource.PlayOneShot(closeSound);
    }


    #region Animation
    public void PlayBoltSound()
    {
        audioSource.PlayOneShot(boltSound);
    }
    #endregion
}
