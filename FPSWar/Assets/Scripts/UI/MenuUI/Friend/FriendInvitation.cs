using UnityEngine;
using TMPro;
using Steamworks;

public class FriendInvitation : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    public CSteamID FriendSteamId{get;set;}
    public CSteamID LobbyId{get;set;}

    public void SetFriendName(string text)
    {
        friendNameText.text = text;
    }
    public void OnInviteFriend()
    {
        SteamMatchmaking.InviteUserToLobby(LobbyId, FriendSteamId);
    }
}
