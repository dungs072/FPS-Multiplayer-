using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class GameLobby : NetworkBehaviour
{
    [Header("Swat")]
    [SerializeField] private GameObject swatCameraLobby;
    [SerializeField] private Transform[] lobbyPointsS;
    [Header("Terrorist")]
    [SerializeField] private GameObject terrorCameraLobby;
    [SerializeField] private Transform[] lobbyPointsT;
    private int sIndex = 0;
    private int tIndex = 0;
    private void Start()
    {
        Team.OnSetLobbyPosition+=ToggleSwatCamera;
        Team.OnToggleLobbyCameras+=ToggleBothCamera;
        Team.OnGetLobbyCamera+=GetLobbyCamera;
        if(!isServer){return;}
        Invoke(nameof(SetAllPlayerLobbyPosition), 1f);
    }
    private void OnDestroy() {
        Team.OnSetLobbyPosition-=ToggleSwatCamera;
        Team.OnToggleLobbyCameras-=ToggleBothCamera;
        Team.OnGetLobbyCamera-=GetLobbyCamera;
    }
    [Server]
    private void SetAllPlayerLobbyPosition()
    {
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        foreach (var player in players)
        {
            Team team = player.GetComponent<Team>();
            if (team.TeamName == TeamName.Swat)
            {
                team.RpcSetLobbyPosition(lobbyPointsS[sIndex++].position);
            }
            else if (team.TeamName == TeamName.Terrorist)
            {
                team.RpcSetLobbyPosition(lobbyPointsT[tIndex++].position);
            }
        }
    }

    private void ToggleSwatCamera(bool state)
    {
        terrorCameraLobby.SetActive(!state);
        swatCameraLobby.SetActive(state);
    }
    public void ToggleBothCamera(bool state)
    {
        terrorCameraLobby.SetActive(state);
        swatCameraLobby.SetActive(state);
    }
    private Camera GetLobbyCamera(TeamName teamName)
    {
        if(teamName==TeamName.Swat)
        {
            return swatCameraLobby.GetComponent<Camera>();
        }
        else if(teamName==TeamName.Terrorist)
        {
            return terrorCameraLobby.GetComponent<Camera>();
        }
        else return null;

    }

}
