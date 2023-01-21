using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemSelection : MonoBehaviour
{
    [SerializeField] private Image iconButton;
    private PlayerController ownedPlayer;
    public string NameItem{get;private set;}
    public void SetTitleButton(string name,Sprite iconWeapon,PlayerController player)
    {
        NameItem = name;
        this.iconButton.sprite = iconWeapon;
        ownedPlayer = player;
    }
    public void OnChooseItem()
    {
        ownedPlayer.GetComponent<HandlePickUp>().PickUpItem(NameItem,true);
        
    }
}
