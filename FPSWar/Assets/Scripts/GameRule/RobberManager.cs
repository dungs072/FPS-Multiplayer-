using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RobberManager : NetworkBehaviour
{
    [Header("Time (mins)")]
    [SerializeField] private int TimeInMatch = 15;

    [SerializeField] private GameObject robbingObject;
    [SerializeField] private float rateIncrease = 0.1f;
    [SerializeField] private float rateDecrease = 0.1f;
    [SerializeField] private float timeToStartDecrease = 5f;

    private Vector3 positionToRobbingObject;

    [SyncVar(hook = nameof(OnIsMatchWithAllConditionChange))]
    private bool isMatchWithAllCondition;

    private Coroutine fillCoroutine;
    private float fillValue = 0f;
    private int currentMinute;
    private int currentSecond;
    private float secondValue = 0;

    private void Start()
    {
        currentMinute = TimeInMatch - 1;
        currentSecond = 59;
        fillValue = 0f;
        if (!isServer) { return; }
        StartCoroutine(InitializeGame());
    }
    private void Update()
    {
        secondValue += Time.deltaTime;
        if (secondValue >= 1f)
        {
            ChangeTimeInGame();
            secondValue = 0f;
        }

    }
    private void ChangeTimeInGame()
    {
        currentSecond--;
        if (currentSecond == -1)
        {
            currentMinute--;
            currentSecond = 59;
        }
        if (isServer)
        {
            if (currentMinute <= 0 && currentSecond <= 0)
            {
                CmdHandleGameWin(TeamName.Swat);
            }
        }

        UIManager.Instance.ChangeTime(currentMinute, currentSecond);
    }
    public override void OnStartServer()
    {
        PlayerController.OnMatchToRob += SetIsMatch;
    }
    public override void OnStopServer()
    {
        PlayerController.OnMatchToRob -= SetIsMatch;
    }
    private IEnumerator InitializeGame()
    {
        yield return new WaitForSeconds(2f);
        var robbingObjectInstance = Instantiate(robbingObject, positionToRobbingObject, Quaternion.identity);
        NetworkServer.Spawn(robbingObjectInstance);
    }
    public void SetPosition(Vector3 pos)
    {
        positionToRobbingObject = pos;
    }
    public void StartIncreaseFill()
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }

        fillCoroutine = StartCoroutine(IncreaseFill());

    }
    private IEnumerator IncreaseFill()
    {
        while (fillValue < 1f)
        {
            fillValue += Time.deltaTime * rateIncrease;
            UIManager.Instance.ChangeFillImageValue(fillValue);
            yield return null;
        }
        if (isServer)
        {
            if (fillValue >= 1f)
            {
                CmdHandleGameWin(TeamName.Terrorist);
            }
        }

        fillValue = 1f;
    }
    public void StartDecreaseFill()
    {

        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        fillCoroutine = StartCoroutine(Decrease());

    }
    private IEnumerator Decrease()
    {
        yield return new WaitForSeconds(timeToStartDecrease);
        while (fillValue > 0f)
        {
            fillValue -= Time.deltaTime * rateDecrease;
            UIManager.Instance.ChangeFillImageValue(fillValue);
            yield return null;
        }
        fillValue = 0f;
    }
    private void SetIsMatch(bool state)
    {
        SetIsMatchWithAllCondition(state);
    }
    #region Server
    [Server]
    private void SetIsMatchWithAllCondition(bool value)
    {
        isMatchWithAllCondition = value;
    }

    #endregion
    #region Client
    private void OnIsMatchWithAllConditionChange(bool oldState, bool newState)
    {
        if (newState)
        {
            StartIncreaseFill();
        }
        else
        {
            StartDecreaseFill();
        }
    }
    [Server]
    private void CmdHandleGameWin(TeamName teamName)
    {
        RpcHandleGameWin(teamName);
    }
    #endregion

    #region Client
    [ClientRpc]
    private void RpcHandleGameWin(TeamName teamName)
    {
        ResultMatch.DisplayResultInMatch(teamName);
    }
    #endregion

}
