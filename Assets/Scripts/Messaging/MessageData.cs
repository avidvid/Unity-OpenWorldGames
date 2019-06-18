using UnityEngine;
using UnityEngine.EventSystems;

public class MessageData : MonoBehaviour, IPointerClickHandler
{
    public MailMessage MessageIns;
    public bool InComing; 
    public UserPlayer TheOtherPlayer;
    void Start()
    {
        if (MessageIns == null)
            return;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (MessageIns == null)
            return;
        var mailMessageHandler = FindObjectOfType(typeof(MailMessageHandler)) as MailMessageHandler;
        if (!mailMessageHandler)
            return;
        mailMessageHandler.ActiveMessage = MessageIns;
        mailMessageHandler.LoadNew = true;
    }
}