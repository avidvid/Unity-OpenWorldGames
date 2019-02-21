using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchData : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    // Use this for initialization
    internal Research Research;
    internal int Level;

    private Tooltip _tooltip;

    void Start()
    {
        _tooltip = Tooltip.Instance();
        if (Research == null)
            return;

        var images = GetComponentsInChildren<Image>();
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        var buttons = GetComponentsInChildren<Button>();
        images[1].sprite = Research.GetSprite();
        texts[0].text = "Level "+ Level;
        if (Level >= Research.MaxLevel)
        {
            buttons[0].interactable = false;
            texts[1].text = "MAX";
        }
        else
            texts[1].text = Research.CalculatePrice(Level).ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Research == null)
            return;
        _tooltip.Activate(Research);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.Deactivate();
    }
}