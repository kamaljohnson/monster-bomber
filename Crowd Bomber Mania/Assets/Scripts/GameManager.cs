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
        GameState = GameState.AtMenu;
        _gameManager.ShowMenu();        

        Reset();

        _gameManager.gameOverUi.SetActive(false);
        _gameManager.gameWonUi.SetActive(false);
    }
    
    public static void StartGame()
    {
        GameState = GameState.Playing;
        _gameManager.HideMenu();
    }

    public static void GameOver()
    {
        GameState = GameState.GameOver;
        _gameManager.gameOverUi.SetActive(true);
        _gameManager.ShowMenu();
    }

    public static void GameWon()
    {
        GameState = GameState.GameWon;
        _gameManager.gameWonUi.SetActive(true);
        _gameManager.ShowMenu();
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
        var remainingInfectedPersons = GameObject.FindGameObjectsWithTag(Person.GetTag(PersonTags.Infected));
        if (remainingInfectedPersons.Length != 0) return;
        if (Cannon.CannonBallsRemaining() != 0) return;

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

    private void Reset()
    {
        Cannon.Reset();
        PersonSpawner.Reset();
        GameProgressManager.Reset();
    }
}
