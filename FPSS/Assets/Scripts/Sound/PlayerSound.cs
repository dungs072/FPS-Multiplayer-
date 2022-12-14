using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private AudioSource audioSource;
    
    private void Start() {
        if(!healthManager.isOwned){return;}
        healthManager.OnNearlyDie+=PlayHeartBeatSound;
        healthManager.OnDie+=StopHeartBeatSound;
    }
    private void OnDestroy() {
        if(!healthManager.isOwned){return;}
        healthManager.OnNearlyDie-=PlayHeartBeatSound;
        healthManager.OnDie-=StopHeartBeatSound;
    }
    private void PlayHeartBeatSound()
    {
        audioSource.Play();
    }
    private void StopHeartBeatSound()
    {
        audioSource.Stop();
    }
}
