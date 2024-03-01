using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Mirror;

public class MaxDeathsUI : MonoBehaviour
{
    [SerializeField] private List<int> maxDeathsScore;
    [SerializeField] private TMP_Text maxDeathsText;
    private int currentIndex = 0;
    private void Start() {
        if(maxDeathsScore.Count==0)
        {
            Debug.LogWarning("Should set the max death score list first");
            return;
        }
        SetMaxDeathsDisplay(maxDeathsScore[currentIndex]);
        PlayerController.OnUpdateMaxDeaths += SetMaxDeathsIndexDisplay;
    }
    private void OnDestroy() {
        PlayerController.OnUpdateMaxDeaths -= SetMaxDeathsIndexDisplay;
    }
    public void IncreaseMaxDeaths()
    {
        currentIndex = Math.Min(currentIndex+1,maxDeathsScore.Count-1);
        SetMaxDeathsDisplay(maxDeathsScore[currentIndex]);
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdChangeMaxDeaths(currentIndex);
    }
    public void DecreaseMaxDeaths()
    {
        currentIndex = Math.Max(currentIndex-1,0);
        SetMaxDeathsDisplay(maxDeathsScore[currentIndex]);
        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdChangeMaxDeaths(currentIndex);
    }
    private void SetMaxDeathsIndexDisplay(int index)
    {
        SetMaxDeathsDisplay(maxDeathsScore[index]);
    }
    private void SetMaxDeathsDisplay(int number)
    {
        maxDeathsText.text = number.ToString();
    }
    public int GetMaxDeaths()
    {
        return maxDeathsScore[currentIndex];
    }
}
