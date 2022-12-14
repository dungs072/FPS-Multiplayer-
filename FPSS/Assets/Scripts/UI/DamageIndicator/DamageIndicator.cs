using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float MaxTimer = 5.0f;
    private float timer = MaxTimer;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if(canvasGroup==null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if(canvasGroup==null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }
    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if(rect==null)
            {
                rect = GetComponent<RectTransform>();
                if(rect==null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    public Transform Target{get;private set;} = null;
    private Transform player = null;
    private IEnumerator countDownCoroutine = null;
    private Action unRegister = null;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    public void Register(Transform target, Transform player,Action unRegister)
    {
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;
        StartCoroutine(RotateToTarget());
        StartTimer();
    }
    private void StartTimer()
    {
        if(countDownCoroutine!=null)
        {
            StopCoroutine(countDownCoroutine);
        }
        countDownCoroutine = CountDown();
        StartCoroutine(countDownCoroutine);
    }
    public void Restart()
    {
        timer = MaxTimer;
        StartTimer();
    }
    private IEnumerator CountDown()
    {
        while(CanvasGroup.alpha<1.0f)
        {
            CanvasGroup.alpha+=4*Time.deltaTime;
            yield return null;
        }
        while(timer>0f)
        {
            timer--;
            yield return new WaitForSeconds(1f);
        }
        while(CanvasGroup.alpha>1f)
        {
            canvasGroup.alpha-= 2*Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }

    private IEnumerator RotateToTarget()
    {
        while(this.enabled)
        {
            if(Target)
            {
                tPos = Target.position;
                tRot = Target.rotation;
            }
            Vector3 direction = player.position-tPos;
            tRot = Quaternion.LookRotation(direction);
            tRot.z = -tRot.y;
            tRot.y = 0;

            Vector3 northDirection = new Vector3(0,0,player.eulerAngles.y);
            Rect.localRotation = tRot*Quaternion.Euler(northDirection);

            yield return null;
        }
    }
}
