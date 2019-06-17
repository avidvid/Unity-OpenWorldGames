using System;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    private static Tooltip _tooltip;

    public GameObject TooltipObject;

    void Awake()
    {
        _tooltip = Tooltip.Instance();
    }
    //internal void Activate(ItemContainer item)
    //{
    //    if (item != null)
    //    {
    //        _tooltip.transform.GetComponentInChildren<TextMeshProUGUI>().text = item.GetTooltip();
    //        TooltipObject.SetActive(true);
    //        TooltipObject.transform.position = Input.mousePosition;
    //    }
    //}

    public void Activate(ItemIns itemIns)
    {
        if (itemIns != null)
        {
            _tooltip.transform.GetComponentInChildren<TextMeshProUGUI>().text = itemIns.GetTooltip();
            TooltipObject.SetActive(true);
            TooltipObject.transform.position = Input.mousePosition;
        }
    }
    public void Activate(Research research)
    {
        _tooltip.transform.GetComponentInChildren<TextMeshProUGUI>().text = research.GetTooltip();
        TooltipObject.SetActive(true);
        TooltipObject.transform.position = Input.mousePosition;
    }
    public void Activate(Character character)
    {
        _tooltip.transform.GetComponentInChildren<TextMeshProUGUI>().text = character.GetTooltip();
        TooltipObject.SetActive(true);
        TooltipObject.transform.position = Input.mousePosition;
    }
    public void Activate(MailMessage messageIns)
    {
        _tooltip.transform.GetComponentInChildren<TextMeshProUGUI>().text = messageIns.GetTooltip();
        TooltipObject.SetActive(true);
        TooltipObject.transform.position = Input.mousePosition;
    }
    public void Deactivate()
    {
        TooltipObject.SetActive(false);
    }
    public static Tooltip Instance()
    {
        if (!_tooltip)
        {
            _tooltip = FindObjectOfType(typeof(Tooltip)) as Tooltip;
            if (!_tooltip)
                Debug.LogError("There needs to be one active Tooltip script on a GameObject in your scene.");
        }
        return _tooltip;
    }
}