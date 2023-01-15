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
    [field: SerializeField] public TeamName TeamName { get; private set; } = TeamName.None;

    public void SetTeamName(TeamName teamName)
    {
        TeamName = teamName;
        if(!isOwned){return;}
        CmdSetTeamName(teamName);
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
        if (!isClientOnly) { return; }
        TeamName = teamName;
    }
    #endregion
}

