using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public TMP_Text cashText;

    public Animator animator;

    public AudioSource cashAddSound;
    public AudioSource cashRemoveSound;
    public AudioSource notEnoughCashSound;
    
    private static ulong _cash ;

    private static CashManager _cashManager;

    private void Start()
    {
        _cashManager = this;
        GetPlayerCashFromPref();
        _cashManager.UpdateUi();
    }

    // -ve cash will be deducted
    public static void AddOrRemoveCash(ulong cash, bool remove = false)
    {
        GetPlayerCashFromPref();
        if (!remove)
        {
            _cash += cash;
            _cashManager.cashAddSound.Play();
            _cashManager.animator.Play("CashAddAnimation", -1, 0f);
        }
        else
        {
            _cash -= cash;
            _cashManager.cashRemoveSound.Play();
            _cashManager.animator.Play("CashDeductAnimation", -1, 0f);
        }
        SetPlayerCashToPref();
        _cashManager.UpdateUi();
    }

    private static void GetPlayerCashFromPref()
    {
        if (PlayerPrefs.HasKey("PlayerCash"))
        {
            _cash = Convert.ToUInt64(PlayerPrefs.GetString("PlayerCash"));
        }
        else
        {
            SetPlayerCashToPref();
        }
    }

    private static void SetPlayerCashToPref()
    {
        PlayerPrefs.SetString("PlayerCash", "" + _cash);
    }

    public static bool MakePurchase(ulong cost)
    {
        
        var purchaseFlag = false;

        if (_cash >= cost)
        {
            AddOrRemoveCash(cost, true);
            purchaseFlag = true;
        }

        if (purchaseFlag == false)
        {
            _cashManager.animator.Play("NotEnoughCashAnimation", -1, 0f);
            _cashManager.notEnoughCashSound.Play();
            NotificationManager.Notify(NotificationType.NotEnoughCash);
        }

        return purchaseFlag;
    }

    private void UpdateUi()
    {
        cashText.text = GetCashDisplay(_cash);
    }
    public static ulong GetCash()
    {
        return _cash;
    }

    public static string GetCashDisplay(ulong cash)
    {
        var displayCash = "";
        var numberOfDigitsInCash = cash > 0 ? Math.Ceiling(Math.Log10(cash)): 0;
        switch (numberOfDigitsInCash)
        {
            case 0:
                displayCash = "" + cash;
                break;
            case 1:
                displayCash = "" + cash;
                break;
            case 2:
                displayCash = "" + cash;
                break;
            case 3:
                displayCash = "" + cash;
                break;
            case 4:
                displayCash = "" + cash;
                break;
            case 5:
                displayCash = "" + cash/1000 + "K";
                break;
            case 6:
                displayCash = "" + cash/1000 + "K";
                break;
            case 7:
                displayCash = "" + cash/1000 + "K";
                break;
            case 8:
                displayCash = "" + cash/1000000 + "M";
                break;
            case 9:
                displayCash = "" + cash/1000000 + "M";
                break;
            case 10:
                displayCash = "" + cash/1000000 + "M";
                break;
            case 11:
                displayCash = "" + cash/1000000000 + "B";
                break;
            case 12:
                displayCash = "" + cash/1000000000 + "B";
                break;
            case 13:
                displayCash = "" + cash/1000000000 + "B";
                break;
            case 14:
                displayCash = "" + cash/1000000000000 + "T";
                break;
            case 15:
                displayCash = "" + cash/1000000000000 + "T";
                break;
            case 16:
                displayCash = "" + cash/1000000000000 + "T";
                break;
            default:
                displayCash = "" + cash/1000000000000000 + "Z";
                break;
        }

        return displayCash + " $";
    }
}
