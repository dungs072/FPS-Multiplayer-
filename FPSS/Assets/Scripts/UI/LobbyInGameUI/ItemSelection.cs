using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;
using TMPro;
public class ItemSelection : MonoBehaviour
{
    public static event Action OnClickButton;
    [SerializeField] private Image iconButton;
    [SerializeField] private bool isNotNone; 
    [SerializeField] private TMP_Text nameItem;
    [SerializeField] private bool isDefaultItem;
    private PlayerController ownedPlayer;
    public string NameItem{get;private set;}
    private void Start() {
        ownedPlayer = NetworkClient.connection.identity.GetComponent<PlayerController>();
    }
    public void SetTitleButton(string name,Sprite iconWeapon,PlayerController player)
    {
        NameItem = name;
        nameItem.text = name;
        this.iconButton.sprite = iconWeapon;
        ownedPlayer = player;
    }
    public void OnChooseItem(bool isAttach)
    {
        if(isDefaultItem)
        {
            ownedPlayer.GetComponent<WeaponManager>().ChangeWeaponIndex(0);
            OnClickButton?.Invoke();
            return;
        }
        if(isAttach)
        {
            ownedPlayer.GetComponent<HandlePickUp>().PickUpAttachment(NameItem,isNotNone);
        }
        else
        {
            ownedPlayer.GetComponent<HandlePickUp>().PickUpItem(NameItem,true);
            OnClickButton?.Invoke();
        }
    }
}
