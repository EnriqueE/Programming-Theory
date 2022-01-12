using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 


public class GameController : MonoBehaviour
{
    public static GameController instance;
    /// <summary>
    /// Controls de Game
    /// </summary>

    private string m_UserName = "UnnamedPlayer";
    public int score = 0;
    public int scoreMultiplier = 10;
    [Header("Rewards")]
    public GameObject weaponRewardPrefab;
    public GameObject healthRewardPrefab; 
    [Space(10)]
    private UIGameController uIGameController;
    public TMP_Text debugText; 
    
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

    public void Log(string text)
    {
       
        if (debugText)
        {
            debugText.text += "\n" + text;
        }
    }

    public class Record
    {
        public string username;
        public int score;
    }
    public GameState gameState = GameState.intro;
    public enum GameState { intro, startMenu, play, gameOver }

    public void AddScore(int newScore) {
        score += newScore * scoreMultiplier;
        if (!uIGameController)
        {
            uIGameController = GameObject.Find("UI").GetComponent<UIGameController>();
        }
        if(uIGameController)
        {
            uIGameController.UpdateUIInfo();
        }
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
        // Log("Game state changed, now: " + newGameState.ToString()); 
        gameState = newGameState;
        /*  if(debugGameStateText) {
            debugGameStateText.text = gameState.ToString(); 
        }*/
        if(gameState==GameState.gameOver)
        {
            StartCoroutine("GameOver"); 
            
        }
        
    }
    public GameState GetGameSate()
    {
        return gameState; 
        
    }
    IEnumerator GameOver()
    {
        
        yield return new WaitForSeconds(1f);
        if (!uIGameController) uIGameController = GameObject.Find("UI").GetComponent<UIGameController>();
        uIGameController.gameOverPanel.SetActive(true);
        yield return null; 
    }
}
