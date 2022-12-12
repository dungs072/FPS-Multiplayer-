using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField] private TeamAttribute teamAttribute;
    public int TeamNumber{get{return TeamNumber;}}
    public bool IsSameTeam(int otherTeamNumber)
    {
        return teamAttribute.TeamNumber ==otherTeamNumber;
    }
    
    
}
[Serializable]
public class TeamAttribute
{
    [SerializeField][Range(1,2)] private int teamNumber = 1;
    public int TeamNumber{get{return teamNumber;}}
}
