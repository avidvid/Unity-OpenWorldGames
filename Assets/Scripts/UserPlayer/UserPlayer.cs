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
    public string LockUntil;
    public UserPlayer.PlayerRanks Rank;
    public int PRank;
    public int ClanId;
    public UserPlayer.ClanRanks ClanRank;
    public bool FBLoggedIn;
    public string FBid;
    public bool GLoggedIn;
    public bool IsEnable;

    public UserPlayer(int id = -1, 
                string userName = null, string description = null, float soundVolume =0,
                float latitude = 0, float longitude = 0,
                float baseLatitude = 0, float baseLongitude = 0, 
                int gem = 0,
                string lastLogin = "",
                string lockUntil = "",
                UserPlayer.PlayerRanks rank = 0, int pRank = 0, int clanId = 0, UserPlayer.ClanRanks clanRank = 0,
                bool fb = false, string fbId = null,
                bool g = false, bool isEnable = true)
    {
        Id = id;
        UserName = userName;
        Description = description;
        SoundVolume = soundVolume;
        Latitude = latitude;
        Longitude = longitude;
        Gem = gem;
        LastLogin = lastLogin;
        LockUntil = lockUntil;
        Rank = rank;
        PRank = pRank;
        ClanId = clanId;
        ClanRank = clanRank;
        FBLoggedIn = fb;
        FBid = fbId;
        GLoggedIn = g;
        IsEnable = isEnable;
    }
    public UserPlayer(UserPlayer userPlayer)
    {
        Id = userPlayer.Id;
        UserName = userPlayer.UserName;
        Description = userPlayer.Description;
        SoundVolume = userPlayer.SoundVolume;
        Latitude = userPlayer.Latitude;
        Longitude = userPlayer.Longitude;
        Gem = userPlayer.Gem;
        LastLogin = userPlayer.LastLogin;
        LockUntil = userPlayer.LockUntil;
        Rank = userPlayer.Rank;
        PRank = userPlayer.PRank;
        ClanId = userPlayer.ClanId;
        ClanRank = userPlayer.ClanRank;
        FBLoggedIn = userPlayer.FBLoggedIn;
        FBid = userPlayer.FBid;
        GLoggedIn = userPlayer.GLoggedIn;
        IsEnable = userPlayer.IsEnable;
    }
    public UserPlayer()
    {
    }
    #region Print
    internal void Print()
    {
        Debug.Log("UserPlayer = " + MyInfo());
    }
    internal string MyInfo()
    {
        return Id + "-" + UserName + 
               " Rank =" + Rank + 
               "(" + Latitude + "," + Longitude + ")"+
               " Gem=" + Gem +
               " FBLoggedIn=" + FBLoggedIn +
               " FBid=" + FBid +
               " Description=" + Description +
               " SoundVolume=" + SoundVolume +
               " LastLogin=" + LastLogin +
               " LockUntil=" + LockUntil; 
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