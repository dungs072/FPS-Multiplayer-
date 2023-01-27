using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class LobbyInGameUIManager : MonoBehaviour
{
    public static event Action OnOutLobbyInGame;
    [SerializeField] private Transform[] startPoints;
    [SerializeField] private WeaponSelectionUI weaponSelectionUI;
    [SerializeField] private GameObject lobbyObject;
    [SerializeField] private TMP_Text whichTeamText;
    [SerializeField] private TMP_Text joinMatchButtonText;
    [SerializeField] private float timeToJoinMatch;
    private float timerJoinMatch;
    private Coroutine joinMatchCoroutine;
    private void Awake()
    {
        weaponSelectionUI.OnGetPlayer += AfterFinishGetPlayer;
    }
    private void OnDestroy()
    {
        weaponSelectionUI.OnGetPlayer -= AfterFinishGetPlayer;
        OnOutLobbyInGame -= HandleOutLobbyInGame;
    }
    private void AfterFinishGetPlayer()
    {
        OnOutLobbyInGame += HandleOutLobbyInGame;

        weaponSelectionUI.OwnedPlayer.GetComponent<RespawnManager>().OnRespawn += TurnOnLobbyUI;
        weaponSelectionUI.OwnedPlayer.GetComponent<RespawnManager>().OnRespawn+=StartTimerToJoinMatch;
        StartCoroutine(DelaySetWhichTeamText());
    }
    private IEnumerator DelaySetWhichTeamText()
    {
        yield return new WaitForSeconds(1f);
        SetWhichTeamText(weaponSelectionUI.OwnedPlayer.GetComponent<Team>().TeamName);
        StartTimerToJoinMatch();
    }
    private void HandleOutLobbyInGame()
    {
        weaponSelectionUI.OwnedPlayer.OnOutLobbyInGame();
    }
    public void StartTimerToJoinMatch()
    {
        timerJoinMatch = timeToJoinMatch;
        joinMatchCoroutine = StartCoroutine(CountDownTimeToJoinMatch());
    }
    private IEnumerator CountDownTimeToJoinMatch()
    {
        while (timerJoinMatch > 0f)
        {
            timerJoinMatch -= Time.deltaTime;
            joinMatchButtonText.text = "Join Match (" + timerJoinMatch.ToString("0")+")";
            yield return null;
        }
        JoinMatch();

    }
    public void JoinMatch()
    {
        if(joinMatchCoroutine!=null){StopCoroutine(joinMatchCoroutine);}
        weaponSelectionUI.TurnOnWeaponponSelection();
        weaponSelectionUI.OwnedPlayer.GetComponent<NetworkPlayerInfor>().TogglePlayerNameCanvas(false);
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
        lobbyObject.SetActive(false);
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
    private void SetWhichTeamText(TeamName teamName)
    {
        string text = "";
        if (teamName == TeamName.Swat)
        {
            text = "You Are In Swat Team !";
        }
        else if (teamName == TeamName.Terrorist)
        {
            text = "You Are In Terrorist Team !";
        }
        whichTeamText.text = text;
    }
}
