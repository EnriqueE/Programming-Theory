using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
   public static GameController instance;
    /// <summary>
    /// Controls de Game
    /// </summary>

    private string m_UserName = "UnnamedPlayer"; 
    public string UserName
    {
        set
        {
            if(value.ToString().Length < 4)
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
    private void ShowErrorMessage(string title, string content)
    {

    }
    public class Record
    {
        public string username;
        public int score; 
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
        Debug.Log("Starting the Game");
    }
}
