using System;
using UnityEngine;

[Serializable]
public class UserPlayer  {

    public enum ClanRanks
    {
        UnRanked,
        User,
        Commander,
        Chief
    }
    public enum PlayerRanks
    {
        UnRanked,
        Bronze3,
        Bronze2,
        Bronze1,
        Silver3,
        Silver2,
        Silver1,
        Gold3,
        Gold2,
        Gold1,
        Platinum3,
        Platinum2,
        Platinum1,
        Diamond3,
        Diamond2,
        Diamond1,
        Champion3,
        Champion2,
        Champion1,
        Master3,
        Master2,
        Master1,
        Legend3,
        Legend2,
        Legend1,
        Saint
    }

    public int Id;
    public string UserName;
    public string Description;
    public float SoundVolume;
    public float Latitude;
    public float Longitude;
    public int Gem;
    public string LastLogin;
    public int LockUntil;
    public UserPlayer.PlayerRanks Rank;
    public int PRank;
    public int ClanId;
    public UserPlayer.ClanRanks ClanRank;
    public bool FBLoggedIn;
    public bool GLoggedIn;
    public string FBid;
    public string Gid;
    public string MacId;
    public bool IsEnable;

    public UserPlayer(string userName, string description)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        UserName = userName;
        Description = description;
        SoundVolume = 0.1f;
        Latitude = 0;
        Longitude = 0;
        Gem = 5;
        LastLogin = "";
        LockUntil = 0;
        Rank = 0;
        PRank = 0;
        ClanId = 0;
        ClanRank = 0;
        FBLoggedIn = false;
        FBid = "Facebook";
        Gid = "Google";
        MacId = "Mac";
        GLoggedIn = false;
        IsEnable = true;
    }
    #region Print
    internal void Print()
    {
        Debug.Log("UserPlayer = " + MyInfo());
    }
    internal string MyInfo()
    {
        try
        {
            return Id + "-" + UserName +
                   " Rank =" + Rank +
                   "(" + Latitude + "," + Longitude + ")" +
                   " Gem=" + Gem +
                   " FBLoggedIn=" + FBLoggedIn +
                   " FBid=" + FBid +
                   " Gid=" + Gid +
                   " MacId=" + MacId +
                   " Description=" + Description +
                   " SoundVolume=" + SoundVolume +
                   " LastLogin=" + LastLogin +
                   " LockUntil=" + LockUntil;
        }
        catch (Exception e)
        {
            return "Empty UserPlayer";
        }
    }
    #endregion
    #region HealthCheck
    internal int CalculateHealthCheck()
    {
        return
            (this.Gem * (int)FieldCode.Gem +
             0)
            * RandomHelper.StringToRandomNumber(UserName)
            ;
    }
    internal int CalculateHealthCheckByField(int value, string field)
    {
        var fieldCode = (int)GetFieldCode(field);
        return value * fieldCode * RandomHelper.StringToRandomNumber(UserName);
    }
    private static FieldCode GetFieldCode(string field)
    {
        switch (field)
        {
            case "Gem":
                return FieldCode.Gem;
            default:
                return FieldCode.None;
        }
    }
    public enum FieldCode
    {
        //should match with API Side
        None = 0,
        Gem = 1,
    }
    internal int CalculateHealthCheck(int value, string field)
    {
        int fieldCode;
        switch (field)
        {
            case "Gem":
                fieldCode = 1;
                break;
            default:
                fieldCode = 0;
                break;
        }
        return value * fieldCode * RandomHelper.StringToRandomNumber(UserName);
    }
    #endregion
}