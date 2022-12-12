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
    private Coroutine hitCrossHairCoroutine;
    private Coroutine startBloodOverlayCoroutine;
    private Coroutine endBloodOverlayCoroutine;
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
    public void TriggerBloodOverlay()
    {
        StartCoroutine(DoBloodOverlay());
    }
    private IEnumerator DoBloodOverlay()
    {
        StopStartEndBloodOverlayCoroutines();
        startBloodOverlayCoroutine = StartCoroutine(StartBloodOverlay());
        yield return new WaitForSeconds(timeToEndBloodOverlay);
        StopStartEndBloodOverlayCoroutines();
        endBloodOverlayCoroutine = StartCoroutine(EndBloodOverlay());
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
    private IEnumerator StartBloodOverlay()
    {
        while (!Mathf.Approximately(canvasGroup.alpha, 1f))
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, speedStartAlpha * Time.deltaTime);
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }
    private IEnumerator EndBloodOverlay()
    {
        while (!Mathf.Approximately(canvasGroup.alpha, 0f))
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, speedStopAlpha * Time.deltaTime);
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }
}
