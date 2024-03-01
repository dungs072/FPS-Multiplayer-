using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ReferenceManager : MonoBehaviour
{
    [field:SerializeField] public FirstPersonController FPSController { get; private set; }
    [field:SerializeField] public ThirdPersonController TPPController { get; private set; }

    [field:SerializeField] public WeaponManager WeaponManager { get; private set; }
    [field:SerializeField] public WeaponTPPManager WeaponTPPManager {get;private set;}
    [field:SerializeField] public HealthManager HealthManager{get;private set;}
    [field:SerializeField] public LeanManager LeanManager{get;private set;}
    [field:SerializeField] public NetworkPlayerManager NetworkPlayerManager {get;private set;}
    [field:SerializeField] public AudioSource AudioSource{get;private set;}
    [field:SerializeField] public EnergyManager EnergyManager{get;private set;}

    public float GetNormalizedTime(Animator animator, string animTag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);
        if (animator.IsInTransition(0) && nextInfo.IsTag(animTag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(animTag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
