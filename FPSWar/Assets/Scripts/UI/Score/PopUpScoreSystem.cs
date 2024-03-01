using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScoreSystem : MonoBehaviour
{
    [SerializeField] private GameObject popUpScorePrefab;
    [SerializeField] private RectTransform holder;


    public void SpawnPopupScore(Vector2 posOnScreen)
    {
        GameObject popUpScoreInstance = Instantiate(popUpScorePrefab,holder);
        popUpScoreInstance.GetComponent<RectTransform>().anchoredPosition = posOnScreen;
        //Destroy(popUpScoreInstance,2f);
    }
}
