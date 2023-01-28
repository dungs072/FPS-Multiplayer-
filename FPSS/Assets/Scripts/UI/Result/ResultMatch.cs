using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using System;
public class ResultMatch : MonoBehaviour
{
    public static event Action<TeamName> OnTeamWin;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text titleResult;
    [Header("Swat")]
    
    [SerializeField] private ResultPlayerUI[] resultPlayersSwat;

    [Header("Terror")]
    [SerializeField] private ResultPlayerUI[] resultPlayersTerror;
    private void Start() {
        OnTeamWin+=DisplayResultMatch;
    }
    private void OnDestroy() {
        OnTeamWin-=DisplayResultMatch;
    }
    private void DisplayResultMatch(TeamName teamName)
    {
        resultPanel.SetActive(true);
        int swatIndex = 0;
        int terrorIndex = 0;
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        PlayerController ownedPlayer = NetworkClient.connection.identity.GetComponent<PlayerController>();
        ownedPlayer.GetComponent<RespawnManager>().StopRespawnCoroutine();
        ownedPlayer.HandleResultInMatch();
        if(ownedPlayer.GetComponent<Team>().TeamName==teamName)
        {
            titleResult.text = "Victory";
        }
        else
        {
            titleResult.text = "Defeat";
        }
        foreach(var player in players)
        {
            NetworkPlayerInfor infor = player.GetComponent<NetworkPlayerInfor>();
            if(player.GetComponent<Team>().TeamName==TeamName.Swat)
            {
                resultPlayersSwat[swatIndex++].SetNameAndKill(infor.PlayerName,infor.KillNumber);
            }
            else if(player.GetComponent<Team>().TeamName==TeamName.Terrorist)
            {
                resultPlayersTerror[terrorIndex++].SetNameAndKill(infor.PlayerName,infor.KillNumber);
            }
        }
    }
    public static void DisplayResultInMatch(TeamName teamName)
    {
        OnTeamWin?.Invoke(teamName);
    }

}
