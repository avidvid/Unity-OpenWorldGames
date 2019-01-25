using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MessageBoard
{
    public MessageBoard(string text, Color textColor, MessageType type=MessageType.None)
    {
        Text = text;
        TextColor = textColor;
        Type = type;
    }

    public string Text { get; set; }
    public Color TextColor { get; set; }
    public MessageType Type { get; set; }

}
