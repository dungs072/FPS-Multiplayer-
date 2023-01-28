using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class ScoreManager : NetworkBehaviour
{
    
    [SerializeField] private int maxWinScore = 50;
    [SyncVar(hook = nameof(OnChangeTerroristScore))]
    private int terroristScore;
    [SyncVar(hook = nameof(OnChangeSwatScore))]
    private int swatScore;

    [SyncVar]
    private bool gameWin = false;
    private void Start()
    {

        if (!isServer) { return; }
        HealthManager.OnIncreasingScore += OnAddScore;
    }
    private void OnDestroy()
    {
        if (!isServer) { return; }
        HealthManager.OnIncreasingScore -= OnAddScore;
    }
    #region Server
    [Server]
    private void OnAddScore(TeamName teamName, int value)
    {
        if(gameWin){return;}
        if (teamName == TeamName.Terrorist)
        {
            AddSwatScore(value);
        }
        else if (teamName == TeamName.Swat)
        {
            AddTerroristScore(value);
        }
    }
    [Server]
    private void AddSwatScore(int value)
    {
        swatScore += value;
        HandleNewScore(swatScore,TeamName.Swat);
    }
    [Server]
    private void AddTerroristScore(int value)
    {
        terroristScore += value;
        HandleNewScore(terroristScore,TeamName.Terrorist);
    }
    [Server]
    private void HandleNewScore(int score, TeamName teamName)
    {
        if (score >= maxWinScore)
        {
            gameWin = true;
            CmdHandleGameWin(teamName);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdHandleGameWin(TeamName teamName)
    {
        RpcHandleGameWin(teamName);
    }
    #endregion
    #region Client
    private void OnChangeTerroristScore(int oldValue, int newValue)
    {
        UIManager.Instance.SetTerroristScoreDisplay(newValue);
    }
    private void OnChangeSwatScore(int oldValue, int newValue)
    {
        UIManager.Instance.SetSwatScoreDisplay(newValue);
    }
    [ClientRpc]
    private void RpcHandleGameWin(TeamName teamName)
    {
        ResultMatch.DisplayResultInMatch(teamName);
    }
    #endregion

}
