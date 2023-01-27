using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class RespawnManager : NetworkBehaviour
{
    public event Action OnRespawn;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private RagdollManager ragdollManager;
    [SerializeField] private DeathManager deathManager;
    [SerializeField] private RigManager rigManager;
    [SerializeField] private float timeToRespawn;
    private Coroutine countDownCoroutine;
    public override void OnStartAuthority()
    {
        healthManager.OnDie+=HandleRespawn;
        OnRespawn+=healthManager.Respawn;
        OnRespawn+=deathManager.TriggerRespawnProcess;
    }
    private void Start() {
        // OnRespawn+=playerController.OnRespawn;
        OnRespawn+=ragdollManager.TurnOffRagdoll;
        OnRespawn+=rigManager.TurnOnRigWeight;
    }
    private void OnDestroy() {
        // OnRespawn-=playerController.OnRespawn;
        OnRespawn-=ragdollManager.TurnOffRagdoll;
        OnRespawn-=rigManager.TurnOnRigWeight;
        if(!isOwned){return;}
        healthManager.OnDie-=HandleRespawn;
        OnRespawn-=healthManager.Respawn;
        OnRespawn-=deathManager.TriggerRespawnProcess;
       
    }
    public void HandleRespawn()
    {
        if(countDownCoroutine!=null){StopCoroutine(countDownCoroutine);}
        countDownCoroutine =  StartCoroutine(CountDownTime());
    }
    private IEnumerator CountDownTime()
    {
        float time = timeToRespawn;
        UIManager ui = UIManager.Instance;
        ui.ToggleRespawnUI(true);
        while(time>0f)
        {
            time -= Time.deltaTime;
            yield return null;
            ui.UpdateRespawnUI(time);
        }
        OnRespawn?.Invoke();
        GetComponent<NetworkPlayerInfor>().TogglePlayerNameCanvas(true);
        if(isOwned){CmdRespawn();}   
        ui.ToggleRespawnUI(false);
    }

    [Command]
    private void CmdRespawn()
    {
        RpcRespawn();
    }
    [ClientRpc]
    private void RpcRespawn()
    {
        if(isOwned){return;}
        OnRespawn?.Invoke();
    }

}
