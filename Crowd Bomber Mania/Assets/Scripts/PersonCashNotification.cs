using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersonCashNotification : MonoBehaviour
{

    public TMP_Text amountText;

    public void SetCashAmount(ulong amount)
    {
        amountText.text = CashManager.GetCashDisplay(amount);
    }
}
