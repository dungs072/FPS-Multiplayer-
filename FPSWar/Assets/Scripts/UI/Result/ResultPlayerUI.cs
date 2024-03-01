using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResultPlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text namePlayer;
    [SerializeField] private TMP_Text killText;

    public void SetNameAndKill(string playerName, int numberKills)
    {
        namePlayer.text = playerName;
        killText.text = numberKills.ToString();
    }
}
