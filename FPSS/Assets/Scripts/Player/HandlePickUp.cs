using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class HandlePickUp : NetworkBehaviour
{
    [SerializeField] private ReferenceManager refer;
    [SerializeField] private HandleDrop handleDrop;
    [SerializeField] private int maxGun = 2;
    [SerializeField] private AudioClip pickupSound;
    private List<PickUp> pickUps = new List<PickUp>();

    private bool canPickUp = false;
    public bool CanPickUp
    {
        get { return canPickUp; }
        set
        {
            if (canPickUp != value)
            {
                HandleUIPickup(value);
            }
            canPickUp = value;
        }
    }
    private void Start() {
        PickUp.OnDestroyPickup+=RemovePickUpItem;
    }
    private void OnDestroy() {
        PickUp.OnDestroyPickup-=RemovePickUpItem;
    }
    private void Update()
    {
        if (!isOwned) { return; }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (pickUps.Count > 0)
            {
                PickUpItem();
            }
        }
        CanPickUp = pickUps.Count > 0;
    }
    private void HandleUIPickup(bool state)
    {
        UIManager.Instance.ToggleFButtonUI(state);
        if(state)
        {
            UIManager.Instance.SetItemPickupInfor(pickUps[0].ItemAttribute.Icon,pickUps[0].ItemAttribute.Name);
        }
        
    }
    public void PickUpItem(string nameItem = "", bool isSelect = false)
    {
        refer.AudioSource.PlayOneShot(pickupSound);
        if (pickUps.Count>0&&pickUps[0].ItemAttribute.Type == ItemType.Bullet)
        {
            refer.WeaponManager.SetFullBulletLeft();
        }
        else
        {
            if (refer.WeaponManager.Weapons.Count == maxGun)
            {
                if(isSelect)
                {
                    refer.WeaponManager.ChangeWeaponIndex(1);
                }
                if (refer.WeaponManager.CurrentWeapon.IsDefaultWeapon) { return; }
                handleDrop.ThrowItem(!isSelect);
                StartCoroutine(DoPickUpItemDelay(nameItem, isSelect));
            }
            else
            {
                DoPickUpItem(nameItem, isSelect);
            }
        }
    }
    public void PickupDefaultItem(string nameItem)
    {
        // refer.AudioSource.PlayOneShot(pickupSound);
        // refer.WeaponManager.ThrowDefaultWeapon();
        // refer.WeaponManager.EquipFpsWeapon(nameItem);
    }
    private IEnumerator DoPickUpItemDelay(string nameWeapon = "", bool isSelect = false)
    {
        yield return new WaitForSeconds(0.1f);
        DoPickUpItem(nameWeapon, isSelect);
    }
    private void DoPickUpItem(string nameWeapon = "", bool isSelect = false)
    {
        if (isSelect)
        {
            refer.WeaponManager.EquipFpsWeapon(nameWeapon);
            refer.WeaponTPPManager.LoadTppWeapon(nameWeapon);
            return;
        }
        if (pickUps.Count == 0) { return; }
        if (pickUps[0] == null) { return; }
        if (!pickUps[0].CanPickup) { return; }
        refer.WeaponManager.EquipFpsWeapon(pickUps[0].ItemAttribute.Name);
        refer.WeaponTPPManager.LoadTppWeapon(pickUps[0].ItemAttribute.Name);
        refer.WeaponTPPManager.DisplayWeapons(false);
        PickUp pickUp = pickUps[0];
        RemovePickUpItem(pickUps[0]);
        NetworkServer.Destroy(pickUp.gameObject);
    }
    public void PickUpAttachment(string nameItem,bool isNotNone)
    {
        WeaponAdjustment adjust =  refer.WeaponManager.CurrentWeapon.GetComponent<WeaponAdjustment>();
        if(adjust==null){return;}
        ScopeAttachment scope = adjust.ToggleScope(nameItem,isNotNone);
        refer.WeaponManager.CurrentWeapon.ChangeCurrentAnimatorIntoNewAnimator(scope?.AnimatorOverride);
    }

    public void AddPickUpItem(PickUp pickUp)
    {
        if (pickUps.Contains(pickUp)) { return; }
        pickUps.Add(pickUp);
    }
    public void RemovePickUpItem(PickUp pickUp)
    {
        if (!pickUps.Contains(pickUp)) { return; }
        pickUps.Remove(pickUp);
    }
    private void RemovePickUpItem(GameObject obj)
    {
        RemovePickUpItem(obj.GetComponent<PickUp>());
    }
}
