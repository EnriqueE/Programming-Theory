using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class UIGameController : MonoBehaviour
{
    public GameObject debugPanel;
    public TMP_Text healthText;
    public TMP_Text weaponLevelText;
    public TMP_Text scoreText; 
    private PlayerController playerController;

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
}
