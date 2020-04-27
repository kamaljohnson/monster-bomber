using System;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public TMP_Text cashText;

    private static int _cash;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerCash"))
        {
            _cash = PlayerPrefs.GetInt("PlayerCash");
        }
        else
        {
            PlayerPrefs.SetInt("PlayerCash", _cash);
        }
    }

    private void Update()
    {
        cashText.text = _cash + " $";
    }

    public static void AddCash(int cash)
    {
        _cash += cash;
        PlayerPrefs.SetInt("PlayerCash", _cash);
    }
}
