using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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
    [Header("Pop up score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject headShotPanel;
    [Header("Respawn")]
    [SerializeField] private GameObject respawnUI;
    [SerializeField] private TMP_Text countDownRespawnText;
    [Header("Posture")]
    [SerializeField] private Image postureImage;
    [SerializeField] private Sprite crouchPosture;
    [SerializeField] private Sprite standPosture;
    [Header("Parent")]
    [SerializeField] private GameObject parentUI;
    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;
    [Header("Score")]
    [SerializeField] private TMP_Text swatScoreText;
    [SerializeField] private TMP_Text terroristScoreText;
    [Header("Pickup infor")]
    [SerializeField] private Image itemDisplay;
    [SerializeField] private TMP_Text nameItemDisplay;

    public PackWeaponUI[] Packs { get { return packs; } }
    private Coroutine hitCrossHairCoroutine;
    private Coroutine startBloodOverlayCoroutine;
    private Coroutine endBloodOverlayCoroutine;
    private Coroutine nearlyDieCoroutine;
    private Coroutine damageCoroutine;
    private Coroutine scoreRewardCoroutine;
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
    public void ToggleParentUI(bool state)
    {
        parentUI.SetActive(state);
    }
    public void ChangePosture(bool isCrouch)
    {
        if(!isCrouch)
        {
            postureImage.sprite = standPosture;
        }
        else
        {
            postureImage.sprite = crouchPosture;
        }
    }
    public void ChangeCrossHair(ItemType type)
    {
        GetComponent<CrossHair>().ChangeCrossHair(type);
    }
    public void ClearPackWeapon()
    {
        if(packs.Length<1){return;}
        packs[1].ClearAllInfor();
    }
    public void TriggerScoreRewardUI(bool isHeashot)
    {
        headShotPanel.SetActive(isHeashot);
        scoreText.gameObject.SetActive(true);
        if (scoreRewardCoroutine != null) { StopCoroutine(scoreRewardCoroutine); }
        scoreRewardCoroutine = StartCoroutine(ScoreRewardUI());
    }

    private IEnumerator ScoreRewardUI()
    {
        yield return new WaitForSeconds(2f);
        headShotPanel.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }
    public void ToggleRespawnUI(bool state)
    {
        respawnUI.SetActive(state);
        ClearPackWeapon();
    }
    public void UpdateRespawnUI(float time)
    {
        countDownRespawnText.text = time.ToString("0.00");
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
            while (canvasGroup.alpha < 0.9f)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, 15 * Time.deltaTime);
                yield return new WaitForSeconds(timePerChangeAlphaValue);
            }
            yield return null;
            while (canvasGroup.alpha > minBloodOverlay)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, 15 * Time.deltaTime);
                yield return new WaitForSeconds(timePerChangeAlphaValue);
            }
        }
    }
    public void TriggerStopNearlyDieUI()
    {
        isNearlyDie = false;
        if (nearlyDieCoroutine != null) { StopCoroutine(nearlyDieCoroutine); }
        if (damageCoroutine != null) { StopCoroutine(damageCoroutine); }
        StopStartEndBloodOverlayCoroutines();
        endBloodOverlayCoroutine = StartCoroutine(EndBloodOverlay(0f, speedStopAlpha));//bug there
    }
    public void TriggerBloodOverlay()
    {
        if (isNearlyDie) { return; }
        //if (nearlyDieCoroutine != null) { return; }
        if(damageCoroutine!=null){StopCoroutine(damageCoroutine);}
        damageCoroutine = StartCoroutine(DoBloodOverlay(0f, 1f, timeToEndBloodOverlay));
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
            if (canvasGroup.alpha < 0.05f) { canvasGroup.alpha = minAmount; }
            yield return new WaitForSeconds(timePerChangeAlphaValue);
        }
    }

    public void ChangeHealthBar(float amount)
    {
        foreGroundHealthBar.localScale = new Vector3(amount, 1f, 1f);
    }
    public void TogglePauseMenu(bool state)
    {
        pauseMenu.SetActive(state);
    }

    public void SetSwatScoreDisplay(int score)
    {
        swatScoreText.text = score.ToString();
    }
    public void SetTerroristScoreDisplay(int score)
    {
        terroristScoreText.text = score.ToString();
    }

    public void SetItemPickupInfor(Sprite icon, string name)
    {
        itemDisplay.sprite = icon;
        nameItemDisplay.text = name;
    }
}

