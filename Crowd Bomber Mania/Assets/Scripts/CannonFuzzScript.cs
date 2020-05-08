using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFuzzScript : MonoBehaviour
{

    public AudioSource audio;
    private void OnEnable()
    {
        audio.Play(0);
    }
}
