using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class NetworkPlayerInfor : NetworkBehaviour
{
    public static event Action ClientOnInforUpdated; 
    [SyncVar(hook =nameof(OnUpdatePlayerName))]
    private string playerName;

    public string PlayerName{get{return playerName;}}
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(PlayerInfor.Instance.PlayerName);
    }
    #region Server
    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
    }
    #endregion

    #region Client
    private void OnUpdatePlayerName(string oldName, string newName)
    {
        ClientOnInforUpdated?.Invoke();
    }
    #endregion
}
