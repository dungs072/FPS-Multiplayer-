using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OptionMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputfield;
    public static string PlayerName{get; set;}

    private void Start() {
        // if(PlayerPrefs.GetString("UserName")==""){return;}
        // nameInputfield.text = PlayerPrefs.GetString("UserName");
        //PlayerName = nameInputfield.text;
    }
    public void Apply()
    {
        PlayerPrefs.SetString("UserName",nameInputfield.text);
        PlayerName = nameInputfield.text;
    } 
}
