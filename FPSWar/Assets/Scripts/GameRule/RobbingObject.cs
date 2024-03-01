using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbingObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Team>(out Team team))
        {
            if (team.isOwned)
            {
                if (team.TeamName == TeamName.Terrorist)
                {
                    UIManager.Instance.ToggleFButtonUIRobberManager(true);
                    UIManager.Instance.SetTitleRobber("Hold this to win");
                    var player = team.GetComponent<PlayerController>();
                    if (player.isOwned)
                    {
                        player.CanRob = true;
                    }
                }
                else if (team.TeamName == TeamName.Swat)
                {
                    UIManager.Instance.ToggleFButtonUIRobberManager(true, false);
                    UIManager.Instance.SetTitleRobber("Protect it from terrorist at all cost");
                }

            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Team>(out Team team))
        {
            if (team.isOwned)
            {
                UIManager.Instance.ToggleFButtonUIRobberManager(false);
                var player = team.GetComponent<PlayerController>();
                if (player.isOwned)
                {
                    player.CanRob = false;
                }
            }
        }
    }
}
