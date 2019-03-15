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

    private float _maxValue;
    private float _fillAmount;
    private int _level; 

    public Color FullColor;
    public Color LowColor;
    public bool LerpColor;

    // Use this for initialization
    void Start ()
    {
        var contents = GetComponentsInChildren<Image>();
        foreach (var content in contents)
            if (content.type ==Image.Type.Filled)
                _content = content;
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
        _fillAmount = Mathf.Clamp(_fillAmount, 0, _maxValue);
        float mappedValue = MapConvert(_fillAmount, _maxValue);
        if (_content.fillAmount != mappedValue)
        {
            if (_level == -1)
                _text.text = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", _fillAmount);
            else
                _text.text = "Level: "+ _level;
            //Doing this with lerp Over time =>  _content.fillAmount = MapConvert(FillAmount, 100) 
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, mappedValue, Time.deltaTime * _lerpSpeed);
            if (LerpColor)
                _content.color = Color.Lerp(LowColor, FullColor, mappedValue);
        }
    }

    private void ChangeFillAmount(float amount)
    {
        if (amount > 0)
        {
            _fillAmount += amount;
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
        _fillAmount = fillAmount;
        _maxValue = maxValue;
        _level = level;
    }

}
