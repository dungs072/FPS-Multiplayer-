using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Mirror;
public class GameRuleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text ruleGameDisplay;
    [SerializeField] private Transform parentPanels;
    [SerializeField] private GameObject maxDeathsPanel;
    [SerializeField] private GameObject robberPanel;
    [SerializeField] private GameObject invasionPanel;

    private List<GameRule> rules = new List<GameRule>();
    public string GameRuleName { get; set; }
    private int currentIndex = 0;
    public GameRule GameRule
    {
        get
        {
            return rules[currentIndex];
        }
    }
    public void SetCurrentIndex(int value)
    {
        currentIndex = value;
        SetTextDisplayRuleGame();
    }

    private void Awake()
    {
        rules.Add(GameRule.Score);
        rules.Add(GameRule.Rob);
        rules.Add(GameRule.Invade);
    }
    private void Start() {
        PlayerController.OnUpdateCurrentRuleIndex+=SetCurrentIndex;
        SetTextDisplayRuleGame();
    }
    private void OnDestroy() {
        PlayerController.OnUpdateCurrentRuleIndex-=SetCurrentIndex;
    }

    #region UI
    public void OnIncreaseClick()
    {
        currentIndex = Math.Min(currentIndex + 1, rules.Count - 1);
        SetTextDisplayRuleGame();
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdChangeCurrentRuleIndex(currentIndex);

    }


    public void OnDecreaseClick()
    {
        currentIndex = Math.Max(currentIndex - 1, 0);
        SetTextDisplayRuleGame();
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdChangeCurrentRuleIndex(currentIndex);
    }
    #endregion
    public void ChangeGameRule(int value)
    {
        currentIndex = value;
        SetTextDisplayRuleGame();
    }
    private void SetTextDisplayRuleGame()
    {
        ToggleAllPanels(false);
        if (GameRule == GameRule.Score)
        {
            ruleGameDisplay.text = "Death match";
            
            maxDeathsPanel.gameObject.SetActive(true);
        }
        else if (GameRule == GameRule.Rob)
        {
            ruleGameDisplay.text = "Boom protection";
            robberPanel.gameObject.SetActive(true);
        }
        else if (GameRule == GameRule.Invade)
        {
            ruleGameDisplay.text = "Invasion";
            invasionPanel.gameObject.SetActive(true);
        }
        GameRuleName = ruleGameDisplay.text;
        UIManager.Instance.GameRule = GameRule;
        UIManager.Instance.ChangeGameRuleResultPanel(GameRule);
    }
    private void ToggleAllPanels(bool state)
    {
        foreach(Transform panel in parentPanels)
        {
            panel.gameObject.SetActive(state);
        }
    }
    public int GetCurrentIndex()
    {
        return currentIndex;
    }

}
public enum GameRule
{
    Score,
    Rob,
    Invade
}
