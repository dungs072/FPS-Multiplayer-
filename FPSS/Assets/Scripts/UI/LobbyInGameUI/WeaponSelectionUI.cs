using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class WeaponSelectionUI : MonoBehaviour
{
    public event Action OnGetPlayer;
    [SerializeField] private Transform content;
    [SerializeField] private ItemSelection itemSelectionPrefab;
    public PlayerController OwnedPlayer { get; private set; }
    private void Start()
    {
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
    private void LoadAllWeapon()
    {
        WeaponBase[] weapons = Resources.LoadAll<WeaponBase>("FPSWeapons/");
        
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].IsDefaultWeapon){continue;}
            ItemSelection itemInstance = Instantiate(itemSelectionPrefab, content);
            itemInstance.SetTitleButton(weapons[i].gameObject.name,weapons[i].ItemAttribute.Icon,OwnedPlayer);
        }
    }
}
