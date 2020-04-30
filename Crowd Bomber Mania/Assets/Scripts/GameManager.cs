using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum GameState
{
    AtMenu,
    Playing,
    GameOver,
    GameWon
}



public class GameManager : MonoBehaviour
{

    public static GameState GameState;

    public static bool CanPlay;
    
    private static GameManager _gameManager;
    
    public GameObject bottomPowerUpUi;

    public GameObject gameOverUi;
    public GameObject gameWonUi;
    
    public void Start()
    {
        GameState = GameState.AtMenu;
        _gameManager = this;
        GoToMenu();
    }

    public void GoToMenu()
    {
        _gameManager.ShowMenu();

        Reset();

        _gameManager.gameOverUi.SetActive(false);
        _gameManager.gameWonUi.SetActive(false);
        
        GameState = GameState.AtMenu;
        StartCoroutine(TriggerCanPlay());
    }
    
    public static void StartGame()
    {
        GameState = GameState.Playing;
        _gameManager.HideMenu();
    }

    public static void GameOver()
    {
        UnityVideoAds.ShowAd();
        GameState = GameState.GameOver;
        _gameManager.gameOverUi.SetActive(true);
        CanPlay = false;
    }

    public static void GameWon()
    {
        GameState = GameState.GameWon;
        _gameManager.gameWonUi.SetActive(true);
        CanPlay = false;
    }

    public static void ReportPersonDead()
    {
        _gameManager.CheckGameEnding();
    }

    public static void ReportCannonBallUsed()
    {
        _gameManager.CheckGameEnding();
    }

    private void CheckGameEnding()
    {
        if (Cannon.CannonBallsRemaining() != 0) return;
        
        var cannonBallsInAir = GameObject.FindGameObjectsWithTag("CannonBall");
        if(cannonBallsInAir.Length != 0) return;
        
        // counting all the infected and contagious persons
        var remainingInfectedPersonsCount =
            GameObject.FindGameObjectsWithTag(Person.GetTag(PersonTags.Infected)).Length;
        if (remainingInfectedPersonsCount != 0) return;
        

        if (GameProgressManager.GetCurrentProgressState() == GameProgressState.Complete)
        {
            GameWon();
        }
        else
        {
            GameOver();
        }

    }

    private void ShowMenu()
    {
        bottomPowerUpUi.GetComponent<Animator>().Play("BottomPowerUpAnimateIn", -1, 0);
    }

    private void HideMenu()
    {
        bottomPowerUpUi.GetComponent<Animator>().Play("BottomPowerUpAnimateOut", -1, 0);
    }

    IEnumerator TriggerCanPlay()
    {
        yield return new WaitForSeconds(1f);
        CanPlay = true;
    }
    
    private void Reset()
    {
        Cannon.Reset();
        PersonSpawner.Reset();
        GameProgressManager.Reset();
        PowerUp.Reset();
    }
}
