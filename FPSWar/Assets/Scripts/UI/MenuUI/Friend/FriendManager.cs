using UnityEngine;
using Steamworks;
using System.Collections.Generic;
using System;
using TMPro;
using System.Collections;

public class FriendManager : MonoBehaviour
{
    [SerializeField] private GameObject friendPage;
    [SerializeField] private FriendInvitation FriendInvitationPrefab;
    [SerializeField] private GameObject requirementPage;
    [SerializeField] private TMP_Text senderContextText;
    [SerializeField] private float maxTimeRequirementDisappear;
    public List<FriendInvitation> Friends { get; private set; } = new List<FriendInvitation>();

    private Callback<LobbyInvite_t> lobbyInviteCallback;
    private CSteamID currentLobbyId;
    private float currentTime=0f;

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized.");
            return;
        }
        MainMenu.OnLobbyCreatedEvent += OnLobbyCreated;
        lobbyInviteCallback = Callback<LobbyInvite_t>.Create(OnLobbyInvite);


    }
    private void OnDestroy()
    {
        MainMenu.OnLobbyCreatedEvent -= OnLobbyCreated;
    }
    private void OnLobbyCreated(CSteamID lobbyId)
    {
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        for (int i = 0; i < friendCount; i++)
        {
            CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);
            var friendInstance = Instantiate(FriendInvitationPrefab, friendPage.transform);
            friendInstance.SetFriendName(friendName);
            friendInstance.FriendSteamId = friendSteamID;
            friendInstance.LobbyId = lobbyId;
        }
    }
    private void OnLobbyInvite(LobbyInvite_t callback)
    {
        CSteamID inviterSteamID = new CSteamID(callback.m_ulSteamIDUser);
        CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
        requirementPage.gameObject.SetActive(true);
        senderContextText.text = "Received invitation from " + inviterSteamID + " to join lobby " + lobbyID;
        currentLobbyId = lobbyID;
        currentTime=0f;
        StartCoroutine(CountRequirementAppear());
        
    }
    private IEnumerator CountRequirementAppear()
    {
        yield return new WaitForSeconds(maxTimeRequirementDisappear);
        OnReject();
    }

    public void OnAccept()
    {
        SteamMatchmaking.JoinLobby(currentLobbyId);
        requirementPage.gameObject.SetActive(false);
    }

    public void OnReject()
    {
        requirementPage.gameObject.SetActive(false);
    }
}
