using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigManager : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint secondHandGrab;
    [SerializeField] private MultiAimConstraint handAim;
    [SerializeField] private MultiAimConstraint bodyAim;
    [SerializeField] private Rig rig;
    [SerializeField] private HealthManager healthManager;
    private float secondHandGrabWeightTarget = 1f;
    private float handAimWeightTarget = 1f;
    private float rigWeightTarget = 1f;
    private void Start() {
        healthManager.OnDie+=TurnOffRigWeight;
    }
    private void OnDestroy() {
        healthManager.OnDie-=TurnOffRigWeight;
    }
    private void Update()
    {
        secondHandGrab.weight = Mathf.Lerp(secondHandGrab.weight, secondHandGrabWeightTarget, Time.deltaTime*20f);
        rig.weight = Mathf.Lerp(rig.weight, rigWeightTarget, Time.deltaTime * 20f);
        handAim.weight = Mathf.Lerp(handAim.weight, handAimWeightTarget, Time.deltaTime * 20f);
    }
    private void TurnOffRigWeight()
    {
        rigWeightTarget = 0;
    }
    public void SetSecondHandGrabWeight(float value)
    {
        secondHandGrabWeightTarget = value;   
    }
    public void SetRigWeight(float value)
    {
        rigWeightTarget = value;
    }
    public void SetHandWeight(float value)
    {
        handAimWeightTarget = value;
    }

}
