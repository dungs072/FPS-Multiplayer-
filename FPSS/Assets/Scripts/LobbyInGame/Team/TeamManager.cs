using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class TeamManager : NetworkBehaviour
{
    [SerializeField] private GameLobby gameLobby;
    public static Dictionary<TeamName, List<Team>> Teams { get; } = new Dictionary<TeamName, List<Team>>();
    private void Awake()
    {
        Teams[TeamName.Swat] = new List<Team>();
        Teams[TeamName.Terrorist] = new List<Team>();
    }
    private void Start()
    {
        AddAllMemberIntoEveryTeam();
    }
    public void AddAllMemberIntoEveryTeam()
    {
        //System.Random rand = new System.Random();
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        for(int i =0;i<players.Count;i++)
        {
            if(players[i].isOwned)
            {
                TeamName teamName = i%2==0?TeamName.Swat:TeamName.Terrorist;
                Team team = players[i].GetComponent<Team>();
                team.SetTeamName(teamName);
                break;
            }
        }
        // while (true)
        // {
        //     int index = rand.Next(players.Count);
        //     if (players[index].GetComponent<Team>().TeamName != TeamName.None)
        //     {
        //         continue;
        //     }
        //     if (players[index].isOwned)
        //     {
        //         // players[index].GetComponent<Team>().SetTeamName(isContrast?TeamName.Swat:
        //         //                                                 TeamName.Terrorist);
        //         Team team = players[index].GetComponent<Team>();
        //         team.SetTeamName(TeamName.Swat,LobbyIndex);
        //         //AddMemberIntoTeam(players[index].GetComponent<Team>());
        //         LobbyIndex++;
        //         isContrast = !isContrast;
        //         break;
        //     }

        // }
    }
    private bool IsAllPlayerHasTeam(List<PlayerController> players)
    {
        foreach (var player in players)
        {
            if (player.GetComponent<Team>().TeamName == TeamName.None) { return false; }
        }
        return true;
    }
    public static void AddMemberIntoTeam(Team team)
    {
        Teams[team.TeamName].Add(team);
    }
    public void ClearAllTeams()
    {
        foreach (var team in Teams.Values)
        {
            team.Clear();
        }
    }

}
