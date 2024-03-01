using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;
using System;
using TMPro;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public static event Action<CSteamID> OnLobbyCreatedEvent;
    [SerializeField] private GameObject landingPagePanel;
    [SerializeField] private bool useSteam = false;
    [Header("Friends")]
    [SerializeField] private GameObject friendPage;
    [Header("User")]
    [SerializeField] private TMP_Text userName;
    [Header("Join lobby")]
    [SerializeField] private JoinLobbyMenu joinLobbyMenu;
    [Header("Lobby")]
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_InputField searchLobby;
    [SerializeField] private GameObject networkManager;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyMatchList_t> Callback_lobbyList;
    protected Callback<LobbyDataUpdate_t> Callback_lobbyInfo;
    private const string HostAddressKey = "HostAddress";
    public List<CSteamID> lobbyIDS = new List<CSteamID>();
    public static ulong MyLobbyId;
    private string lobbyName;
    private string typeGame;
    private string mapName;
    private int typeGameIndex;
    private int mapIndex;
    private void Start()
    {
        if (!useSteam)
        {
            PlayerPrefs.SetString("UserName", "No Name");
        }
        else
        {
            InitializeSteam();
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
            //Callback_lobbyInfo = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyInfo);
        }

        userName.text = PlayerPrefs.GetString("UserName");

    }
    private void InitializeSteam()
    {
        bool isSteamInitialized = SteamAPI.Init();
        if (!isSteamInitialized)
        {
            Debug.LogError("Failed to initialize Steam API.");
            return;
        }
        CSteamID steamID = SteamUser.GetSteamID();
        string displayName = SteamFriends.GetFriendPersonaName(steamID);
        PlayerPrefs.SetString("UserName", displayName);
        OptionMenu.PlayerName = displayName;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void HostLobby(ELobbyType eLobbyType, string lobbyName, 
                        string mapName, string typeGame,
                        int currentTypeIndex, int currentMapIndex)
    {
        landingPagePanel.SetActive(false);
        if (useSteam)
        {
            SteamMatchmaking.CreateLobby(eLobbyType, 8);
            this.lobbyName = lobbyName;
            this.mapName = mapName;
            this.typeGame = typeGame;
            typeGameIndex = currentTypeIndex;
            mapIndex = currentMapIndex;
            return;
        }
        NetworkManager.singleton.StartHost();

    }
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return;
        }
        NetworkManager.singleton.StartHost();
        var lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        MyLobbyId = callback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(
            lobbyId,
            HostAddressKey,
            SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(
            lobbyId,
            "fpssname",
            lobbyName
        );
        SteamMatchmaking.SetLobbyData(lobbyId,
            "typegame",
            typeGame
        );
        SteamMatchmaking.SetLobbyData(lobbyId,
            "mapName",
            mapName
        );
        SteamMatchmaking.SetLobbyData(lobbyId,
            "typegameIndex",
            typeGameIndex.ToString()
        );
        SteamMatchmaking.SetLobbyData(lobbyId,
            "mapGameIndex",
            mapIndex.ToString()
        );
        lobbyNameText.text = lobbyName;
        OnLobbyCreatedEvent?.Invoke(lobbyId);
    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {

        if (NetworkServer.active) { return; }
        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),
                            HostAddressKey);
        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();
        lobbyIDS.Clear();
        if (joinLobbyMenu.ListOfLobbies.Count > 0)
        {
            joinLobbyMenu.DestroyOldJoinLobby();
        }
        landingPagePanel.SetActive(false);
    }
    // join 
    public void GetListOfLobbies()
    {
        if (lobbyIDS.Count > 0)
        {
            lobbyIDS.Clear();
        }
        SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
        SteamAPICall_t try_getList = SteamMatchmaking.RequestLobbyList();
    }
    private void OnGetLobbiesList(LobbyMatchList_t result)
    {
        if (joinLobbyMenu.ListOfLobbies.Count > 0)
        {
            joinLobbyMenu.DestroyOldJoinLobby();
        }
        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDS.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
        joinLobbyMenu.DisplayLobbies(lobbyIDS);
    }
    public void OnSearchButton()
    {
        string searchText = searchLobby.text;
        if (joinLobbyMenu.ListOfLobbies.Count > 0)
        {
            joinLobbyMenu.DestroyOldJoinLobby();
        }
        joinLobbyMenu.DisplayLobbies(lobbyIDS,searchText);
    }
}
