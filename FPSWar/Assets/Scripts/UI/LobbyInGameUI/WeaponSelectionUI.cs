using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Unity.VisualScripting;
public class WeaponSelectionUI : MonoBehaviour
{
    public event Action OnGetPlayer;
    [SerializeField] private Transform weaponContent;
    [SerializeField] private Transform defaultWeaponContent;
    [SerializeField] private Transform attachmentContent;
    [SerializeField] private Transform defaultAttachmentSelection;
    [SerializeField] private ItemSelection weaponSelectionPrefab;
    [SerializeField] private ItemSelection defaultWeaponSeletionPrefab;
    [SerializeField] private ItemSelection attachmentSelectionPrefab;
    [SerializeField] private GameObject weaponSelection;
    [SerializeField] private GameObject defaultWeaponSelection;
    [SerializeField] private GameObject attachmentSelection;
    [Header("Weapon Selection UI")]
    [SerializeField] private int maxHorizontalWeapon = 3;
    [SerializeField] private GameObject horizontalContent;
    [Header("Grenade Selection")]
    [SerializeField] private GameObject grenadeContent;
    [SerializeField] private ItemSelection grenadeItem;
    public PlayerController OwnedPlayer { get; private set; }
    private List<ItemSelection> attachments;
    private List<ItemSelection> weaponItems;
    private List<GameObject> horizontalContents;
    private Coroutine attachmentCoroutine;
    private void Start()
    {
        attachments = new List<ItemSelection>();
        weaponItems = new List<ItemSelection>();
        horizontalContents = new List<GameObject>();
        ItemSelection.OnClickButton += TurnOnAttachmentSelection;
        ItemSelection.OnClickToWeapon += ToggleOffWeaponRing;
        ItemSelection.OnClickToAttachment += ToggleOffAttachmentRing;
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        foreach (var player in players)
        {
            if (player.isOwned)
            {
                OwnedPlayer = player;
                break;
            }
        }
        OnGetPlayer?.Invoke();
        LoadAllWeapon();
        attachments.Add(defaultAttachmentSelection.GetComponent<ItemSelection>());
        Team.OnReturnLobbyInGame+=HandleToReturnLobby;
    }

    private void OnDestroy()
    {
        ItemSelection.OnClickButton -= TurnOnAttachmentSelection;
        ItemSelection.OnClickToWeapon -= ToggleOffWeaponRing;
        ItemSelection.OnClickToAttachment -= ToggleOffAttachmentRing;
        Team.OnReturnLobbyInGame-=HandleToReturnLobby;
    }
    public void HandleToReturnLobby()
    {
        if (weaponItems == null) { return; }
        if (weaponItems.Count == 0) { return; }
        if (UIManager.Instance.GameRule == GameRule.Rob)
        {
            ToggleInteractableWeaponButton(false);
            Invoke(nameof(HandleChooseRandomWeapon), 1f);
        }
        else
        {
            ToggleInteractableWeaponButton(true);
        }
    }
    private void LoadAllWeapon()
    {
        WeaponBase[] weapons = Resources.LoadAll<WeaponBase>("FPSWeapons/");
        int countPerHorizontal = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].IsDefaultWeapon)
            {
                ItemSelection itemInstance = Instantiate(defaultWeaponSeletionPrefab, defaultWeaponContent);
                itemInstance.SetTitleButton(weapons[i].gameObject.name, weapons[i].ItemAttribute.Icon, OwnedPlayer);
            }
            else if (weapons[i] is ShootWeaponBase)
            {
                if (countPerHorizontal == maxHorizontalWeapon)
                {
                    countPerHorizontal = 0;
                }
                if (countPerHorizontal == 0)
                {
                    var horizontalContentInstance = Instantiate(horizontalContent, weaponContent);
                    horizontalContents.Add(horizontalContentInstance);
                }
                countPerHorizontal++;
                ItemSelection itemInstance = Instantiate(weaponSelectionPrefab,
                        horizontalContents[horizontalContents.Count - 1].transform);
                itemInstance.SetTitleButton(weapons[i].gameObject.name, weapons[i].ItemAttribute.Icon, OwnedPlayer);
                weaponItems.Add(itemInstance);
            }
            else if (weapons[i] is GrenadeWeaponBase)
            {
                var grenadeItemInstance = Instantiate(grenadeItem, grenadeContent.transform);
                grenadeItemInstance.SetTitleButton(weapons[i].gameObject.name, weapons[i].ItemAttribute.Icon, OwnedPlayer);
                grenadeItemInstance.GetComponent<ItemPurcharse>().BoomType = (weapons[i] as GrenadeWeaponBase).BoomType;
            }
        }
        HandleToReturnLobby();

    }
    private void LoadAllAttachment(WeaponBase weaponBase)
    {
        if (weaponBase.TryGetComponent<WeaponAdjustment>(out WeaponAdjustment adjustment))
        {
            foreach (var scope in adjustment.GetAllScope())
            {
                ItemSelection itemInstance = Instantiate(attachmentSelectionPrefab, attachmentContent);
                itemInstance.SetTitleButton(scope.ScopeInfor.Name, scope.ScopeInfor.Icon, OwnedPlayer);
                attachments.Add(itemInstance);
            }
        }
    }
    private void ToggleInteractableWeaponButton(bool state)
    {
        foreach (var item in weaponItems)
        {
            var button = item.GetComponent<ButtonController>();
            button.SetInteractable(state);
        }
    }
    private void HandleChooseRandomWeapon()
    {
        ToggleOffWeaponRing();

        int index = UnityEngine.Random.Range(0, weaponItems.Count);
        weaponItems[index].ToggleRing(true);
        string nameItem = weaponItems[index].NameItem;
        OwnedPlayer.GetComponent<HandlePickUp>().PickUpItem(nameItem, true);
        TurnOnAttachmentSelection();
    }
    public void ToggleOffAttachmentRing()
    {
        if (attachments == null) { return; }
        foreach (var attachment in attachments)
        {
            attachment.ToggleRing(false);
        }
    }
    public void ToggleOffWeaponRing()
    {
        if (attachments == null) { return; }
        foreach (var weapon in weaponItems)
        {
            weapon.ToggleRing(false);
        }
    }
    private void TurnOnAttachmentSelection()
    {
        ClearInforAttachmentDisplay();
        if(attachmentCoroutine!=null)
        {
            StopCoroutine(attachmentCoroutine);
            attachmentCoroutine=null;
        }
        attachmentCoroutine = StartCoroutine(DelayTimeDisplayAttachmentUI());
    }
    public void ClearAllRings()
    {
        ToggleOffAttachmentRing();
        ToggleOffWeaponRing();
        ClearInforAttachmentDisplay();
    }
    private IEnumerator DelayTimeDisplayAttachmentUI()
    {
        yield return new WaitForSeconds(0.5f);
        LoadAllAttachment(OwnedPlayer.GetComponent<WeaponManager>().CurrentWeapon);
    }
    public void ClearInforAttachmentDisplay()
    {
        foreach (Transform child in attachmentContent)
        {
            if (child == defaultAttachmentSelection) { continue; }
            Destroy(child.gameObject);
        }
    }
}
