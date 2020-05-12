using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public AnimationClip splashScreenAnimationClip;

    public void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(splashScreenAnimationClip.length);
        
        SceneManager.LoadScene("Game");
    }    
}
