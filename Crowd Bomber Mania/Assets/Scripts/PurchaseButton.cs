using System;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public enum PurchaseType
    {
        removeAds
    }

    public PurchaseType purchaseType;

    public void ClickPurchaseButton()
    {
        switch (purchaseType)
        {
            case PurchaseType.removeAds:
                IAPManager.instance.BuyRemoveAds();
                break;
        }
    }
}
