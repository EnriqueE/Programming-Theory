using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    public GameObject debugPanel;
    public TMP_Text debugText; 
    public TMP_Text healthText;
    public TMP_Text weaponLevelText;
    public Text scoreText; 
    private PlayerController playerController;
    public GameObject gameOverPanel;
    public GameObject mainUIPanel;

    [Header("Lives")]
    public GameObject healthPanel;
    public GameObject livesUnit;
    public GameObject livesUnitEmpty;
    public GameObject livesUnitRed; 
    

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>(); 
    }
    public void UpdateUIInfo()
    {

        UpdateHealthInfo();
       
        scoreText.text = GameController.instance.score.ToString(); 

    }
    private void UpdateHealthInfo()
    {
        // Destroy all lives in livesPanel;
        foreach (Transform child in healthPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < playerController.health; i++)
        {
            GameObject prefab;
            int health = playerController.health;
            int currentHealth = playerController.currentHealth;
            float width = livesUnit.GetComponent<RectTransform>().rect.width;


            if (i < currentHealth)
            {
                if(currentHealth < 4)
                {
                    prefab = livesUnitRed;
                } else
                {
                    prefab = livesUnit;
                }
                
            } else
            {
                prefab = livesUnitEmpty;
            }
           
            GameObject liveUnitInstance = Instantiate(prefab, healthPanel.transform);
            liveUnitInstance.transform.Translate(Vector3.right * i * width  );
            liveUnitInstance.transform.Translate(Vector3.left * health * width / 2);
        }


    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
    }
  
}
