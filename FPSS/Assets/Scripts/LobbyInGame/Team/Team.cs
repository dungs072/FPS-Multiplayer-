using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public enum TeamName
{
    None,
    Terrorist,
    Swat
}
public class Team : NetworkBehaviour
{
    public static event Func<TeamName,Camera> OnGetLobbyCamera;
    public static event Action<bool> OnSetLobbyPosition;
    public static event Action<bool> OnToggleLobbyCameras;
    [field: SerializeField] public TeamName TeamName { get; private set; } = TeamName.None;
    [SerializeField] private ReferenceManager referManager;
    [SerializeField] private Transform xRotatetionObject;
    [SerializeField] private GameObject headItemSwat;
    [SerializeField] private RotateUI rotateUI;
    public Vector3 LobbyPosition { get; set; }

    private Quaternion defaultRotation;
    private Quaternion defaultXRotation;

    private void Start()
    {
        defaultRotation = transform.rotation;
        defaultXRotation = xRotatetionObject.rotation;
    }

    public void SetTeamName(TeamName teamName)
    {
        TeamName = teamName;
        if (teamName == TeamName.Swat)
        {
            headItemSwat.SetActive(true);
        }
        // i have to run twice CmdSetTeamName function. Because
        // the first for set position for player
        // the second for set TeamName for player
        CmdSetTeamName(teamName);
        StartCoroutine(DelaySetTeamName(teamName));
        //OnSetTeam?.Invoke(this,lobbyIndex);
    }
    private IEnumerator DelaySetTeamName(TeamName teamName)
    {
        yield return new WaitForSeconds(1f);
        CmdSetTeamName(teamName);
    }

    public void ToggleLobbyCameras(bool state)
    {
        OnToggleLobbyCameras?.Invoke(state);
    }
    public void ReturnToLobbyPosition()
    {
        transform.position = LobbyPosition;
        transform.rotation = defaultRotation;
        if (TeamName == TeamName.Swat)
        {
            OnSetLobbyPosition?.Invoke(true);
        }
        else if (TeamName == TeamName.Terrorist)
        {
            OnSetLobbyPosition?.Invoke(false);
        }
        GetComponent<PlayerController>().SetInGameProgress(false);
        GetComponent<NetworkPlayerInfor>().TogglePlayerNameCanvas(true);
        referManager.WeaponTPPManager.DisplayWeapons(true);
        referManager.TPPController.LocomotionValue = 0f;
        xRotatetionObject.rotation = defaultXRotation;
        ResetCursorMode();

    }
    private void ResetCursorMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #region Server

    [Command]
    private void CmdSetTeamName(TeamName teamName)
    {
        RpcSetTeamName(teamName);
    }
    #endregion

    #region Client
    [ClientRpc]
    private void RpcSetTeamName(TeamName teamName)
    {
        if(isOwned){return;}
        TeamName = teamName;
        if (teamName == TeamName.Swat)
        {
            headItemSwat.SetActive(true);
        }
    }
    [ClientRpc]
    public void RpcSetLobbyPosition(Vector3 position)
    {
        rotateUI.SetUpLobbyCamera(OnGetLobbyCamera?.Invoke(TeamName));
        if (!isOwned) { return; }
        transform.position = position;
        LobbyPosition = position;
        if (TeamName == TeamName.Swat)
        {
            OnSetLobbyPosition?.Invoke(true);
        }
        else if (TeamName == TeamName.Terrorist)
        {
            OnSetLobbyPosition?.Invoke(false);
        }
    }
    #endregion
}

