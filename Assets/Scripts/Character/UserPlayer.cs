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

    public int Id { get; private set; }
    public string UserName { get; set; }
    public string Description { get; set; }
    public float SoundVolume { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime LockUntil { get; set; }
    public UserPlayer.PlayerRanks Rank { get; set; }
    public int PRank { get; set; }
    public int ClanId { get; set; }
    public UserPlayer.ClanRanks ClanRank { get; set; }
    public string CharacterList { get; set; }
    public bool FBLoggedIn { get; set; }
    public string FBid { get; set; }
    public bool GLoggedIn { get; set; }
    public bool IsEnable { get; set; }

    public UserPlayer(int id = -1, 
                string userName = null, string description = null, float soundVolume =0,
                float latitude = 0, float longitude = 0,
                float baseLatitude = 0, float baseLongitude = 0,
                DateTime lastLogin = new DateTime(),
                DateTime lockUntil = new DateTime(),
                UserPlayer.PlayerRanks rank = 0, int pRank = 0, int clanId = 0, UserPlayer.ClanRanks clanRank = 0,
                string characterList = "0,1,2,3,4",
                bool fb = false, string fbId = null,
                bool g = false, bool isEnable = true)
    {
        Id = id;
        UserName = userName;
        Description = description;
        SoundVolume = soundVolume;
        Latitude = latitude;
        Longitude = longitude;
        LastLogin = lastLogin;
        LockUntil = lockUntil;
        Rank = rank;
        PRank = pRank;
        ClanId = clanId;
        ClanRank = clanRank;
        CharacterList = characterList;
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
        LastLogin = userPlayer.LastLogin;
        LockUntil = userPlayer.LockUntil;
        Rank = userPlayer.Rank;
        PRank = userPlayer.PRank;
        ClanId = userPlayer.ClanId;
        ClanRank = userPlayer.ClanRank;
        CharacterList = userPlayer.CharacterList; 
        FBLoggedIn = userPlayer.FBLoggedIn;
        FBid = userPlayer.FBid;
        GLoggedIn = userPlayer.GLoggedIn;
        IsEnable = userPlayer.IsEnable;
    }
    public UserPlayer()
    {
        Id = -1;
    }
    internal void Print()
    {
        Debug.Log("UserPlayer = " + Id + "-" + UserName + " " + Rank + " " + LastLogin+": (" + Latitude + ","+ Longitude + ") " 
                  + CharacterList + LockUntil );
    }
}
