using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PiController : MonoBehaviour
{
    private PiUIManager piUi;
    [SerializeField] private string piMenuName = "WeaponMenu";
    private bool menuOpened;
    private PiUI normalMenu;
    private PlayerController playerController;
    // Use this for initialization
    void Start()
    {
        piUi = UIManager.Instance.PIUI;
        //Get menu for easy not repetitive getting of the menu when setting joystick input
        normalMenu = piUi.GetPiUIOf(piMenuName);
        playerController = GetComponent<PlayerController>();
        menuOpened = false;
        SetUpPiMenu();

    }
    private void SetUpPiMenu()
    {
        var piDatas = normalMenu.piData;
        var weapons = playerController.GetComponent<WeaponManager>().Weapons;
        for (int i = 0; i < piDatas.Length; i++)
        {
            if (i > weapons.Count - 1)
            {
                piDatas[i].sliceLabel = "";
                continue;
            }
            piDatas[i].icon = weapons[i].ItemAttribute.Icon;
            piDatas[i].disabledColor = Color.grey;
            piDatas[i].Index = i;
            piDatas[i].NameItem = weapons[i].ItemAttribute.Name;
            piDatas[i].sliceLabel = "";
            if (piDatas[i].OnCustomClick.GetPersistentEventCount()==0)
            {
                piDatas[i].OnCustomClick = new UnityEngine.Events.UnityEvent<int>();
                piDatas[i].OnCustomClick.AddListener(OnPressPiCut);
                piDatas[i].OnCustomHoverEnter = new UnityEngine.Events.UnityEvent<string>();
                piDatas[i].OnCustomHoverEnter.AddListener(OnHoverEnter);
                piDatas[i].onHoverExit = new UnityEngine.Events.UnityEvent();
                piDatas[i].onHoverExit.AddListener(OnHoverExitM);

            }
            if (weapons[i] is ShootWeaponBase)
            {
                var shootWeapon = weapons[i] as ShootWeaponBase;
                
                piDatas[i].isInteractable = !(shootWeapon.BulletLeft == 0 && shootWeapon.BulletLeftInMag == 0);

            }
            if(weapons[i] is GrenadeWeaponBase)
            {
                var grenadeWeapon = weapons[i] as GrenadeWeaponBase;
                piDatas[i].isInteractable = grenadeWeapon.GrenadeAmmount>0;
            }
            if(weapons[i] is MeleeWeaponBase)
            {
                piDatas[i].isInteractable = true;
            }
            if(playerController.GetComponent<WeaponManager>().CurrentWeapon==weapons[i])
            {
                piDatas[i].isInteractable = false;
                piDatas[i].disabledColor = Color.blue;
            }


        }
        piUi.UpdatePiMenu(piMenuName);
    }
    private void OnPressPiCut(int index)
    {
        var weaponManager = playerController.GetComponent<WeaponManager>();
        weaponManager.ChangeWeaponIndex(index);
        ChangeStatePi();
    }
    private void OnHoverEnter(string nameItem)
    {
        UIManager.Instance.SetNameItemInPi(nameItem);
    }
    private void OnHoverExitM()
    {
        UIManager.Instance.SetNameItemInPi("");
    }
    void Update()
    {
        if(!playerController.isOwned){return;}
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeStatePi();
        }
    }

    private void ChangeStatePi()
    {
        SetUpPiMenu();
        menuOpened = !menuOpened;
        playerController.ToggleControll(menuOpened);
        piUi.ChangeMenuState(piMenuName, new Vector2(3*Screen.width / 4f, Screen.height / 2f));
    }

    //Test function that writes to the console and also closes the menu
    public void TestFunction()
    {
        //Closes the menu
        piUi.ChangeMenuState(piMenuName);
        Debug.Log("You Clicked me!");
    }

    public void OnHoverEnter()
    {
        Debug.Log("Hey get off of me!");
    }
    public void OnHoverExit()
    {
        Debug.Log("That's right and dont come back!");
    }
}
