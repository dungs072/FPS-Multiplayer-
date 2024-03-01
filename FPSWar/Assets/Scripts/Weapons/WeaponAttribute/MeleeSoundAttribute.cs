using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Attribute", menuName = "Attributes/Melee Sound", order = 1)]
public class MeleeSoundAttribute:ScriptableObject
{
    [SerializeField] private AudioClip obstacleHitAudio;
    [SerializeField] private AudioClip bodyHitAudio;
    [SerializeField] private AudioClip long_tail_audio;
    [SerializeField] private AudioClip ponkAudio;


    public AudioClip ObstacleHitAudio{get{return obstacleHitAudio;}}
    public AudioClip BodyHitAudio{get{return bodyHitAudio;}}
    public AudioClip LongTailAudio{get{return long_tail_audio;}}
    public AudioClip PonkAudio{get{return ponkAudio;}}
}
