using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Steamworks;
public class PreHost : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private Toggle friendOnlyToggle;
    [SerializeField] private TMP_Dropdown typeGameDropDown;
    [SerializeField] private GameObject mapChoicePanel;
    [SerializeField] private NotificationControl notification;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject preHostPanel;
    [SerializeField] private GameRuleManager gameRuleManager;

    public string LobbyName{get{return lobbyNameInputField.text;}}
    public bool IsFriendOnly{get{return friendOnlyToggle.isOn;}}
    public int RulegameIndex{get{return typeGameDropDown.value;}}

    private void Start() 
    {
        ResetToDefaultHost();
    }
    public void OnStartLobbyClick()
    {
        if(mapManager.IsEmptyMap())
        {
            notification.SetText("you must select the map to play");
            notification.gameObject.SetActive(true);
            return;
        }
        preHostPanel.SetActive(false);
        gameRuleManager.ChangeGameRule(RulegameIndex);
        ELobbyType eLobbyType = IsFriendOnly?ELobbyType.k_ELobbyTypeFriendsOnly:ELobbyType.k_ELobbyTypePublic;
        mainMenu.HostLobby(eLobbyType, LobbyName, mapManager.GetMapName(), 
                            gameRuleManager.GameRuleName,
                            gameRuleManager.GetCurrentIndex(),
                            mapManager.GetCurrentMapIndex());
        
        ResetToDefaultHost();
    }
    public void OnChooseMapClick()
    {
        mapChoicePanel.SetActive(true);
    }
    private void ResetToDefaultHost()
    {
        lobbyNameInputField.text = PlayerPrefs.GetString("UserName") + "'s lobby";
        friendOnlyToggle.isOn = false;
        typeGameDropDown.value = 0;
        mapManager.ResetMap();
    }

}
