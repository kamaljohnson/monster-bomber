using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum NotificationType
{
    PopulationPowerUp,
    ExtraCannonPowerUp,
    SpeedPowerUp,
    NotEnoughCash,
    LevelUp,
    AllMonstersInfected
}

public class NotificationManager : MonoBehaviour
{
    public Transform spawnLocation;

    public Sprite populationPowerUpImage;
    public Sprite extraCannonPowerUpImage;
    public Sprite speedPowerUpImage;
    public Sprite notEnoughCashImage;
    public Sprite levelUpImage;
    public Sprite allMonstersInfectedImage;

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
            case NotificationType.LevelUp:
                return "Level Up";
            case NotificationType.AllMonstersInfected:
                return "All Monsters Infected";
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
            case NotificationType.LevelUp:
                return levelUpImage;
            case NotificationType.AllMonstersInfected:
                return allMonstersInfectedImage;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
