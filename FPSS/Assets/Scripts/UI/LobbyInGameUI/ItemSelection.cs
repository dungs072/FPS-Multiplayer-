using UnityEngine;
using TMPro;
public class ItemSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text titleButton;
    private PlayerController ownedPlayer;
    public string NameItem{get;private set;}
    public void SetTitleButton(string name,PlayerController player)
    {
        NameItem = name;
        titleButton.text = name;
        ownedPlayer = player;
    }
    public void OnChooseItem()
    {
        ownedPlayer.GetComponent<HandlePickUp>().PickUpItem(NameItem,true);
        
    }
}
