using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using Random = UnityEngine.Random;

public enum ShootType
{
    Single,
    Continuous
}
public class ShootWeaponBase : WeaponBase
{
    public event Action<int, int> OnChangeBulletLeft;
    public event Action<Vector3, Vector3, int, float, int> OnShoot;
    public event Action OnFire;

    private readonly int UnArmFireHash = Animator.StringToHash("Unaim Fire");
    private readonly int ArmFireHash = Animator.StringToHash("Aim Fire ");
    private readonly int AimInHash = Animator.StringToHash("Aim In");
    private readonly int AimOutHash = Animator.StringToHash("Aim Out");
    private readonly int ReloadHash = Animator.StringToHash("Reload");
    private readonly int InspectHash = Animator.StringToHash("Inspect");
    private readonly int ReadyThrowHash = Animator.StringToHash("ReadyThrow");
    private readonly int ThrowHash = Animator.StringToHash("Throw");

    [field: SerializeField] public ShootType ShootType { get; private set; }
    [Header("Attribute")]
    [SerializeField] private float fireTime = 0.1f;
    [SerializeField] private float inspectTimeBegin = 5f;
    [SerializeField] protected int maxBulletInMag = 30;
    [SerializeField] protected int maxBullet = 150;
    [SerializeField] protected int bulletPerShoot = 1;
    [SerializeField] protected bool canShootWhileReloading = false;
    [field: SerializeField] public bool CanDelayInShoot { get; private set; } = false;
    [field: SerializeField] public bool IsComplexReload { get; private set; } = false;
    [Header("Sounds")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip ReloadSound;
    [SerializeField] private AudioClip boltMainSound;
    [SerializeField] private AudioClip runOutOfAmmoInMagSound;
    [Header("Recoil")]
    [SerializeField] private float recoilAmountX;
    [SerializeField] private float recoilAmountY;
    [SerializeField] private float aimRecoilSensitivity;

    [Header("Shoot")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LayerMask layers;
    [SerializeField] private int shootForce;
    [SerializeField] private float spread = 0f;
    [SerializeField] private float timeToDeactivateProjectile = 5f;
    [SerializeField] private float rangeShoot = float.MaxValue;
    [Header("Boom")]
    [SerializeField] private BoomManager boomManager;
    [SerializeField] private GameObject boomPack;
    [SerializeField] private List<Boom> booms;
    [Header("Icons")]
    [SerializeField] private Sprite bulletIcon;
    public Sprite BulletIcon { get { return bulletIcon; } }

    public bool IsOwnedByPlayer { get { return isOwnedByPlayer; } }

    protected bool canReload = true;
    private bool canReadyThrow = true;
    private bool canThrow = true;
    private float accumulatedInspectTime = 0f;
    private int bulletLeft = -1;

    private float currentRecoilXPos;
    private float currentRecoilYPos;
    private Vector3 targetPoint;
    private RaycastHit[] hits;

    protected int currentBulletInMag = 0;

    public int BulletLeftInMag { get { return currentBulletInMag; } }
    public int BulletLeft { get { return maxBullet; } }
    public int BulletMaxInMag { get { return maxBulletInMag; } }
    public bool IsReloading { get { return !canReload; } }


    public bool IsThrowing { get { return !canReadyThrow; } }





    private void RecoilMath(float sensitivity)
    {
        currentRecoilXPos = ((UnityEngine.Random.value - .5f) / 2) * recoilAmountX * sensitivity;
        currentRecoilYPos = ((UnityEngine.Random.value - .5f) / 2) * recoilAmountY * sensitivity;
        fps.MouseLook.XRotRecoil = -Mathf.Abs(currentRecoilYPos);
        fps.MouseLook.YRotRecoil = currentRecoilXPos;
    }
    private void Awake()
    {
        hits = new RaycastHit[bulletPerShoot];
    }
    private void Start()
    {
        AssignBullet();
    }
    private void AssignBullet()
    {
        if (bulletLeft > -1) { return; }
        currentBulletInMag = maxBulletInMag;
        bulletLeft = maxBullet;
        PackWeaponUI pack = UIManager.Instance.Packs[0];
        pack.ChangeBulletLeftAmountDisplay(currentBulletInMag, BulletLeft);
    }
    private void OnEnable()
    {
        canReload = true;
        base.InitializeWeapon();
        // update UI
        PackWeaponUI pack = UIManager.Instance.Packs[0];
        pack.ChangeBulletLeftAmountDisplay(currentBulletInMag, BulletLeft);

    }
    protected override void TakeOutWeapon()
    {
        base.TakeOutWeapon();
        if (currentBulletInMag == 0)
        {
            CheckReload();
        }
    }
    protected void ChangeAmountBullet(int bulletInMag, int bulletL)
    {
        OnChangeBulletLeft?.Invoke(bulletInMag, bulletL);
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
    public void InspectAnimation()
    {
        Animator.CrossFadeInFixedTime(InspectHash, CrossFadeTime);
    }
    public void ReloadAnimation()
    {
        Animator.CrossFadeInFixedTime(ReloadHash, CrossFadeTime);
    }
    private void ReadyThrowGrenadeAnimation()
    {
        Animator.CrossFadeInFixedTime(ReadyThrowHash, CrossFadeTime);
    }
    private void ThrowGrenadeAnimation()
    {
        Animator.CrossFadeInFixedTime(ThrowHash, CrossFadeTime);
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
        if (TryGetComponent<Scope>(out Scope scope))
        {
            scope.ScopeUp();
        }
    }
    public void DoAimOut()
    {
        AimOutAnimation();
        if (TryGetComponent<Scope>(out Scope scope))
        {
            scope.ScopeDown();
        }
    }
    public void CheckReload()
    {
        if (!canReload) { return; }
        if (ItemAttribute.Type != ItemType.Sniper)
        {
            if (TryGetComponent<Scope>(out Scope scope))
            {
                scope.ScopeDown();
            }
        }
        playerController.PlayerSound.PlayReloading();
        Reload();
    }
    protected virtual void Reload()
    {
        ReloadAnimation();
        audioSource.PlayOneShot(ReloadSound);
        canReload = false;
        CalculateAmmoToReload();

    }
    private void CalculateAmmoToReload()
    {
        int needAmmorReload = maxBulletInMag - currentBulletInMag;
        int ammoCanReload = maxBullet - needAmmorReload > 0 ? needAmmorReload : maxBullet;
        maxBullet -= ammoCanReload;
        currentBulletInMag += ammoCanReload;
    }
    public override void CheckAttack(bool isAiming)
    {
        if (isTakingOut) { return; }
        if (!canAttack) { return; }
        if (!canShootWhileReloading)
        {
            if (!canReload) { return; }
        }
        Attack(isAiming);
        OnFire?.Invoke();

    }
    protected override void Attack(bool isAiming)
    {
        canAttack = false;
        if (currentBulletInMag > 0)
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }

            currentBulletInMag -= 1;
            if (isAiming)
            {
                ArmFireAnimation();
                RecoilMath(aimRecoilSensitivity);
            }
            else
            {
                UnArmFireAnimation();
                RecoilMath(1);
            }

            for (int i = 0; i < bulletPerShoot; i++)
            {

                ShootRayCast(i);
                PlayFireSound();

            }
        }
        else
        {
            audioSource.PlayOneShot(runOutOfAmmoInMagSound);
        }
        OnChangeBulletLeft?.Invoke(currentBulletInMag, maxBullet);
        StartCoroutine(ResetFire());
    }
    public Vector3 GetDirectionBeforeShooting()
    {
        Ray ray = fps.FirstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        return ray.direction;
    }

    private Vector3 CalculateDirectionWithSpread(Vector3 startPoint, Vector3 spreadV, int index)
    {
        Ray ray = fps.FirstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f) + spreadV);
        if (Physics.Raycast(ray, out hits[index], rangeShoot, layers))
        {
            targetPoint = hits[index].point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
            hits[index].point = targetPoint;
        }
        Vector3 directionWithoutSpread = targetPoint - startPoint;
        return directionWithoutSpread;
    }

    protected virtual void ShootRayCast(int index)
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 spreadV = new Vector3(x, y, 0);
        Vector3 directionWithSpread = CalculateDirectionWithSpread(attackPoint.position, spreadV, index);
        OnShoot?.Invoke(hits[index].point, spreadV, shootForce, timeToDeactivateProjectile, damage);
        ObjectPoolManager objPManager = GetObjectPoolManager();
        IProjectilePool tempPrj = objPManager.GetReadyObject() as IProjectilePool;
        if (tempPrj == null)
        {
            if (projectile == null) { return; }
            GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
            currentBullet.transform.forward = directionWithSpread;
            if (currentBullet.TryGetComponent<Projectile>(out Projectile prj))
            {
                prj.SetRaycastHit(hits[index]);
                prj.SetPlayerController(playerController);
                prj.SetDamage(damage);
                prj.ShootProjectile();
            }
        }
        else
        {
            tempPrj.ReturnToNewState(attackPoint.position, directionWithSpread.normalized);
            tempPrj.SetBeforeShoot(playerController, damage, hits[index]);
            tempPrj.SetActive();
        }

    }
    protected virtual ObjectPoolManager GetObjectPoolManager()
    {
        return ParentPoolManagers.Instance.GetObjPoolManager(TypeObjectPoolManager.PROJECTILE_FPS);
    }

    private void ResetAcInspectTime()
    {
        accumulatedInspectTime = 0f;
    }
    public void CheckInspect(float deltaTime, bool canInspect)
    {
        if (!canInspect)
        {
            ResetAcInspectTime();
            CanInspect = false;
            return;
        }
        accumulatedInspectTime += deltaTime;
        if (accumulatedInspectTime < inspectTimeBegin) { return; }
        if (accumulatedInspectTime < inspectTimeBegin + 0.1f) { CanInspect = false; return; }
        CanInspect = true;
        accumulatedInspectTime = 0f;
        Inspect();

    }
    private void Inspect()
    {
        InspectAnimation();
    }
    private void PlayFireSound()
    {
        audioSource.PlayOneShot(fireSound);
    }
    private IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(fireTime);
        canAttack = true;
    }

    #region Animation
    public void PlayBolt()
    {
        canReload = true;
        OnChangeBulletLeft?.Invoke(currentBulletInMag, maxBullet);
    }
    public void FinishThrowGrenade()
    {
        canReadyThrow = true;
        canThrow = true;
    }
    public void PlayTakeOut()
    {
        if (boltMainSound == null) { return; }
        audioSource.PlayOneShot(boltMainSound);
    }

    #endregion
    public bool IsFullBulletInMag()
    {
        return currentBulletInMag == maxBulletInMag;
    }
    public void SetFullBulletLeft()
    {
        if (IsDefaultWeapon)
        {
            AssignBullet();
        }
        maxBullet = bulletLeft;
        OnChangeBulletLeft?.Invoke(currentBulletInMag, bulletLeft);
    }

}
