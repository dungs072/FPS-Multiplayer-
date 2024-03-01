using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsSFX", menuName = "SFX", order = 1)]
public class AssetsSFX : ScriptableObject
{
    [Header("Sprite")]
    [SerializeField] private Sprite normalButton;
    [SerializeField] private Sprite hoverButton;
    [SerializeField] private Sprite pressButton;
    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    
    public Sprite NormalButton{get{return normalButton;}}
    public Sprite HoverButton{get{return hoverButton;}}
    public Sprite PressButton{get{return pressButton;}}

    public AudioClip HoverSound{get{return hoverSound;}}
    public AudioClip ClickSound{get{return clickSound;}}
}
