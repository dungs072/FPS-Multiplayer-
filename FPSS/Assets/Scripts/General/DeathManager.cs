using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
public class DeathManager : NetworkBehaviour
{
    [SerializeField] private GameObject fpsCameraObject;
    [SerializeField] private GameObject dieCameraObject;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private float timeToTurnOffFreeLookCamera = 1f;

    public override void OnStartLocalPlayer()
    {
        healthManager.OnDie+=TriggerDeathProcess;
    }
    private void OnDestroy() {
        if(!isLocalPlayer){return;}
        healthManager.OnDie-=TriggerDeathProcess;
    }
       
    public void TriggerDeathProcess()
    {
        fpsCameraObject.SetActive(false);
        dieCameraObject.SetActive(true);
        UIManager.Instance.ToggleDynamicCrossHair(true);
        StartCoroutine(TurnOffFreeLookCamera());
    }
    public void TriggerRespawnProcess()
    {
        dieCameraObject.SetActive(false);
        fpsCameraObject.SetActive(true);
        UIManager.Instance.ToggleDynamicCrossHair(false);
    }
    private IEnumerator TurnOffFreeLookCamera()
    {
        yield return new WaitForSeconds(timeToTurnOffFreeLookCamera);
        freeLookCamera.enabled = false;
    }
}
