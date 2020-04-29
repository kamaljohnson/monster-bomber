using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PowerUpType
{
    PopulationGrowth,
    ExtraCannon,
    SpeedIncrease
}

public class PowerUp : MonoBehaviour
{

    public PowerUpType type;
    public GameObject adIcon;
    public TMP_Text powerUpCostText;
    
    public int powerUpCost;

    public List<int> powerUpInitialCosts;

    public List<float> powerUpMultipliers;

    private bool _adSupport;
    
    private void Start()
    {
        powerUpCost = GetPriceOfPowerUp();
        UpdateUi();
        Debug.Log(GetName(type));
    }

    public void RequestPowerUpActivation()
    {
        var priceOfPowerUp = GetPriceOfPowerUp();
        bool purchaseStatus;
        
        purchaseStatus = priceOfPowerUp == 0 || CashManager.MakePurchase(priceOfPowerUp);
        
        if (purchaseStatus)
        {
            ActivatePowerUp();
        }
    }

    public void ActivatePowerUp()
    {
        switch (type)
        {
            case PowerUpType.PopulationGrowth:
                PersonSpawner.SpawnExtraPersons(1);
                break;
            case PowerUpType.ExtraCannon:
                FindObjectOfType<Cannon>().AddExtraCannonBall(1);
                break;
            case PowerUpType.SpeedIncrease:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdatePowerUpCost();
    }

    private int GetPriceOfPowerUp()
    {
        if (!PlayerPrefs.HasKey(GetName(type) + "Cost"))
        {
            SetPowerUpCost(powerUpInitialCosts[(int) type]);
        }
        
        return PlayerPrefs.GetInt(GetName(type) + "Cost");
    }

    private void SetPowerUpCost(int cost)
    {
        PlayerPrefs.SetInt(GetName(type) + "Cost", cost);
    }

    private void UpdatePowerUpCost()
    {
        powerUpCost = (int)(powerUpCost * powerUpMultipliers[(int)type]);
        SetPowerUpCost(powerUpCost);
        UpdateUi();
    }

    private void UpdateUi()
    {
        powerUpCostText.text = CashManager.GetCashIn_kmb(powerUpCost) + " $";
        if (_adSupport)
        {
            adIcon.gameObject.SetActive(true);
            powerUpCostText.gameObject.SetActive(false);

        }
        else
        {
            powerUpCostText.gameObject.SetActive(true);
            adIcon.gameObject.SetActive(false);
        }
    }

    private static string GetName(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.PopulationGrowth:
                return "PopulationGrowth";
            case PowerUpType.ExtraCannon:
                return "ExtraCannon";
            case PowerUpType.SpeedIncrease:
                return "SpeedIncrease";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

