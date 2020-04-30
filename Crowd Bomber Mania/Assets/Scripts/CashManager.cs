using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CashManager : MonoBehaviour
{
    public TMP_Text cashText;

    public Animator animator;
    
    private static int _cash;

    private static CashManager _cashManager;

    private void Start()
    {
        _cashManager = this;
        GetPlayerCashFromPref();
        _cashManager.UpdateUi();
    }

    // -ve cash will be deducted
    public static void AddOrRemoveCash(int cash)
    {
        Debug.Log("cash added : " + cash);
        _cash += cash;
        SetPlayerCashToPref();
        _cashManager.UpdateUi();
        if (cash > 0)
        {
            _cashManager.animator.Play("CashAddAnimation", -1, 0f);
        }
        else
        {
            _cashManager.animator.Play("CashDeductAnimation", -1, 0f);
        }
    }

    private static void GetPlayerCashFromPref()
    {
        if (PlayerPrefs.HasKey("PlayerCash"))
        {
            _cash = PlayerPrefs.GetInt("PlayerCash");
        }
        else
        {
            SetPlayerCashToPref();
        }
    }

    private static void SetPlayerCashToPref()
    {
        PlayerPrefs.SetInt("PlayerCash", _cash);
    }
    
    public static bool MakePurchase(int cost)
    {
        var purchaseFlag = false;

        if (_cash >= cost)
        {
            AddOrRemoveCash(-cost);
            purchaseFlag = true;
        }

        if (purchaseFlag == false)
        {
            _cashManager.animator.Play("NotEnoughCashAnimation", -1, 0f);
            Debug.Log("Not enough cash");
        }

        return purchaseFlag;
    }

    private void UpdateUi()
    {
        cashText.text = GetCashIn_kmb(_cash) + " $";
    }

    public static string GetCashIn_kmb(int cash)
    {
        if (cash > 1000000) return ((int) (cash / 10000)).ToString("F") + "B";
        if (cash > 100000) return ((int)(cash / 10000)).ToString("F") + "M";
        if (cash > 10000) return ((int)(cash / 1000)).ToString("F") + "K";
        return cash.ToString();
    }

    public static int GetCash()
    {
        return _cash;
    }
}
