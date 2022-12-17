using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;
public class RigManager : NetworkBehaviour
{
    [SerializeField] private TwoBoneIKConstraint secondHandGrab;
    [SerializeField] private MultiAimConstraint handAim;
    [SerializeField] private MultiAimConstraint bodyAim;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private Animator animator;
    [SerializeField] private Rig rig;
    [SerializeField] private HealthManager healthManager;
    [Header("Source target")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform hint;
    [SerializeField] private Transform targetRL;
    [SerializeField] private Transform hintRL;
    [SyncVar]
    private float secondHandGrabWeightTarget = 1f;
    [SyncVar]
    private float handAimWeightTarget = 1f;
    [SyncVar]
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
    public void TurnOffRigWeight()
    {
        SetRigWeight(0f);
    }
    public void TurnOnRigWeight()
    {
        SetRigWeight(1f);
    }
    public void ChangeSecondHandGrabSourceTarget(int i = 0)
    {
        if(i==0)
        {
            secondHandGrab.data.target = target;
            secondHandGrab.data.hint = target;
        }
        else
        {
            secondHandGrab.data.target = targetRL;
            secondHandGrab.data.hint = hintRL;
        }
        Rebuild();
    }
    private void Rebuild()
    {
        rigBuilder.Build();
        //animator.Rebind();
    }
    public void SetSecondHandGrabWeight(float value)
    {
        secondHandGrabWeightTarget = value;
        if(!isOwned){return;}
        CmdSetSecondHandGrabWeight(value);   
    }
    public void SetRigWeight(float value)
    {
        rigWeightTarget = value;
        if(!isOwned){return;}
        CmdSetRigWeight(value);
    }
    public void SetHandWeight(float value)
    {
        handAimWeightTarget = value;
        if(!isOwned){return;}
        CmdSetHandWeight(value);
    }

    #region Server
    [Command]
    private void CmdSetRigWeight(float value)
    {
        rigWeightTarget = value;
    }
    [Command]
    private void CmdSetSecondHandGrabWeight(float value)
    {
        secondHandGrabWeightTarget = value;
    }
    [Command]
    private void CmdSetHandWeight(float value)
    {
        handAimWeightTarget = value;
    }
    #endregion

}
