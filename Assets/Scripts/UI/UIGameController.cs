using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class UIGameController : MonoBehaviour
{
    public GameObject debugPanel;
    public TMP_Text debugText; 
    public TMP_Text healthText;
    public TMP_Text weaponLevelText;
    public TMP_Text scoreText; 
    private PlayerController playerController;
    public GameObject gameOverPanel; 

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>(); 
    }
    public void UpdateUIInfo()
    {
        //Debug.Log(playerController.health.ToString());
     
            healthText.text = playerController.health.ToString();
            weaponLevelText.text = (playerController.currentWeaponLevel + 1).ToString();
       
            scoreText.text = GameController.instance.score.ToString(); 

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
    }
  
}
