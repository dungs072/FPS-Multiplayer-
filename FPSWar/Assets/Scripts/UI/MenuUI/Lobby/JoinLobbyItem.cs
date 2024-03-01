using Steamworks;
using UnityEngine;
using TMPro;
using System;

public class JoinLobbyItem : MonoBehaviour
{
    public static event Action<CSteamID,int,int> OnJoinLobby;
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_Text countPlayerText;
    [SerializeField] private TMP_Text mapNameText;
    [SerializeField] private TMP_Text typeGameText;
    public string LobbyName { get; set; }
    public int MaxPlayer { get; set; }
    public int CurrentPlayer { get; set; }
    public int CurrentMapIndex{get;set;}
    public int CurrentTypeGameIndex{get;set;}
    public CSteamID LobbyId { get; set; }
    public void SetLobbyName(string text)
    {
        lobbyNameText.text = text;
    }
    public void SetCountPlayer(string text)
    {
        countPlayerText.text = text;
    }
    public void SetMapName(string text)
    {
        mapNameText.text = text;
    }
    public void SetTypeGame(string text)
    {
        typeGameText.text = text;
    }
    public void OnClick()
    {
        OnJoinLobby?.Invoke(LobbyId, CurrentTypeGameIndex, CurrentMapIndex);
    }
}
