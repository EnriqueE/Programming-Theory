using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    /// <summary>
    /// Controls de Game
    /// </summary>

    private string m_UserName = "UnnamedPlayer";
    public int score = 0;
    public int scoreMultiplier = 10; 
    private UIGameController uIGameController; 
    public string UserName
    {
        set
        {
            if (value.ToString().Length < 4)
            {
                Debug.LogError("The username must contain at least 4 characters");
            } else
            {
                m_UserName = value;
            }
        }
        get
        {
            return m_UserName;
        }
    }

    public Text debugGameStateText;
    public Text debugGeneralPurposeText1;
    public Text debugGeneralPurposeText2;
    public Text debugGeneralPurposeText3;

    public class Record
    {
        public string username;
        public int score;
    }
    private GameState gameState = GameState.intro;
    public enum GameState { intro, startMenu, play, gameOver }

    public void AddScore(int newScore) {
        score += newScore * scoreMultiplier;
        uIGameController.UpdateUIInfo(); 
    }
    private void Start()
    {
        uIGameController = GameObject.Find("UI").GetComponent<UIGameController>();    
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game"); 
    }
    public void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
        if(debugGameStateText) {
            debugGameStateText.text = gameState.ToString(); 
        }
        
    }
    public GameState GetGameSate()
    {
        return gameState; 
        
    }
}
