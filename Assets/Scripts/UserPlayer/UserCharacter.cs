using System;
using UnityEngine;

[Serializable]
public class UserCharacter
{
    public int Id;
    public int CharacterId;
    public int UserId;
    public string CharacterCode;
    public UserCharacter()
    {
        CharacterId = 0;
        UserId = 0;
        CharacterCode = "";
    }
    public UserCharacter(int characterId, int playerId, string characterCode = null)
    {
        CharacterId = characterId;
        UserId = playerId;
        CharacterCode = characterCode;
    }
    internal void Print()
    {
        Debug.Log("UserCharacter: " + MyInfo());
    }
    internal string MyInfo()
    {
        CharacterDatabase characterDatabase = CharacterDatabase.Instance();
        try
        {
            var character = characterDatabase.GetCharacterById(CharacterId);
            return Id + "-" + character.MyInfo() + " CharacterCode= " + CharacterCode;
        }
        catch (Exception e)
        {
            return "Empty";
        }
    }
}