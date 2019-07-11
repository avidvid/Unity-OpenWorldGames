using System;
using UnityEngine;

[Serializable]
public class MailMessage
{
    public int Id;
    public int SenderId;
    public int ReceiverId;
    public int ReceiverClanId;
    public string SendTime;
    public string Title;
    public string Body;
    public bool Read;
    public bool IsPublic;
    public bool Deleted;

    public MailMessage(string title, string body,int senderId, int receiverId, int receiverClanId=0 )
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        SenderId = senderId;
        ReceiverId = receiverId;
        ReceiverClanId = receiverClanId;
        SendTime = "Now";
        Title = title;
        Body = body;
        Read = false;
        IsPublic = false;
        Deleted = false;
    }

    #region Print
    internal void Print()
    {
        Debug.Log("MailMessage: " + MyInfo());
    }
    internal string MyInfo()
    {
        UserDatabase userDatabase = UserDatabase.Instance();
        try
        {
            return Id + "-" + Title +
                   " From "+ userDatabase.GetUserById(SenderId).UserName  +
                   " To " + (ReceiverClanId ==0 ? userDatabase.GetUserById(ReceiverId).UserName : "All Clan XXX ") +
                   " at " + SendTime +
                   (Read ? " (Read)" : "") +
                   (IsPublic ? " (IsPublic)" : "") +
                   (Deleted ? " (Deleted)" : "");
        }
        catch (Exception e)
        {
            return "Empty";
        }
    }
    internal string GetTooltip()
    {
        var tooltip = "<color=green>  " + Body  + "</color>";
        return tooltip;
    }
    internal string GetInfo()
    {
        UserDatabase userDatabase = UserDatabase.Instance();
        try
        {
            return "From " + userDatabase.GetUserById(SenderId).UserName ;
        }
        catch (Exception e)
        {
            return "Unknown";
        }
    }
    #endregion
}