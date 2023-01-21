using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button readyGameButton;
    [SerializeField] private PlayerLobby[] namePlayerTexts;

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
    }
    private void AuthorityHandlePartyOnwerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }
    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdStartGame();
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
        }
        else
        {
            NetworkManager.singleton.StopClient();
            SceneManager.LoadScene(0);
        }
    }
}
