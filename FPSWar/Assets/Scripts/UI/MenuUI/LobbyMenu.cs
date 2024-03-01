using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using Steamworks;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button readyGameButton;
    [SerializeField] private PlayerLobby[] namePlayerTexts;
    [SerializeField] private GameObject mapSelectionPage;
    [SerializeField] private NotificationControl notification;
    [SerializeField] private Button increaseDeathsButton;
    [SerializeField] private Button decreaseDeathButton;
    [SerializeField] private BackgroundManager bgManager;
    [SerializeField] private Button inviteFriendButton;
    [SerializeField] private Image backgroundLobbyImage;
    [SerializeField] private AudioListener audioListener;
    public static string CurrentMapName = "";

    private void Start()
    {
        MyNetworkManager.ClientOnConnected += HandleClientConnected;
        PlayerController.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOnwerStateUpdated;
        PlayerController.OnIsReadyInLobbyUpdated+=OnUpdateReadyStateInLobby;
        NetworkPlayerInfor.ClientOnInforUpdated += ClientHandleInforUpdated;
    }
    private void OnDestroy()
    {
        MyNetworkManager.ClientOnConnected -= HandleClientConnected;
        PlayerController.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOnwerStateUpdated;
        PlayerController.OnIsReadyInLobbyUpdated-=OnUpdateReadyStateInLobby;
        NetworkPlayerInfor.ClientOnInforUpdated -= ClientHandleInforUpdated;
    }
    private void ClientHandleInforUpdated(bool isClientOnly)
    {
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        for (int i = 0; i < players.Count; i++)
        {
            namePlayerTexts[i].SetPlayerName(players[i].GetComponent<NetworkPlayerInfor>().PlayerName);
            namePlayerTexts[i].ToggleReadyIcon(true);
        }
        for (int i = players.Count; i < namePlayerTexts.Length; i++)
        {
            namePlayerTexts[i].SetPlayerName("Waiting for player...");
            namePlayerTexts[i].ToggleReadyIcon(false);
        }
        //startGameButton.interactable = players.Count>=2;
        if (isClientOnly)
        {
            readyGameButton.gameObject.SetActive(true);
        }
    }
    private void OnUpdateReadyStateInLobby(bool state)
    {
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        for (int i = 0; i < players.Count; i++)
        {
            
            namePlayerTexts[i].ToggleReadyIcon(players[i].GetReadyInLobby());
        }
    }
    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
        backgroundLobbyImage.enabled = true;
        audioListener.enabled = false;
        bgManager.ChangeBackgroundImage(BGImageType.WaitingBG);
    }
    private void AuthorityHandlePartyOnwerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
        increaseDeathsButton.gameObject.SetActive(state);
        decreaseDeathButton.gameObject.SetActive(state);
        inviteFriendButton.gameObject.SetActive(state);
    }
    public void StartGame()
    {
        if(CurrentMapName=="")
        {
            notification.SetText("you must select the map to play");
            notification.gameObject.SetActive(true);
            return;
        }
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdStartGame(CurrentMapName);
    }
    public void ReadyGame()
    {
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdSetReadyInLobby();
    }
    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
            SteamMatchmaking.LeaveLobby(new CSteamID(MainMenu.MyLobbyId));
        }
        else
        {
            NetworkManager.singleton.StopClient();
            SceneManager.LoadScene(0);
        }
        backgroundLobbyImage.enabled = false;
    }


    public void OnClickChooseMap()
    {
        mapSelectionPage.SetActive(true);
    }
}
