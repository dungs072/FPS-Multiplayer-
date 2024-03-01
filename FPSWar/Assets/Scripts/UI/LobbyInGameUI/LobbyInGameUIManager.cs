using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class LobbyInGameUIManager : MonoBehaviour
{
    public static event Action OnOutLobbyInGame;
    [SerializeField] private WeaponSelectionUI weaponSelectionUI;
    [SerializeField] private GameObject lobbyObject;
    [SerializeField] private TMP_Text whichTeamText;
    [SerializeField] private TMP_Text joinMatchButtonText;
    [SerializeField] private float timeToJoinMatch;
    [Header("Spawn point Death match")]
    [SerializeField] private Transform[] startPointsSwatDeathMatch;
    [SerializeField] private Transform[] startPointTerroristDeathMatch;
    [Header("Spawn point Robber")]
    [SerializeField] private Transform[] startPointsSwatRobber;
    [SerializeField] private Transform[] startPointTerroristRobber;
    
    private float timerJoinMatch;
    private Coroutine joinMatchCoroutine;
    int currentStartPointIndex = 0;
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
        weaponSelectionUI.OwnedPlayer.GetComponent<RespawnManager>().OnRespawn += StartTimerToJoinMatch;
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
            joinMatchButtonText.text = "Join Match (" + timerJoinMatch.ToString("0") + ")";
            yield return null;
        }
        JoinMatch();

    }
    public void JoinMatch()
    {
        if (joinMatchCoroutine != null) { StopCoroutine(joinMatchCoroutine); }
        weaponSelectionUI.OwnedPlayer.SetCursorLock(true);
        weaponSelectionUI.OwnedPlayer.GetComponent<NetworkPlayerInfor>().TogglePlayerNameCanvas(false);
        weaponSelectionUI.ClearAllRings();
        Team team = weaponSelectionUI.OwnedPlayer.GetComponent<Team>();
        if (UIManager.Instance.GameRule == GameRule.Score)
        {
            if (team.TeamName == TeamName.Swat)
            {
                weaponSelectionUI.OwnedPlayer.transform.position = GetStartPointPosition(startPointsSwatDeathMatch);
            }
            else if (team.TeamName == TeamName.Terrorist)
            {
                weaponSelectionUI.OwnedPlayer.transform.position = GetStartPointPosition(startPointTerroristDeathMatch);
            }
        }
        else if (UIManager.Instance.GameRule == GameRule.Rob)
        {
            if (team.TeamName == TeamName.Swat)
            {
                weaponSelectionUI.OwnedPlayer.transform.position = GetStartPointPosition(startPointsSwatRobber);
            }
            else if (team.TeamName == TeamName.Terrorist)
            {
                weaponSelectionUI.OwnedPlayer.transform.position = GetStartPointPosition(startPointTerroristRobber);
            }
        }

        StartCoroutine(SetStartPosition(weaponSelectionUI.OwnedPlayer));
        weaponSelectionUI.OwnedPlayer.GetComponent<Team>().ToggleLobbyCameras(false);
        weaponSelectionUI.OwnedPlayer.GetComponent<WeaponTPPManager>().DisplayWeapons(false);
        lobbyObject.SetActive(false);
    }
    private Vector3 GetStartPointPosition(Transform[] positions)
    {
        currentStartPointIndex = (currentStartPointIndex+1)%positions.Length;
        return positions[currentStartPointIndex].position;
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
        Color color = Color.white;
        if (teamName == TeamName.Swat)
        {
            text = "You Are In Swat Team !";
            color = Color.blue;
        }
        else if (teamName == TeamName.Terrorist)
        {
            text = "You Are In Terrorist Team !";
            color = Color.red;
        }

        whichTeamText.text = text;
        whichTeamText.color = color;
    }
}
