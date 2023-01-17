using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class LobbyInGameUIManager : MonoBehaviour
{
    [SerializeField] private Transform[] startPoints;

    public void JoinMatch()
    {
        List<PlayerController> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        foreach (var player in players)
        {
            if (player.isOwned)
            {
                Team team = player.GetComponent<Team>();
                if (team.TeamName == TeamName.Swat)
                {
                    player.transform.position = startPoints[0].position;
                }
                else if(team.TeamName == TeamName.Terrorist)
                {
                    player.transform.position = startPoints[1].position;
                }
                StartCoroutine(SetStartPosition(player));
                player.GetComponent<Team>().ToggleLobbyCameras(false);
                return;
            }
        }
    }
    private IEnumerator SetStartPosition(PlayerController player)
    {
        yield return new WaitForSeconds(1f);
        player.SetInGameProgress(true);
    }
}
