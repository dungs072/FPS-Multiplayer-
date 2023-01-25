using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class WeaponSelectionUI : MonoBehaviour
{
    public event Action OnGetPlayer;
    [SerializeField] private Transform content;
    [SerializeField] private Transform attachmentContent;
    [SerializeField] private Transform defaultAttachmentSelection;
    [SerializeField] private ItemSelection weaponSelectionPrefab;
    [SerializeField] private ItemSelection attachmentSelectionPrefab;
    [SerializeField] private GameObject weaponSelection;
    [SerializeField] private GameObject attachmentSelection;
    public PlayerController OwnedPlayer { get; private set; }
    private void Start()
    {
        ItemSelection.OnClickButton += TurnOnAttachmentSelection;
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
    }
    private void OnDestroy()
    {
        ItemSelection.OnClickButton -= TurnOnAttachmentSelection;
    }
    private void LoadAllWeapon()
    {
        WeaponBase[] weapons = Resources.LoadAll<WeaponBase>("FPSWeapons/");

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].IsDefaultWeapon) { continue; }
            ItemSelection itemInstance = Instantiate(weaponSelectionPrefab, content);
            itemInstance.SetTitleButton(weapons[i].gameObject.name, weapons[i].ItemAttribute.Icon, OwnedPlayer);
        }
    }
    private void LoadAllAttachment(WeaponBase weaponBase)
    {
        
        if (weaponBase.TryGetComponent<WeaponAdjustment>(out WeaponAdjustment adjustment))
        {
            foreach (var scope in adjustment.GetAllScope())
            {
                ItemSelection itemInstance = Instantiate(attachmentSelectionPrefab, attachmentContent);
                itemInstance.SetTitleButton(scope.ScopeInfor.Name,scope.ScopeInfor.Icon, OwnedPlayer);
            }
        }
    }
    private void TurnOnAttachmentSelection()
    {
        weaponSelection.SetActive(false);
        attachmentSelection.SetActive(true);
        ClearInforAttachmentDisplay();
        Invoke(nameof(DelayTimeDisplayAttachmentUI),0.5f);
    }
    public void TurnOnWeaponponSelection()
    {
        attachmentSelection.SetActive(false);
        weaponSelection.SetActive(true);
    }
    private void DelayTimeDisplayAttachmentUI()
    {
        LoadAllAttachment(OwnedPlayer.GetComponent<WeaponManager>().CurrentWeapon);
    }
    public void ClearInforAttachmentDisplay()
    {
        foreach(Transform child in attachmentContent)
        {
            if(child==defaultAttachmentSelection){continue;}
            Destroy(child.gameObject);
        }
    }
}
