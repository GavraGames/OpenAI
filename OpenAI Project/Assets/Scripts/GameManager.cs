using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
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
            npcControllers[i].onPlayerExitTrigger.AddListener(RegisterCloseNPC);
        }
    }

    private void RegisterCloseNPC(NPCController npcController)
    {
        closestNPC = npcController;
    }
    
    private void RemoveCloseNPC(NPCController npcController)
    {
        closestNPC = null;
    }

    private void OnValidate()
    {
        npcControllers = FindObjectsOfType<NPCController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactButton) && nearNPC)
        {
            onInteractWithNPC.Invoke();
        }
    }
}
