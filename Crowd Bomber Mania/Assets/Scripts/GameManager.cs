using System;
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
    
    public void Start()
    {
        GameState = GameState.AtMenu;
        _gameManager = this;
        _gameManager.ShowMenu();
    }

    public static void StartGame()
    {
        GameState = GameState.Playing;
        _gameManager.HideMenu();
    }

    public static void GameOver()
    {
        GameState = GameState.GameOver;
        _gameManager.ShowMenu();
    }

    public static void GameWon()
    {
        GameState = GameState.GameWon;
        _gameManager.ShowMenu();
        LevelManager.IncrementLevel();
    }

    private void ShowMenu()
    {
        bottomPowerUpUi.GetComponent<Animator>().Play("BottomPowerUpAnimateIn", -1, 0);
    }

    private void HideMenu()
    {
        bottomPowerUpUi.GetComponent<Animator>().Play("BottomPowerUpAnimateOut", -1, 0);
    }
}
