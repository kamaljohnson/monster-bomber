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
    }
}
