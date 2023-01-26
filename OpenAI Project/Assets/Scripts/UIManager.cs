using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public const string HINT_TEXT = "Press {0} to interact";
    
    [HideInInspector] public UnityEvent<string> onSendTextAfterValidate;
    
    public GameManager gameManager;
    
    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TMP_InputField dialogueInputField;
    public Button dialogueSendButton;

    [Header("Incoming Dialogue")]
    public GameObject incomingDialoguePanel;
    public TMP_Text characterResponseText;
    public GameObject hourGlassIcon;

    public GameObject interactHintPanel;
    public TextMeshProUGUI interactHintText;

    public StarterAssetsInputs fpsInput;
    public FirstPersonController firstPersonController;
    public PlayerInput playerInput;

    public void SendTextClicked()
    {
       if( ValidateText())
       {
           onSendTextAfterValidate.Invoke(dialogueInputField.text);
           dialogueInputField.text = string.Empty;
           ToggleDialoguePanel();
           //  ToggleDialoguePanel();
       }
    }

    [ContextMenu("Toggle Panel")]
    public void ToggleDialoguePanel()
    {
        if(dialogueInputField.isFocused)
            return;
        dialoguePanel.SetActive(!dialoguePanel.activeSelf);
        fpsInput.cursorInputForLook = !dialoguePanel.activeSelf;
        fpsInput.cursorLocked = !dialoguePanel.activeSelf;
        fpsInput.enabled = !dialoguePanel.activeSelf;
        firstPersonController.enabled = !dialoguePanel.activeSelf;
        fpsInput.move = Vector2.zero;
        fpsInput.look = Vector2.zero;
        Cursor.visible = dialoguePanel.activeSelf;
        Cursor.lockState = !dialoguePanel.activeSelf ? CursorLockMode.Locked : CursorLockMode.Confined;
        playerInput.enabled = !dialoguePanel.activeSelf;
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
        gameManager.networkManager.onCharacterPromptSent.AddListener(delegate(string arg0) 
            { hourGlassIcon.SetActive((true));  });
        gameManager.networkManager.onCharacterResponseRecieved.AddListener(delegate(string arg0) 
            { hourGlassIcon.SetActive((false));  });
    }

    public void ShowHintPanel(NPCController npcController)
    {
        interactHintPanel.SetActive(true);
        incomingDialoguePanel.SetActive(true);
    }

    public void HideHintPanel(NPCController npcController)
    {
        interactHintPanel.SetActive(false);
    }
    
    public void SetCharacterResponse(string textToSet)
    {
        characterResponseText.text = textToSet;
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
