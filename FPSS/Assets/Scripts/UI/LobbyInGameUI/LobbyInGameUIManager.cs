using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LobbyInGameUIManager : MonoBehaviour
{
    public static event Action OnOutLobbyInGame;
    [SerializeField] private Transform[] startPoints;
    [SerializeField] private WeaponSelectionUI weaponSelectionUI;
    [SerializeField] private GameObject lobbyObject;

    private void Awake()
    {
        weaponSelectionUI.OnGetPlayer += AfterFinishGetPlayer;
    }
    private void OnDestroy()
    {
        weaponSelectionUI.OnGetPlayer -= AfterFinishGetPlayer;
        OnOutLobbyInGame-=HandleOutLobbyInGame;
    }
    private void AfterFinishGetPlayer()
    {
        OnOutLobbyInGame+=HandleOutLobbyInGame;
        weaponSelectionUI.OwnedPlayer.GetComponent<RespawnManager>().OnRespawn+=TurnOnLobbyUI;
    }
    private void HandleOutLobbyInGame()
    {
        weaponSelectionUI.OwnedPlayer.OnOutLobbyInGame();
    }
    public void JoinMatch()
    {
        Team team = weaponSelectionUI.OwnedPlayer.GetComponent<Team>();
        if (team.TeamName == TeamName.Swat)
        {
            weaponSelectionUI.OwnedPlayer.transform.position = startPoints[0].position;
        }
        else if (team.TeamName == TeamName.Terrorist)
        {
            weaponSelectionUI.OwnedPlayer.transform.position = startPoints[1].position;
        }
        StartCoroutine(SetStartPosition(weaponSelectionUI.OwnedPlayer));
        weaponSelectionUI.OwnedPlayer.GetComponent<Team>().ToggleLobbyCameras(false);
        weaponSelectionUI.OwnedPlayer.GetComponent<WeaponTPPManager>().DisplayWeapons(false);
        
    }
    public void TurnOnLobbyUI()
    {
        lobbyObject.SetActive(true);
    }
    private IEnumerator SetStartPosition(PlayerController player)
    {
        yield return new WaitForSeconds(1f);
        player.SetInGameProgress(true);
        OnOutLobbyInGame?.Invoke();
    }
}
