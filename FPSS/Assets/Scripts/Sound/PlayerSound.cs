using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip reloadingHumanSaySound;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private AudioSource heartAudioSource;
    [SerializeField] private AudioSource mainAudioSource;
    
    private void Start() {
        if(!healthManager.isOwned){return;}
        healthManager.OnNearlyDie+=PlayHeartBeatSound;
        healthManager.OnDie+=StopHeartBeatSound;
        healthManager.OnRescue+=StopHeartBeatSound;
    }
    private void OnDestroy() {
        if(!healthManager.isOwned){return;}
        healthManager.OnNearlyDie-=PlayHeartBeatSound;
        healthManager.OnDie-=StopHeartBeatSound;
        healthManager.OnRescue-=StopHeartBeatSound;
    }
    private void PlayHeartBeatSound()
    {
        heartAudioSource.Play();
    }
    private void StopHeartBeatSound()
    {
        heartAudioSource.Stop();
    }
    public void PlayReloading()
    {
        mainAudioSource.PlayOneShot(reloadingHumanSaySound);
    }
}
