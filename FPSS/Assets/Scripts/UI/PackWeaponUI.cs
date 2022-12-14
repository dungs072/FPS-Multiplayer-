using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class PackWeaponUI:MonoBehaviour
{
    [SerializeField] private Image weaponDisplay;
    [SerializeField] private Image bulletDisplay;
    [SerializeField] private TMP_Text bulletLeftDisplay;
    public void ChangeWeaponDisplay(Sprite gunIcon)
    {
        weaponDisplay.sprite = gunIcon;
    }
    public void ChangeBulletDisplay(Sprite bulletIcon)
    {
        bulletDisplay.sprite = bulletIcon;
    }
    public void ChangeBulletLeftAmountDisplay(int bulletLeftInMag, int bulletLeft)
    {
        bulletLeftDisplay.text = String.Format($"{bulletLeftInMag}/{bulletLeft}");
    }

}
