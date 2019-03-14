using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour {
    
    //
    public GUISkin Skin;

    //Messaging board variables
    private string _alert;
    private List<MessageBoard> _messageBoardQueue = new List<MessageBoard>();
    private float _windowWidth = 300;
    private int _messageLength = 5;
    private Rect _alertBox;
    private bool _coRoutineExecuting = false;

    //Stats box variables
    //private int _coin;
    //private int _gem;
    //private int _live;
    //private int _level;
    //private int _experience;
    //private int _rank;
    //private string _name;
    
    private static GUIManager _GUIManager;

    public static GUIManager Instance()
    {
        if (!_GUIManager)
        {
            _GUIManager = FindObjectOfType(typeof(GUIManager)) as GUIManager;
            if (!_GUIManager)
                Debug.LogError("There needs to be one active GUIManager script on a GameObject in your scene.");
        }
        return _GUIManager;
    }
    void Start()
    {
        PrintMessage("Welcome To AUSTIN Map",Color.blue);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            RemoveMessage();
        if (!_coRoutineExecuting)
            if (_messageBoardQueue.Count != 0)
                StartCoroutine(RemoveMessage(2, _messageBoardQueue.Count));
    }
    void OnGUI()
    {
        if (_alert != "")
            GUI.Box(_alertBox, _alert, Skin.GetStyle("box"));
    }
    public void PrintMessage(string message, Color color)
    {
        var newMessage = new MessageBoard(message, color);
        if (_messageBoardQueue.Count == _messageLength)
        {
            if (_messageBoardQueue.IndexOf(newMessage)>0)
                return;
            _messageBoardQueue.RemoveAt(0);
        }
        _messageBoardQueue.Add(newMessage);
        _alertBox = BuildAlertBox();
    }
    private void RemoveMessage()
    {
        if (_messageBoardQueue.Count == 0)
            return;
        _messageBoardQueue.RemoveAt(0);
        _alertBox = BuildAlertBox();
    }
    IEnumerator RemoveMessage(float time,int cnt)
    {
        _coRoutineExecuting = true;
        yield return new WaitForSeconds(time);
        // RemoveMessage execute after the delay and if the size of queue haven't change
        if (cnt == _messageBoardQueue.Count)
            RemoveMessage();
        _coRoutineExecuting = false;
    }
    private Rect BuildAlertBox()
    {
        _alert = BuildAlertString();
        float windowHeight = Skin.box.CalcHeight(new GUIContent(_alert), _windowWidth);
        float x = (Screen.width - _windowWidth) / 2;
        float y = 20;
        return new Rect(x, y, _windowWidth, windowHeight);
    }
    private string BuildAlertString()
    {
        string alert = "";
        foreach (var msg in _messageBoardQueue)
            alert += "<color=" + ColorName(msg.TextColor) + ">" + msg.Text + "</color>\n";
        return alert.Trim();
    }
    private string ColorName(Color color)
    {
        if (color.Compare(Color.yellow))
            return "yellow";
        if (color.Compare(Color.black))
            return "black";
        if (color.Compare(Color.green))
            return "green";
        if (color.Compare(Color.blue))
            return "blue";
        if (color.Compare(Color.red))
            return "red";
        return "white";
    }
}
