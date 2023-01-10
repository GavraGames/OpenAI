using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class UIManager : MonoBehaviour
{
    public const string HINT_TEXT = "Press {0} to interact";
    
    public GameManager gameManager;
    
    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TMP_InputField dialogueInputField;
    public Button dialogueSendButton;

    public GameObject interactHintPanel;
    public TextMeshProUGUI interactHintText;

    public StarterAssetsInputs fpsInput;
    public FirstPersonController firstPersonController;

    public void SendTextClicked()
    {
       if( ValidateText())
       {
           dialogueInputField.text = string.Empty;
       }
    }

    [ContextMenu("Toggle Panel")]
    public void ToggleDialoguePanel()
    {
        dialoguePanel.SetActive(!dialoguePanel.activeSelf);
        fpsInput.cursorInputForLook = !dialoguePanel.activeSelf;
        fpsInput.cursorLocked = !dialoguePanel.activeSelf;
        fpsInput.enabled = !dialoguePanel.activeSelf;
        firstPersonController.enabled = !dialoguePanel.activeSelf;
        fpsInput.move = Vector2.zero;
        fpsInput.look = Vector2.zero;
        Cursor.visible = dialoguePanel.activeSelf;
        Cursor.lockState = !dialoguePanel.activeSelf ? CursorLockMode.Locked : CursorLockMode.Confined;
    }

    private void Start()
    {
        interactHintText.text = string.Format(HINT_TEXT, gameManager.interactButton.ToString());
        
        //events
        for (int i = 0; i < gameManager.npcControllers.Length; i++)
        {
            gameManager.npcControllers[i].onPlayerEnterTrigger.AddListener(ShowHintPanel);
            gameManager.npcControllers[i].onPlayerExitTrigger.AddListener(HideHintPanel);
        }
        gameManager.onInteractWithNPC.AddListener(delegate { ToggleDialoguePanel(); });
    }

    public void ShowHintPanel(NPCController npcController)
    {
        interactHintPanel.SetActive(true);
    }

    public void HideHintPanel(NPCController npcController)
    {
        interactHintPanel.SetActive(false);
    }
    
    bool ValidateText()
    {
        bool valid = true;
        if(dialogueInputField.text == string.Empty)
        {
            valid = false;
        }

        if (valid)
            Debug.Log("Text Good");
        else
            Debug.Log("Text Bad!");

        return valid;
    }

   
}
