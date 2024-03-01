using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;
public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject joinTable;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private Button joinButton;
    [SerializeField] private Transform joinLobbyContentParent;
    [SerializeField] private JoinLobbyItem joinLobbyItemPrefab;
    [SerializeField] private GameRuleManager gameRuleManager;
    [SerializeField] private MapManager mapManager;
    private List<JoinLobbyItem> listOfLobbies = new List<JoinLobbyItem>();
    public List<JoinLobbyItem> ListOfLobbies { get{return listOfLobbies;} }

    private void OnEnable() {
        MyNetworkManager.ClientOnConnected+=HandleClientConnected;
        MyNetworkManager.ClientOnDisconnected+=HandleClientDisconnected;
    }
    private void OnDisable() {
        MyNetworkManager.ClientOnConnected-=HandleClientConnected;
        MyNetworkManager.ClientOnDisconnected-=HandleClientDisconnected;
    }
    private void Start() {
        JoinLobbyItem.OnJoinLobby+=JoinLobby;
    }
    private void OnDestroy() {
        JoinLobbyItem.OnJoinLobby-=JoinLobby;
    }

    public void Join()
    {
        string address = addressInput.text;
        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();
        joinButton.interactable = false;
    }
    private void JoinLobby(CSteamID lobbyId, int typeGameIndex, int mapIndex)
    {
        SteamMatchmaking.JoinLobby(lobbyId);
        gameRuleManager.SetCurrentIndex(typeGameIndex);
        mapManager.ChangeSelectionMapImage(mapIndex);

    }
    private void HandleClientConnected()
    {
        joinTable.SetActive(false);
        joinButton.interactable = true;
    }
    private void HandleClientDisconnected()
    {
        joinTable.SetActive(true);
        joinButton.interactable = true;
    }
    public void DestroyOldJoinLobby()
    {
        foreach(var lobby in listOfLobbies)
        {
            Destroy(lobby.gameObject);
        }
        listOfLobbies.Clear();
    }
    public void DisplayLobbies(List<CSteamID> lobbyIDS, string search = "")
    {
        foreach(var lobbyId in lobbyIDS)
        {
            string name = SteamMatchmaking.GetLobbyData((CSteamID)lobbyId.m_SteamID,"fpssname");
            if(name==""){continue;}
            if(!name.Contains(search)){continue;}
            var typeGame = SteamMatchmaking.GetLobbyData((CSteamID)lobbyId.m_SteamID,"typegame");
            var mapName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyId.m_SteamID,"mapName");
            var typeGameIndex = SteamMatchmaking.GetLobbyData((CSteamID)lobbyId.m_SteamID,"typegameIndex");
            var mapIndex = SteamMatchmaking.GetLobbyData((CSteamID)lobbyId.m_SteamID,"mapGameIndex");
            var joinLobbyInstance = Instantiate(joinLobbyItemPrefab,joinLobbyContentParent);
            joinLobbyInstance.LobbyId = (CSteamID)lobbyId.m_SteamID;
            joinLobbyInstance.LobbyName = name;
            joinLobbyInstance.CurrentPlayer = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyId.m_SteamID);
            joinLobbyInstance.MaxPlayer = SteamMatchmaking.GetLobbyMemberLimit((CSteamID)lobbyId.m_SteamID);
            joinLobbyInstance.CurrentTypeGameIndex = int.Parse(typeGameIndex);
            joinLobbyInstance.CurrentMapIndex = int.Parse(mapIndex);
            joinLobbyInstance.SetLobbyName(joinLobbyInstance.LobbyName);
            joinLobbyInstance.SetCountPlayer(joinLobbyInstance.CurrentPlayer.ToString()+"/"+joinLobbyInstance.MaxPlayer.ToString());
            joinLobbyInstance.SetTypeGame(typeGame);
            joinLobbyInstance.SetMapName(mapName);
            listOfLobbies.Add(joinLobbyInstance);
        }
    }
}

