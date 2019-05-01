using System;


[Serializable]
public class ApiRequest
{
    public string Action= "UpdateUserPlayer";
    public UserPlayer UserPlayer;

    internal string MyInfo()
    {
        string info = "Action ="+ Action;
        if (UserPlayer.Id != 0) info += " UserPlayer=" + UserPlayer.MyInfo();
        return info;
    }
}