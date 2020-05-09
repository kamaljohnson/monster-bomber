using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBeepScript : MonoBehaviour
{

    public AudioSource audio;

    public void OnEnable()
    {
        audio.Play(0);
    }
}
