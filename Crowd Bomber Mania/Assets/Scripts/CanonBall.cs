﻿using System;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    public float destructionDuration;

    public void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject, destructionDuration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Person"))
        {
            other.gameObject.GetComponent<Person>().TriggerInfection();
        }
    }
}