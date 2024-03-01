using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
public class ItemPurcharse : MonoBehaviour
{
    [SerializeField] private TMP_Text countPurchaseText;
    private PlayerController ownedPlayer;
    private WeaponManager weaponManager;

    public BoomType BoomType { get; set; }
    private void Start()
    {
        ownedPlayer = NetworkClient.connection.identity.GetComponent<PlayerController>();
        weaponManager = ownedPlayer.GetComponent<WeaponManager>();
    }
    private void OnEnable() {
        countPurchaseText.text = "2";
    }

    public void IncreaseItem(int amount)
    {
        int value = 0;
        if (BoomType == BoomType.GRENADE)
        {
            weaponManager.FragGrenade.GrenadeAmmount += amount;
            value = weaponManager.FragGrenade.GrenadeAmmount;
        }
        else if (BoomType == BoomType.SMOKE)
        {
            weaponManager.SmokeGrenade.GrenadeAmmount += amount;
            value = weaponManager.SmokeGrenade.GrenadeAmmount;
        }
        SetCountText(value.ToString());
    }
    public void DecreaseItem(int amount)
    {
        int value = 0;
        if (BoomType == BoomType.GRENADE)
        {
            weaponManager.FragGrenade.GrenadeAmmount -= amount;
            value = weaponManager.FragGrenade.GrenadeAmmount;
        }
        else if (BoomType == BoomType.SMOKE)
        {
            weaponManager.SmokeGrenade.GrenadeAmmount -= amount;
            value = weaponManager.SmokeGrenade.GrenadeAmmount;
        }
        SetCountText(value.ToString());
    }
    public void SetCountText(string text)
    {
        countPurchaseText.text = text;
    }
}
