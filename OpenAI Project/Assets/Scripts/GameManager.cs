using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
  
    public NetworkManager networkManager;
    public UIManager uiManager;
    public NPCController[] npcControllers;
    public KeyCode interactButton;

    [HideInInspector] public UnityEvent onInteractWithNPC;

    private bool nearNPC
    {
        get
        {
           return closestNPC != null;
        }
    }

    private NPCController closestNPC;

    private void Start()
    {
        for (int i = 0; i < npcControllers.Length; i++)
        {
            npcControllers[i].onPlayerEnterTrigger.AddListener(RegisterCloseNPC);
            npcControllers[i].onPlayerExitTrigger.AddListener(RemoveCloseNPC);
        }
        networkManager.onCharacterResponseRecieved.AddListener(uiManager.SetCharacterResponse);
        uiManager.onSendTextAfterValidate.AddListener(networkManager.SendPromptToServer);
    }

    private void RegisterCloseNPC(NPCController npcController)
    {
        closestNPC = npcController;
    }
    
    private void RemoveCloseNPC(NPCController npcController)
    {
        closestNPC = null;
        uiManager.incomingDialoguePanel.SetActive(false);
        uiManager.characterResponseText.text = string.Empty;
    }

    private void OnValidate()
    {
        npcControllers = FindObjectsOfType<NPCController>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(interactButton) && nearNPC)
        {
            onInteractWithNPC.Invoke();
        }

        if (uiManager.dialoguePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            uiManager.SendTextClicked();
        }
    }
}
