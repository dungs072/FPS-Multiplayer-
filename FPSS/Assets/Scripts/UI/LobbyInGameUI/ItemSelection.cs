using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;
public class ItemSelection : MonoBehaviour
{
    public static event Action OnClickButton;
    [SerializeField] private Image iconButton;
    [SerializeField] private bool isNotNone;
    private PlayerController ownedPlayer;
    public string NameItem{get;private set;}
    private void Start() {
        ownedPlayer = NetworkClient.connection.identity.GetComponent<PlayerController>();
    }
    public void SetTitleButton(string name,Sprite iconWeapon,PlayerController player)
    {
        NameItem = name;
        this.iconButton.sprite = iconWeapon;
        ownedPlayer = player;
    }
    public void OnChooseItem(bool isAttach)
    {
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
