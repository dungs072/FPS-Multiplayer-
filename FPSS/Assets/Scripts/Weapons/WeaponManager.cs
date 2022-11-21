using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponBase[] weapons;


    public WeaponBase CurrentWeapon { get { return weapons[0]; } }

    public WeaponBase[] Weapons { get { return weapons; } }
}
