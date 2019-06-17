using UnityEngine;
using UnityEngine.EventSystems;

public class MessageData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MailMessage MessageIns;
    public bool InComing; 
    public UserPlayer TheOtherPlayer;
    private Tooltip _tooltip;

    void Start()
    {
        _tooltip = Tooltip.Instance();
        if (MessageIns == null)
            return;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.Activate(MessageIns);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.Deactivate();
    }
}