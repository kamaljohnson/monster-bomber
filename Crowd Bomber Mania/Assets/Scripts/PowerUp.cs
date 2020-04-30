using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    private int adShowCount = 3;    
    private int _adCounter;
    
    private void Start()
    {
        powerUpCost = GetPriceOfPowerUpFromPref();
        UpdateUi();
    }

    public void RequestPowerUpActivation()
    {        
        var priceOfPowerUp = GetPriceOfPowerUpFromPref();
        bool purchaseStatus;
        
        purchaseStatus = _adSupport || CashManager.MakePurchase(priceOfPowerUp);

        if (_adSupport)
        {
            RewardedAdsManager.ShowRewardedVideo();
        }
        
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
                PersonMovementController.UpdatePersonSpeed();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdatePowerUpCost();
    }

    private int GetPriceOfPowerUpFromPref()
    {
        if (!PlayerPrefs.HasKey(GetName(type) + "Cost"))
        {
            SetPowerUpCostToPref(powerUpInitialCosts[(int) type]);
        }
        
        return PlayerPrefs.GetInt(GetName(type) + "Cost");
    }

    private void SetPowerUpCostToPref(int cost)
    {
        PlayerPrefs.SetInt(GetName(type) + "Cost", cost);
    }

    private void UpdatePowerUpCost()
    {
        powerUpCost = (int)(powerUpCost * powerUpMultipliers[(int)type]);
        SetPowerUpCostToPref(powerUpCost);
        
        if (CashManager.GetCash() < powerUpCost && !_adSupport)
        {
            _adSupport = true;
        }
        else
        {
            _adSupport = false;
        }
        
        _adCounter++;
        if (_adCounter >= adShowCount || _adSupport)
        {
            _adSupport = true;
            _adCounter = 0;
        }
        
        UpdateUi();
    }

    private void UpdateUi()
    {
        powerUpCostText.text = CashManager.GetCashIn_kmb(powerUpCost) + " $";

        //TODO: get actual availability
        var adAvailable = RewardedAdsManager.IsAdReady();
        if (_adSupport && adAvailable)
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

    public static void Reset()
    {
        foreach (var powerUp in FindObjectsOfType<PowerUp>())
        {
            powerUp._adSupport = false;
            powerUp.UpdateUi();
        }
    }
}

