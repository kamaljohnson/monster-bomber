using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersonCashNotification : MonoBehaviour
{

    public TMP_Text amountText;

    public void SetCashAmount(int amount)
    {
        amountText.text = amount + " $";
    }
}
