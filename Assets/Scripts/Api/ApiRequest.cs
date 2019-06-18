using System;


[Serializable]
public class ApiRequest
{
    public UserPlayer UserPlayer;
    public CharacterSetting CharacterSetting;
    public CharacterMixture CharacterMixture;
    public CharacterResearching CharacterResearching;
    public CharacterResearch CharacterResearch;
    public UserCharacter UserCharacter;
    public UserRecipe UserRecipe;
    public UserItem UserInventory;
    public MailMessage MailMessage;

    public string Action;
    public int Time;
    public string Code;
    public int HealthCheck;

    internal string MyInfo()
    {
        string info = "Action ="+ Action + " HealthCheck=" + HealthCheck;
        if (UserPlayer.Id != 0) info += " UserPlayer=" + UserPlayer.MyInfo();
        if (CharacterSetting.Id != 0) info += " CharacterSetting=" + CharacterSetting.MyInfo();
        if (CharacterMixture.Id != 0) info += " CharacterMixture=" + CharacterMixture.MyInfo();
        if (CharacterResearching.Id != 0) info += " CharacterResearching=" + CharacterResearching.MyInfo();
        if (CharacterResearch.Id != 0) info += " CharacterResearch=" + CharacterResearch.MyInfo();
        if (UserRecipe.Id != 0) info += " UserRecipe=" + UserRecipe.MyInfo();
        if (UserInventory.Id != 0) info += " UserInventory=" + UserInventory.MyInfo();
        if (MailMessage.Id != 0) info += " MailMessage=" + MailMessage.MyInfo();
        return info;
    }
}