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
    private DateTime _sendTime;
    public string Title;
    public string Body;
    public bool Delivered;
    public bool Read;
    public bool Deleted;

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
                   " To " + (ReceiverClanId !=0 ? userDatabase.GetUserById(ReceiverId).UserName : "All Clan XXX ") +
                   " at " + SendTime +
                   (Delivered ? " Delivered" : "") +
                   (Read ? " Read" : "") +
                   (Deleted ? " Deleted" : "");
        }
        catch (Exception e)
        {
            return "Empty";
        }
    }
    #endregion
}