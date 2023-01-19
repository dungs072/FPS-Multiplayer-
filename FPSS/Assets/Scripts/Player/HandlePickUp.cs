using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class HandlePickUp : NetworkBehaviour
{
    [SerializeField] private ReferenceManager refer;
    [SerializeField] private HandleDrop handleDrop;
    [SerializeField] private int maxGun = 2;
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
    }
    public void PickUpItem(string nameWeapon = "", bool isSelect = false)
    {
        if (refer.WeaponManager.Weapons.Count == maxGun)
        {
            if (refer.WeaponManager.CurrentWeapon.IsDefaultWeapon) { return; }
            handleDrop.ThrowItem();
            StartCoroutine(DoPickUpItemDelay(nameWeapon,isSelect));
        }
        else
        {
            DoPickUpItem(nameWeapon,isSelect);
        }

    }
    private IEnumerator DoPickUpItemDelay(string nameWeapon = "",bool isSelect = false)
    {
        yield return new WaitForSeconds(0.1f);
        DoPickUpItem(nameWeapon,isSelect);
    }
    private void DoPickUpItem(string nameWeapon = "",bool isSelect = false)
    {
        if(isSelect)
        {
            refer.WeaponManager.EquipFpsWeapon(nameWeapon);
            refer.WeaponTPPManager.LoadTppWeapon(nameWeapon);
            return;
        }
        if(pickUps.Count==0){return;}
        if(pickUps[0]==null){return;}
        if(!pickUps[0].CanPickup){return;}
        refer.WeaponManager.EquipFpsWeapon(pickUps[0].Name);
        refer.WeaponTPPManager.LoadTppWeapon(pickUps[0].Name);
        RemovePickUpItem(pickUps[0]);
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
}
