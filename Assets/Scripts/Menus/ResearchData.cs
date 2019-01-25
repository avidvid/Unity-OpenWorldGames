using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchData : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    // Use this for initialization
    public Research Research;
    public CharacterResearch CharacterResearch;

    private Tooltip _tooltip;

    void Start()
    {
        _tooltip = Tooltip.Instance();
        if (Research == null)
            return;

        var images = GetComponentsInChildren<Image>();
        var texts = GetComponentsInChildren<Text>();
        var buttons = GetComponentsInChildren<Button>();
        images[1].sprite = Research.GetSprite();
        int nextLevel = CharacterResearch != null ? CharacterResearch.Level+1 : 1;
        texts[0].text = "Level "+ nextLevel;
        if (nextLevel >= Research.MaxLevel)
        {
            buttons[0].interactable = false;
            texts[1].text = "MAX";
        }
        else
            texts[1].text = Research.CalculatePrice(nextLevel).ToString();
    }

    void Update()
    {
    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.Activate(Research);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.Dectivate();
    }

}
