using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public TMP_Text cashText;

    private static int _cash;

    private static CashManager _cashManager;

    private void Start()
    {
        _cashManager = this;
        if (PlayerPrefs.HasKey("PlayerCash"))
        {
            _cash = PlayerPrefs.GetInt("PlayerCash");
        }
        else
        {
            PlayerPrefs.SetInt("PlayerCash", _cash);
        }
        _cashManager.UpdateUi();
    }

    public static void AddCash(int cash)
    {
        _cash += cash;
        PlayerPrefs.SetInt("PlayerCash", _cash);
        GameProgressManager.UpdateProgress(cash);
        _cashManager.UpdateUi();
    }

    private void UpdateUi()
    {
        cashText.text = _cash + " $";
    }
}
