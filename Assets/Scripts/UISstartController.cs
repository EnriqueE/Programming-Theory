using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISstartController : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button changeNameButton;
    [SerializeField] private Button saveNameButton;
    [SerializeField] private Text userNameText;
    [SerializeField] private InputField userNameInput;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorPanelText; 

    
    public void ChangeName()
    {        
        userNameText.gameObject.SetActive(false);
        userNameInput.gameObject.SetActive(true);
        userNameInput.Select();
        changeNameButton.gameObject.SetActive(false);
        saveNameButton.gameObject.SetActive(true); 
    }
    public void SaveName()
    {
        string name = userNameInput.text;
        Debug.Log("Saving name: " + name);
        if(name.Length < 4 )
        {
            StartCoroutine("ShowError", "The username must contain at least 4 characters");
            return; 
        }
        userNameText.text = name;
        GameController.instance.UserName = name;
        userNameInput.gameObject.SetActive(false);
        saveNameButton.gameObject.SetActive(false);
        changeNameButton.gameObject.SetActive(true);
        userNameText.gameObject.SetActive(true); 
    }

    private IEnumerator ShowError(string content)
    {        
        errorPanelText.text = content;
        errorPanel.gameObject.SetActive(true); 
        yield return new WaitForSeconds(3);
        errorPanel.gameObject.SetActive(false); 
    }

}
