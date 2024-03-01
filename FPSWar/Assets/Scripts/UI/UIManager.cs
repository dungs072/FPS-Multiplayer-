using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static event Action OnExitMatch;
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
    [Header("EnergyBar")]
    [SerializeField] private RectTransform energyBar;
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
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject[] pauseElements;
    [SerializeField] private GameObject settingPanel;
    [Header("Score")]
    [SerializeField] private TMP_Text swatScoreText;
    [SerializeField] private TMP_Text terroristScoreText;
    [Header("Pickup infor")]
    [SerializeField] private Image itemDisplay;
    [SerializeField] private TMP_Text nameItemDisplay;
    [Header("Kill Display")]
    [SerializeField] private Transform displayBox;
    [SerializeField] private KillBox killBoxPrefab;
    [Header("Result")]
    [SerializeField] private GameObject resultPanel;
    [Header("Pi menu")]
    [SerializeField] private PiUIManager piUI; 
    [SerializeField] private TMP_Text nameItemText;
    [Header("RobberManager")]
    [SerializeField] private GameObject fbuttonUIRobber;
    [SerializeField] private GameObject iconFButton;
    [SerializeField] private TMP_Text titleRobber;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text timeInGame;
    [Header("Score panel")]
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject fillPanel;

    public PackWeaponUI[] Packs { get { return packs; } }
    private Coroutine hitCrossHairCoroutine;
    private Coroutine startBloodOverlayCoroutine;
    private Coroutine endBloodOverlayCoroutine;
    private Coroutine nearlyDieCoroutine;
    private Coroutine damageCoroutine;
    private Coroutine scoreRewardCoroutine;
    private bool isNearlyDie = false;
    public PiUIManager PIUI{get{return piUI;}}

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
    private void Start() {
        OnExitMatch+=HandleUIWhenExitMatch;
    }
    private void OnDestroy() {
        OnExitMatch-=HandleUIWhenExitMatch;
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
        packs[0].ClearAllInfor();
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
    public void ChangeEnergyBar(float amount)
    {
        energyBar.localScale = new Vector3(amount,1f,1f);
    }
    public void TogglePauseMenu(bool state)
    {
        pausePanel.SetActive(state);
        if(!state)
        {
            settingPanel.SetActive(state);
        }
        ReturnToNormalPauseMenu();
        
    }
    public void ReturnToNormalPauseMenu()
    {
        pauseMenu.SetActive(true);
        foreach(var ele in pauseElements)
        {
            ele.SetActive(false);
        }
    }
    public void ReturnDefaultStateUI()
    {
        FButtonUI.SetActive(false);
        respawnUI.SetActive(false);
        pausePanel.SetActive(false);
        resultPanel.SetActive(false);
        swatScoreText.text = "0";
        terroristScoreText.text = "0";
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
    public void CreateDisplayKillBox(string nameKiller, string namePatient)
    {
        KillBox killBoxInstance = Instantiate(killBoxPrefab,displayBox);
        killBoxInstance.SetKillTitle(nameKiller,namePatient);
    }
    public void ExitMatch()
    {
        ReturnToNormalPauseMenu();
        OnExitMatch?.Invoke();
    }

    #region Pie
    public void SetNameItemInPi(string name)
    {
        nameItemText.text = name;
    }
    #endregion

    #region Exit game
    private void HandleUIWhenExitMatch()
    {
        parentUI.SetActive(false);
    }
    #endregion

    #region GameRule
    public GameRule GameRule
    {
        get;
        set;
    }
    public void ChangeGameRuleResultPanel(GameRule gameRule)
    {
        scorePanel.SetActive(false);
        fillPanel.SetActive(false);
        if(gameRule==GameRule.Score)
        {
            scorePanel.SetActive(true);
        }
        else if(gameRule==GameRule.Rob)
        {
            fillPanel.SetActive(true);
        }
    }
    #endregion


    #region RobberManager
    public void ToggleFButtonUIRobberManager(bool state, bool state2 = true)
    {
        fbuttonUIRobber.SetActive(state);
        iconFButton.SetActive(state2);
    }
    public void SetTitleRobber(string text)
    {
        titleRobber.text = text;
    }
    public void ChangeFillImageValue(float value)
    {
        fillImage.fillAmount = value;
    }
    public void ChangeTime(int minute, int second)
    {
        timeInGame.text = minute.ToString()+":"+second.ToString();
    }
    #endregion
}
