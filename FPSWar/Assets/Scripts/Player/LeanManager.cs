using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class LeanManager : NetworkBehaviour
{
    [SerializeField] private float leanAngle = 45;
    [SerializeField] private float speedRotate = 5f;
    [Header("FPS")]
    [SerializeField] private Transform fpsSpine;
    [Header("TPP")]
    [SerializeField] private Transform spine;
  
    private Vector3 straightTPPRotation;
    private Vector3 straightFPSRotation;

    [SyncVar]
    private float targetAngle=0f;
    private float previousTargetAngle = 0f;
    private void Start()
    {
        SetUpFPS();
        SetUpTPP();
    }
    private void SetUpTPP()
    {
        float x = spine.localEulerAngles.x;
        float y = spine.localEulerAngles.y;
        float z = spine.localEulerAngles.z;
        straightTPPRotation = new Vector3(x,y,z);
    }
    private void SetUpFPS()
    {
        float x = fpsSpine.localEulerAngles.x;
        float y = fpsSpine.localEulerAngles.y;
        float z = fpsSpine.localEulerAngles.z;
        straightFPSRotation = new Vector3(x,y,z);
    } 
    private void Update()
    {
        if(previousTargetAngle==targetAngle){return;}
        previousTargetAngle = Mathf.Lerp(previousTargetAngle,targetAngle,speedRotate*Time.deltaTime);
        AddAngleToFPSSpine(previousTargetAngle);
        AddAngleToTPPSpine(previousTargetAngle);
        if(Mathf.Abs(previousTargetAngle-targetAngle)<0.01f)
        {
            previousTargetAngle = targetAngle;
        }
    }
    public void LeanLeft()
    {
        targetAngle = -leanAngle;
        CmdSetTargetAngle(targetAngle);
        
    }
    public void LeanRight()
    {
        targetAngle = leanAngle;
        CmdSetTargetAngle(targetAngle);
    }
    public void StandStraight()
    {
        targetAngle = 0f;
        CmdSetTargetAngle(targetAngle);
    }
    private void AddAngleToTPPSpine(float angle)
    {
        spine.transform.localRotation = Quaternion.Euler(straightTPPRotation.x+angle,
                                        straightTPPRotation.y,straightTPPRotation.z);
    }
    private void AddAngleToFPSSpine(float angle)
    {
        fpsSpine.transform.localRotation = Quaternion.Euler(straightFPSRotation.x,
                                        straightFPSRotation.y,straightFPSRotation.z-angle);
    }

    [Command]
    private void CmdSetTargetAngle(float angle)
    {
        targetAngle = angle;
    }

}
