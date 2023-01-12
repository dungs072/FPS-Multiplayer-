using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;
    public event Action OnAddPlayers;

    private bool isGameInProgress = false;
    public List<PlayerController> PlayersAuthority { get; } = new List<PlayerController>();
    public List<PlayerController> Players { get; } = new List<PlayerController>();
    #region Server
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (!isGameInProgress) { return; }
        conn.Disconnect();
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        PlayerController player = conn.identity.GetComponent<PlayerController>();
        Players.Remove(player);
        base.OnServerDisconnect(conn);

    }
    public override void OnStopServer()
    {
        Players.Clear();
        isGameInProgress = false;
    }
    public void StartGame()
    {
        //if(Players.Count<2){return;}
        isGameInProgress = true;
        ServerChangeScene("Map02");
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        PlayerController player = conn.identity.GetComponent<PlayerController>();
        Players.Add(player);
        player.SetPartyOwner(Players.Count == 1);
    }

    // public override void OnServerSceneChanged(string sceneName)
    // {
    //     base.OnServerSceneChanged()
    //     if (SceneManager.GetActiveScene().name.StartsWith("Map02"))
    //     {
    //         foreach (var player in Players)
    //         {
    //             player.SetIsInGameProgress();
    //         }
    //     }
    // }
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        if (SceneManager.GetActiveScene().name.StartsWith("Map02"))
        {
            foreach (var player in Players)
            {
                player.SetInGameProgress(true);
            }
        }
    }

    public void AddPlayers(PlayerController playerController)
    {
        PlayersAuthority.Add(playerController);
        OnAddPlayers.Invoke();
    }
    #endregion

    #region Client
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        ClientOnDisconnected?.Invoke();
    }
    public override void OnStopClient()
    {
        Players.Clear();
    }
    #endregion



}
