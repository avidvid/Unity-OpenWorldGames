
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class MessagePanelHandler : MonoBehaviour
{

    private static MessagePanelHandler _messagePanelHandler;

    void Start()
    {
        _messagePanelHandler = MessagePanelHandler.Instance();
    }


    public static MessagePanelHandler Instance()
    {
        if (!_messagePanelHandler)
        {
            _messagePanelHandler = FindObjectOfType(typeof(MessagePanelHandler)) as MessagePanelHandler;
            if (!_messagePanelHandler)
                Debug.LogError("There needs to be one active MessagePanelHandler script on a GameObject in your scene.");
        }
        return _messagePanelHandler;
    }

    
    internal void ShowMessage(string message, MessagePanel.PanelType yesNo, UnityAction firstEvent = null, UnityAction secondEvent = null)
    {
        var messageBoxPref = Resources.Load<GameObject>("Prefabs/MessagePanel");
        GameObject messagePanel = Instantiate(messageBoxPref);
        var messenger = messagePanel.GetComponent<MessagePanel>();
        messenger.ShowMessage(message, yesNo, firstEvent, secondEvent);
    }
}

