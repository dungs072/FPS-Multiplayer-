using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class TeamManager : MonoBehaviour
{
    private static Dictionary<TeamName, List<Team>> teams = new Dictionary<TeamName, List<Team>>();
    private bool isContrast = false;
    private void Awake()
    {
        teams[TeamName.Swat] = new List<Team>();
        teams[TeamName.Terrorist] = new List<Team>();
    }
    private void Start() {
        AddAllMemberIntoEveryTeam();
    }
    public void AddAllMemberIntoEveryTeam()
    {
        System.Random rand = new System.Random();
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        while (true)
        {
            if(IsAllPlayerHasTeam(players)){break;}
            int index = rand.Next(players.Count);
            if (players[index].GetComponent<Team>().TeamName != TeamName.None)
            {
                continue;
            }
            players[index].GetComponent<Team>().SetTeamName(isContrast?TeamName.Swat:
                                                            TeamName.Terrorist);
            
            isContrast = !isContrast;
            AddMemberIntoTeam(players[index].GetComponent<Team>());
        }
    }
    private bool IsAllPlayerHasTeam(List<PlayerController> players)
    {
        foreach(var player in players)
        {
            if(player.GetComponent<Team>().TeamName==TeamName.None){return false;}
        }
        return true;
    }
    public static void AddMemberIntoTeam(Team team)
    {
        teams[team.TeamName].Add(team);
    }
    public void ClearAllTeams()
    {
        foreach (var team in teams.Values)
        {
            team.Clear();
        }
    }
}
