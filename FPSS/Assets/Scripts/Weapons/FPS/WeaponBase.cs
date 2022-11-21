using System.Collections;
using UnityEngine;
public enum ShootType
{
    Single,
    Continuous
}
public class WeaponBase : MonoBehaviour
{
    private const float CrossFadeTime = 0.1f;
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int RunHash = Animator.StringToHash("Run");
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private readonly int UnArmFireHash = Animator.StringToHash("Unaim Fire");
    private readonly int ArmFireHash = Animator.StringToHash("Aim Fire ");
    private readonly int AimInHash = Animator.StringToHash("Aim In");
    private readonly int AimOutHash = Animator.StringToHash("Aim Out");
    private readonly int ReloadHash = Animator.StringToHash("Reload");
    private readonly int InspectHash = Animator.StringToHash("Inspect");
    [field: SerializeField] public Animator Animator { get; private set; }
    [field:SerializeField] public ShootType ShootType { get; private set; }
    [Header("Attribute")]
    [SerializeField] private float fireTime = 0.1f;
    [SerializeField] private float inspectTimeBegin = 5f;
    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip ReloadSound;


    private bool canShoot = true;
    private bool canReload = true;
    private float accumulatedInspectTime = 0f;

    public bool IsReloading { get { return !canReload; } }
  
    public void RunAnimation()
    {
        Animator.CrossFadeInFixedTime(RunHash, CrossFadeTime);
    }
    public void WalkAnimation()
    {
        Animator.CrossFadeInFixedTime(WalkHash, CrossFadeTime);
    }
    public void IdleAnimation()
    {
        Animator.CrossFadeInFixedTime(IdleHash, CrossFadeTime);
    }
    public void UnArmFireAnimation()
    {
        Animator.CrossFadeInFixedTime(UnArmFireHash, CrossFadeTime);
    }
    public void ArmFireAnimation()
    {
        Animator.CrossFadeInFixedTime(ArmFireHash, CrossFadeTime);
    }
    public void AimInAnimation()
    {
        Animator.CrossFadeInFixedTime(AimInHash, CrossFadeTime);
    }
    public void AimOutAnimation()
    {
        Animator.CrossFadeInFixedTime(AimOutHash, CrossFadeTime);
    }
    public void ReloadAnimation()
    {
        Animator.CrossFadeInFixedTime(ReloadHash, CrossFadeTime);
    }
    public void SetCanAim(bool state)
    {
        Animator.SetBool(AimInHash, state);
    }
    public void ClearAcInspectTime()
    {
        accumulatedInspectTime = 0f;
    }
    public void CheckAimIn()
    {
        DoAimIn();
    }
    private void DoAimIn()
    {
        AimInAnimation();
    }
    public void DoAimOut()
    {
        AimOutAnimation();
    }
    public void CheckReload()
    {
        if (!canReload) { return; }
        Reload();
    }
    private void Reload()
    {
        ReloadAnimation();
        audioSource.PlayOneShot(ReloadSound);
        canReload = false;
    }
    public void CheckShoot(bool isAiming)
    {
        if (!canShoot) { return; }
        Shoot(isAiming);
    }
    public void Shoot(bool isAiming)
    {
        canShoot = false;
        if(isAiming)
        {
            ArmFireAnimation();
        }
        else
        {
            UnArmFireAnimation();
        }
        PlayFireSound();
        StartCoroutine(ResetFire());
    }
    private void PlayFireSound()
    {
        audioSource.PlayOneShot(fireSound);
    }
    private IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(fireTime);
        canShoot = true;
    }

    #region Animation
    public void PlayBolt()
    {
        canReload = true;
    }

    #endregion

}
