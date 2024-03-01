using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;
using UnityEngine.TestTools;
using Unity.Burst.CompilerServices;
public class NetworkPlayerInfor : NetworkBehaviour
{
    public static event Action<bool> ClientOnInforUpdated; 
    [Header("UI")]
    [SerializeField] private GameObject playerNameCanvas;
    [SerializeField] private TMP_Text playerNameText;
    [SyncVar(hook =nameof(OnUpdatePlayerName))]
    private string playerName;

    public int KillNumber{get{return killNumber;}}

    [SyncVar]
    private int killNumber = 0;

    public string PlayerName{get{return playerName;}}
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(OptionMenu.PlayerName);
        TogglePlayerNameCanvas(true);
    }

    public void UpdateDisplayPlayerInforList()
    {
        CmdUpdateDisplayNameInfor();
    }

    public void TogglePlayerNameCanvas(bool state)
    {
        if(!isOwned){return;}
        CmdTogglePlayerNameCanvas(state);
    }
    #region Server
    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
    }
    [Command]
    private void CmdTogglePlayerNameCanvas(bool state)
    {
        RpcTogglePlayerNameCanvas(state);
    }
    [Command]
    public void CmdAddKillNumber(int value)
    {
        killNumber+=value;
    }
    [Command]
    private void CmdUpdateDisplayNameInfor()
    {
        RpcUpdateDisplayNameInfor();
    }
    #endregion

    #region Client
    [ClientRpc]
    private void RpcTogglePlayerNameCanvas(bool state)
    {
        playerNameCanvas.SetActive(state);
    }
    [ClientRpc]
    private void RpcUpdateDisplayNameInfor()
    {
        ClientOnInforUpdated?.Invoke(false);
    }
    private void OnUpdatePlayerName(string oldName, string newName)
    {
        ClientOnInforUpdated?.Invoke(isClientOnly);
        playerNameText.text = newName;
    }

    #endregion
}
