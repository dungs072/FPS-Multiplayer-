using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    private static string[] teamName = new string[3]{"None","Terrorist","Swat"};

    [SerializeField] private int teamCount = 2;
    private List<List<Team>> teamss = new List<List<Team>>();

    private void Awake() {
        for(int i =0;i<teamCount;i++)
        {
            teamss.Add(new List<Team>());
        }
    }
    public void AddMemberIntoTeam(Team team)
    {
        teamss[team.TeamNumber-1].Add(team);
    }
    public void ClearAllTeams()
    {
        for(int i =0;i<teamCount;i++)
        {
            teamss[i].Clear();
        }
    }
}
