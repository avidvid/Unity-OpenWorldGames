using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{

    private static Tooltip _tooltip;

    public GameObject TooltipObject;


    // Use this for initialization
    void Start ()
    {
        _tooltip = Tooltip.Instance();
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

    public void Activate(ItemContainer item)
    {
        if (item.Id != -1)
        {
            _tooltip.transform.GetChild(0).GetComponent<Text>().text = item.GetTooltip();
            TooltipObject.SetActive(true);
            TooltipObject.transform.position = Input.mousePosition;
        }
    }

    public void Activate(Research research)
    {
        _tooltip.transform.GetChild(0).GetComponent<Text>().text = research.GetTooltip();
        TooltipObject.SetActive(true);
        TooltipObject.transform.position = Input.mousePosition;
    }


    public void Dectivate()
    {
        TooltipObject.SetActive(false);
    }

}
