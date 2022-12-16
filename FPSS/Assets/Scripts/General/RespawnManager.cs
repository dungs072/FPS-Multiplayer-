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
    [SerializeField] private float timeToRespawn;
    public override void OnStartAuthority()
    {
        healthManager.OnDie+=HandleRespawn;
        OnRespawn+=healthManager.Respawn;
        OnRespawn+=deathManager.TriggerRespawnProcess;
    }
    private void Start() {
        OnRespawn+=playerController.OnRespawn;
        OnRespawn+=ragdollManager.TurnOffRagdoll;
    }
    private void OnDestroy() {
        OnRespawn-=playerController.OnRespawn;
        OnRespawn-=ragdollManager.TurnOffRagdoll;
        if(!isOwned){return;}
        healthManager.OnDie-=HandleRespawn;
        OnRespawn-=healthManager.Respawn;
        OnRespawn-=deathManager.TriggerRespawnProcess;
       
    }
    public void HandleRespawn()
    {
        StartCoroutine(CountDownTime());
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
        ui.ToggleRespawnUI(false);
    }

}
