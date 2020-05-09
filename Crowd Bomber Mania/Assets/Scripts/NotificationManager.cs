using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum NotificationType
{
    PopulationPowerUp,
    ExtraCannonPowerUp,
    SpeedPowerUp,
    NotEnoughCash
}

public class NotificationManager : MonoBehaviour
{
    public Transform spawnLocation;

    public Sprite populationPowerUpImage;
    public Sprite extraCannonPowerUpImage;
    public Sprite speedPowerUpImage;
    public Sprite notEnoughCashImage;

    private static NotificationManager _notificationManager;

    public GameObject notificationUnit;
    
    private void Start()
    {
        _notificationManager = this;
    }

    public static void Notify(NotificationType type)
    {
        _notificationManager.InstantiateNotification(type);
    }

    private void InstantiateNotification(NotificationType type)
    {
        var notificationObj = Instantiate(notificationUnit, spawnLocation);
        
        var notification = notificationObj.GetComponent<NotificationUnit>();
        
        var sprite = GetNotificationImage(type);
        var text = GetNotificationText(type);
        notification.SetSprite(sprite);
        notification.SetText(text);

    }

    private string GetNotificationText(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.PopulationPowerUp:
                return "Power Up Activated";
            case NotificationType.ExtraCannonPowerUp:
                return "Power Up Activated";
            case NotificationType.SpeedPowerUp:
                return "Power Up Activated";
            case NotificationType.NotEnoughCash:
                return "Not Enough Cash";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    private Sprite GetNotificationImage(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.PopulationPowerUp:
                return populationPowerUpImage;
            case NotificationType.ExtraCannonPowerUp:
                return extraCannonPowerUpImage;
            case NotificationType.SpeedPowerUp:
                return speedPowerUpImage;
            case NotificationType.NotEnoughCash:
                return notEnoughCashImage;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
