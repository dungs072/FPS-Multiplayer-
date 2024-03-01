using InfimaGames.LowPolyShooterPack;
using UnityEngine;
[CreateAssetMenu(fileName = "Item Attribute", menuName = "Attributes/Melee Effect", order = 1)]
public class MeleeEffectAttribute : ScriptableObject
{
    [Header("Effect")]
    [SerializeField] private GameObject concreteEffect;
    [SerializeField] private GameObject metalEffect;
    [SerializeField] private GameObject woodEffectWithoutDecal;
    [SerializeField] private GameObject bloodEffectWithoutDecal;
    public GameObject WoodEffectWithoutDecal{get{return woodEffectWithoutDecal;}}
    public GameObject BloodEffectWithoutDecal{get{return bloodEffectWithoutDecal;}}
}

