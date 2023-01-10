using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCController : MonoBehaviour
{

    public NPCControllerEvent onPlayerEnterTrigger = new NPCControllerEvent();
    public NPCControllerEvent onPlayerExitTrigger  = new NPCControllerEvent();
        
    public void OnTriggerEnter(Collider other)
    {
        onPlayerEnterTrigger.Invoke(this);
        Debug.Log("Entered Trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        onPlayerExitTrigger.Invoke(this);
        Debug.Log(("Exited Trigger"));
    }
}
