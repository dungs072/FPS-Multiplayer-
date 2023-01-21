using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class ScoreManager : NetworkBehaviour
{
    [SyncVar(hook =nameof(OnChangeTerroristScore))]
    private int terroristScore;
    [SyncVar(hook = nameof(OnChangeSwatScore))]
    private int swatScore;
    private void Start() {
        if(!isServer){return;}
        HealthManager.OnIncreasingScore+=OnAddScore;
    }
    private void OnDestroy() {
        if(!isServer){return;}
        HealthManager.OnIncreasingScore-=OnAddScore;
    }
    [Server]
    private void OnAddScore(TeamName teamName, int value)
    {
        if(teamName == TeamName.Terrorist)
        {
            AddSwatScore(value);
        }
        else if(teamName==TeamName.Swat)
        {
            AddTerroristScore(value);
        }
    }
    [Server]
    private void AddSwatScore(int value)
    {
        swatScore+=value;
    }
    [Server]
    private void AddTerroristScore(int value)
    {
        terroristScore+=value;
    }
    // #region Server
    // [Command]
    // private void CmdSetTerroristScore(int value)
    // {
    //     terroristScore = value;
    // }
    // [Command]
    // private void CmdSetSwatScore(int value)
    // {
    //     swatScore = value;
    // }
    //#endregion
    #region Client
    private void OnChangeTerroristScore(int oldValue, int newValue)
    {
        UIManager.Instance.SetTerroristScoreDisplay(newValue);
    }
    private void OnChangeSwatScore(int oldValue, int newValue)
    {
        UIManager.Instance.SetSwatScoreDisplay(newValue);
    }
    #endregion

}
