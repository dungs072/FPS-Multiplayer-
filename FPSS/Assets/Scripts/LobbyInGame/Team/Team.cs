using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public enum TeamName
{
    None,
    Terrorist,
    Swat
}
public class Team : NetworkBehaviour
{
    public static event Action<bool> OnSetLobbyPosition;
    public static event Action<bool> OnToggleLobbyCameras;
    [field: SerializeField] public TeamName TeamName { get; private set; } = TeamName.None;
    public Vector3 LobbyPosition { get; set; }

    public void SetTeamName(TeamName teamName)
    {
        TeamName = teamName;
        CmdSetTeamName(teamName);
        //OnSetTeam?.Invoke(this,lobbyIndex);
    }
    public void ToggleLobbyCameras(bool state)
    {
        OnToggleLobbyCameras?.Invoke(state);
    }

    #region Server

    [Command]
    private void CmdSetTeamName(TeamName teamName)
    {
        RpcSetTeamName(teamName);
    }
    #endregion

    #region Client
    [ClientRpc]
    private void RpcSetTeamName(TeamName teamName)
    {
        TeamName = teamName;
    }
    [ClientRpc]
    public void RpcSetLobbyPosition(Vector3 position)
    {
        if(!isOwned){return;}
        transform.position = position;
        LobbyPosition = position;
        if (TeamName == TeamName.Swat)
        {
            OnSetLobbyPosition?.Invoke(true);
        }
        else if (TeamName == TeamName.Terrorist)
        {
            OnSetLobbyPosition?.Invoke(false);
        }
    }
    #endregion
}

