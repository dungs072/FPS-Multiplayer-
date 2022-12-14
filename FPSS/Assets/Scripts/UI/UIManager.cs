using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField][Range(0,1f)] private float minBloodOverlay = 0.7f;
    private Coroutine hitCrossHairCoroutine;
    private Coroutine startBloodOverlayCoroutine;
    private Coroutine endBloodOverlayCoroutine;
    private Coroutine nearlyDieCoroutine;
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
        nearlyDieCoroutine = StartCoroutine(HandleNearlyDieUI());
    }
    private IEnumerator HandleNearlyDieUI()
    {
        while(true)
        {
            StartCoroutine(DoBloodOverlay(minBloodOverlay,1f,timeToEndBloodOverlay));
            yield return 2f;//bug here
        }
    }
    public void TriggerStopNearlyDieUI()
    {
        if(nearlyDieCoroutine==null){return;}
        StopCoroutine(nearlyDieCoroutine);
    }
    public void TriggerBloodOverlay()
    {
        StartCoroutine(DoBloodOverlay(0f,1f,timeToEndBloodOverlay));
    }
    private IEnumerator DoBloodOverlay(float min, float max,float time)
    {
        StopStartEndBloodOverlayCoroutines();
        startBloodOverlayCoroutine = StartCoroutine(StartBloodOverlay(max));
        yield return new WaitForSeconds(time);
        StopStartEndBloodOverlayCoroutines();
        endBloodOverlayCoroutine = StartCoroutine(EndBloodOverlay(min));
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
    private IEnumerator StartBloodOverlay(float maxAmount)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, maxAmount))
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, maxAmount, speedStartAlpha * Time.deltaTime);
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }
    private IEnumerator EndBloodOverlay(float minAmount)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, minAmount))
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, minAmount, speedStopAlpha * Time.deltaTime);
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }
}
