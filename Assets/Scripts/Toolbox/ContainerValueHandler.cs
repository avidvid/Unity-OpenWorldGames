using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ContainerValueHandler : MonoBehaviour {
    
    private Text _text;

    public float FillAmount;

    // Use this for initialization
    void Start () {
        var texts = GetComponentsInChildren<Text>();
        foreach (var text in texts)
            _text = text;
    }

    public void UpdateValue(int fillAmount)
    {
        _text.text = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", fillAmount);
        FillAmount = fillAmount;
    }
}
