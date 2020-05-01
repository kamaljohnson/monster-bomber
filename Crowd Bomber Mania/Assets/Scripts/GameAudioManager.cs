using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public GameObject speakerOnIcon;
    public GameObject speakerOffIcon;

    public void SpeakerOn()
    {
        speakerOffIcon.SetActive(false);
        speakerOnIcon.SetActive(true);
        AudioListener.pause = false;
    }
    
    public void SpeakerOff()
    {
        speakerOffIcon.SetActive(true);
        speakerOnIcon.SetActive(false);
        AudioListener.pause = true;
    }
}
