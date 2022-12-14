using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject crossHairScope;
    [SerializeField] private GameObject dynamicCrossHair;
    [SerializeField] private GameObject centerDot;
    [SerializeField] private GameObject FButtonUI;
    [SerializeField] private GameObject hitCrossHair;

    [SerializeField] private float timeToEndBloodOverlay = 1f;
    [SerializeField] private float timePerChangeAlphaValue = 0.2f;
    [SerializeField] private float speedStartAlpha = 10f;
    [SerializeField] private float speedStopAlpha = 5f;
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Nearly die UI")]
    [SerializeField][Range(0, 1f)] private float minBloodOverlay = 0.7f;
    [Header("HealthBar")]
    [SerializeField] private RectTransform foreGroundHealthBar;
    [Header("Weapon Pack")]
    [SerializeField] private PackWeaponUI[] packs;

    public PackWeaponUI[] Packs{get{return packs;}}
    private Coroutine hitCrossHairCoroutine;
    private Coroutine startBloodOverlayCoroutine;
    private Coroutine endBloodOverlayCoroutine;
    private Coroutine nearlyDieCoroutine;
    private Coroutine damageCoroutine;
    private bool isNearlyDie = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ToggleCrossHairScope(bool state)
    {
        crossHairScope.SetActive(state);
    }
    public void ToggleDynamicCrossHair(bool state)
    {
        dynamicCrossHair.SetActive(!state);
        centerDot.SetActive(!state);
    }
    public void ToggleFButtonUI(bool state)
    {
        FButtonUI.SetActive(state);
    }
    public void ToggleHitCrossHair()
    {
        TurnOnHitCrossHair();
    }
    private void TurnOnHitCrossHair()
    {
        if (hitCrossHairCoroutine != null)
        {
            StopCoroutine(hitCrossHairCoroutine);
        }
        hitCrossHair.SetActive(true);
        hitCrossHairCoroutine = StartCoroutine(TurnOffHitCrossHair());
    }
    private IEnumerator TurnOffHitCrossHair()
    {
        yield return new WaitForSeconds(0.35f);
        hitCrossHair.SetActive(false);
    }
    public void TriggerNearlyDieUI()
    {
        TriggerStopNearlyDieUI();
        isNearlyDie = true;
        nearlyDieCoroutine = StartCoroutine(HandleNearlyDieUI());
    }
    private IEnumerator HandleNearlyDieUI()
    {
        while (isNearlyDie)
        {
            while (canvasGroup.alpha<0.9f)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, 15*Time.deltaTime);
                yield return new WaitForSeconds(timePerChangeAlphaValue);
            }
            yield return null;
            while (canvasGroup.alpha>minBloodOverlay)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, 15*Time.deltaTime);
                yield return new WaitForSeconds(timePerChangeAlphaValue);
            }
        }
    }
    public void TriggerStopNearlyDieUI()
    {
        isNearlyDie = false;
        if(nearlyDieCoroutine!=null){StopCoroutine(nearlyDieCoroutine);}
        if(damageCoroutine!=null){StopCoroutine(damageCoroutine);}
        StopStartEndBloodOverlayCoroutines();
        endBloodOverlayCoroutine = StartCoroutine(EndBloodOverlay(0f,speedStopAlpha));
    }
    public void TriggerBloodOverlay()
    {
        if (isNearlyDie) { return; }
        if(nearlyDieCoroutine!=null){return;}
        damageCoroutine =  StartCoroutine(DoBloodOverlay(0f, 1f, timeToEndBloodOverlay));
    }
    private IEnumerator DoBloodOverlay(float min, float max, float time)
    {
        StopStartEndBloodOverlayCoroutines();
        startBloodOverlayCoroutine = StartCoroutine(StartBloodOverlay(max, speedStartAlpha));
        yield return new WaitForSeconds(time);
        StopStartEndBloodOverlayCoroutines();
        endBloodOverlayCoroutine = StartCoroutine(EndBloodOverlay(min, speedStopAlpha));
    }
    private void StopStartEndBloodOverlayCoroutines()
    {
        if (endBloodOverlayCoroutine != null)
        {
            StopCoroutine(endBloodOverlayCoroutine);
        }
        if (startBloodOverlayCoroutine != null)
        {
            StopCoroutine(startBloodOverlayCoroutine);
        }
    }
    private IEnumerator StartBloodOverlay(float maxAmount, float speed)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, maxAmount))
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, maxAmount, speed * Time.deltaTime);
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }
    private IEnumerator EndBloodOverlay(float minAmount, float speed)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, minAmount))
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, minAmount, speed * Time.deltaTime);
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }

    public void ChangeHealthBar(float amount)
    {
        foreGroundHealthBar.localScale = new Vector3(amount,1f,1f);
    }
}
