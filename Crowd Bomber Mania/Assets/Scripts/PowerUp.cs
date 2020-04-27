using System;
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

    public void ActivatePowerUp()
    {
        switch (type)
        {
            case PowerUpType.PopulationGrowth:
                FindObjectOfType<PersonSpawner>().SpawnPerson(1);
                break;
            case PowerUpType.ExtraCannon:
                break;
            case PowerUpType.SpeedIncrease:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
