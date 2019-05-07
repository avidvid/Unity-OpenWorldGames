using System;


[Serializable]
public class ApiRequest
{
    public string Action= "UpdateUserPlayer";
    public UserPlayer UserPlayer;
    public CharacterSetting CharacterSetting;
    public CharacterMixture CharacterMixture;
    public CharacterResearching CharacterResearching;

    //public List<UserItem> UserInventory;
    //public List<UserCharacter> UserCharacters;
    //public List<CharacterResearch> CharacterResearches;
    //public List<UserRecipe> UserRecipes;

    public int Time;
    public int HealthCheck;

    internal string MyInfo()
    {
        string info = "Action ="+ Action + " HealthCheck=" + HealthCheck;
        if (UserPlayer.Id != 0) info += " UserPlayer=" + UserPlayer.MyInfo();
        return info;
    }
}