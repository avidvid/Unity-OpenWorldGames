using System;


[Serializable]
public class ApiRequest
{
    public string Action= "UpdateUserPlayer";
    public UserPlayer UserPlayer;
    public CharacterSetting CharacterSetting;
    public CharacterMixture CharacterMixture;
    public CharacterResearching CharacterResearching;

    public CharacterResearch CharacterResearch;
    public UserCharacter UserCharacter;
    public UserRecipe UserRecipe;

    //public List<UserItem> UserInventory;

    public int Time;
    public int Code;
    public int HealthCheck;

    internal string MyInfo()
    {
        string info = "Action ="+ Action + " HealthCheck=" + HealthCheck;
        if (UserPlayer.Id != 0) info += " UserPlayer=" + UserPlayer.MyInfo();
        if (CharacterSetting.Id != 0) info += " CharacterSetting=" + CharacterSetting.MyInfo();
        if (CharacterMixture.Id != 0) info += " CharacterMixture=" + CharacterMixture.MyInfo();
        if (CharacterResearching.Id != 0) info += " CharacterResearching=" + CharacterResearching.MyInfo();
        if (CharacterResearch.Id != 0) info += " CharacterResearch=" + CharacterResearch.MyInfo();
        return info;
    }
}