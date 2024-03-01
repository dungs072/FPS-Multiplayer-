using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KillBox : MonoBehaviour
{
    [SerializeField] private TMP_Text killTitle;
    [SerializeField] private float timeToDestroy = 4f;

    private void Start() {
        Destroy(gameObject,timeToDestroy);
    }
    public void SetKillTitle(string nameKiller, string namePatient)
    {
        killTitle.text = nameKiller + " killed " + namePatient;
    }
}
