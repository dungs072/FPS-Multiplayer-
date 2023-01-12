using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInfor : MonoBehaviour
{
    public static PlayerInfor Instance { get; private set; }
    [SerializeField] private TMP_InputField nameInput;

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

    public string PlayerName { get { return nameInput.text; } }
}
