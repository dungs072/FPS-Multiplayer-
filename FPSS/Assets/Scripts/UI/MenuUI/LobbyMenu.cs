using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text[] namePlayerTexts;

    private void Start()
    {
        MyNetworkManager.ClientOnConnected+=HandleClientConnected;
        PlayerController.AuthorityOnPartyOwnerStateUpdated +=AuthorityHandlePartyOnwerStateUpdated;
        NetworkPlayerInfor.ClientOnInforUpdated+=ClientHandleInforUpdated;
    }
    private void OnDestroy() {
        MyNetworkManager.ClientOnConnected-=HandleClientConnected;
        PlayerController.AuthorityOnPartyOwnerStateUpdated -=AuthorityHandlePartyOnwerStateUpdated;
        NetworkPlayerInfor.ClientOnInforUpdated-=ClientHandleInforUpdated;
    }
    private void ClientHandleInforUpdated()
    {
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        for(int i =0;i<players.Count;i++)
        {
            namePlayerTexts[i].text = players[i].GetComponent<NetworkPlayerInfor>().PlayerName;
        }
        for(int i=players.Count;i<namePlayerTexts.Length;i++)
        {
            namePlayerTexts[i].text = "Waiting for player...";
        }
        //startGameButton.interactable = players.Count>=2;
    }
    public void OnUpdateTextLobby(int index, string name)
    {
        if(index>=namePlayerTexts.Length){return;}
        namePlayerTexts[index].text = name;
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
    public void LeaveLobby()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
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
