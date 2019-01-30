using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour
{
    [SerializeField]
    private float _lerpSpeed=2;

    private Image _content;
    private TextMeshProUGUI _text;

    //todo: make these private 
    public float MaxValue;
    public float FillAmount;
    public int Level; 

    public Color FullColor;
    public Color LowColor;
    public bool LerpColor;

    // Use this for initialization
    void Start ()
    {
        var contents = GetComponentsInChildren<Image>();
        foreach (var conten in contents)
            if (conten.type ==Image.Type.Filled)
                _content = conten;
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
                _text = text;
        if (LerpColor)
            _content.color = FullColor;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    ShowBarStat();
	}

    private void ShowBarStat()
    {
        FillAmount = Mathf.Clamp(FillAmount, 0, MaxValue);
        float mappedValue = MapConvert(FillAmount, MaxValue);
        if (_content.fillAmount != mappedValue)
        {
            if (Level == -1)
                _text.text = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", FillAmount);
            else
                _text.text = "Level: "+ Level;
            //Doing this with lerp Over time =>  _content.fillAmount = MapConvert(FillAmount, 100) 
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, mappedValue, Time.deltaTime * _lerpSpeed);
            if (LerpColor)
                _content.color = Color.Lerp(LowColor, FullColor, mappedValue);
        }
    }

    private void ChangeFillAmount(float amount)
    {
        if (amount != 0)
        {
            FillAmount += amount;
            ShowBarStat();
        }
    }


    private float MapConvert(float value, float inMax)
    {
        return MapConvert(value, 0, inMax, 0, 1);
    }

    private float MapConvert(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //Map the value between inMin and inMax to a value between outMin and outMax
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void UpdateValues(int fillAmount, int maxValue,int level = -1)
    {
        FillAmount = fillAmount;
        MaxValue = maxValue;
        Level = level;
    }

}
