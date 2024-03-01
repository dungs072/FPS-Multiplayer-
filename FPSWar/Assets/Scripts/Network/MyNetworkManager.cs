using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private RobberManager robberManager;
    [Header("Handling UI On Server")]
    [SerializeField] private MaxDeathsUI maxDeathsUI;
    [SerializeField] private GameRuleManager gameRuleManager;
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
        PlayerController serverPlayer = GetServerPlayer();
        if (serverPlayer == null) { return; }
        if (serverPlayer.TryGetComponent(out NetworkPlayerInfor playerInfor))//stupid code
        {
            playerInfor.UpdateDisplayPlayerInforList();
        }
        base.OnServerDisconnect(conn);

    }
    public PlayerController GetServerPlayer()
    {
        foreach (var player in Players)
        {
            if (player.isServer)
            {
                return player;
            }
        }
        return null;
    }
    public override void OnStopServer()
    {
        Players.Clear();
        isGameInProgress = false;
    }
    public void StartGame(string mapName)
    {
        //if(Players.Count<2){return;}
        foreach (var player in Players)
        {
            if (!player.GetReadyInLobby()) { return; }
        }
        isGameInProgress = true;
        ServerChangeScene(mapName);
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        PlayerController player = conn.identity.GetComponent<PlayerController>();
        Players.Add(player);
        player.SetPartyOwner(Players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        if (SceneManager.GetActiveScene().name.StartsWith("Map"))
        {
            TeamManager teamManagerInstance = Instantiate(teamManager);
            NetworkServer.Spawn(teamManagerInstance.gameObject);
            if (gameRuleManager.GameRule == GameRule.Score)
            {
                ScoreManager scoreManagerInstance = Instantiate(scoreManager);
                scoreManagerInstance.SetMaxWinScore(maxDeathsUI.GetMaxDeaths());

                NetworkServer.Spawn(scoreManagerInstance.gameObject);
            }
            else if(gameRuleManager.GameRule==GameRule.Rob)
            {
                RobberManager robberManagerInstance = Instantiate(robberManager);
                robberManagerInstance.SetPosition(Vector3.zero);
                NetworkServer.Spawn(robberManagerInstance.gameObject);
            }
            else if(gameRuleManager.GameRule==GameRule.Invade)
            {

            }

        }

    }
    public void AddPlayers(PlayerController playerController)
    {
        PlayersAuthority.Add(playerController);
        OnAddPlayers?.Invoke();
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
