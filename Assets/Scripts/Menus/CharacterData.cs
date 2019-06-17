using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character SlotCharacter;
    private Tooltip _tooltip;

    void Start()
    {
        _tooltip = Tooltip.Instance();
        if (SlotCharacter == null)
            return;

        //GetComponentsInChildren<Image>()[1].sprite = SlotCharacter.GetSprite();
        //GetComponentInChildren<TextMeshProUGUI>().text = SlotCharacter.Name;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.Activate(SlotCharacter);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.Deactivate();
    }
}