using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBeepScript : MonoBehaviour
{

    public AudioSource audio;

    public void OnEnable()
    {
        Debug.Log("Beep");
        audio.Play(0);
    }
}
