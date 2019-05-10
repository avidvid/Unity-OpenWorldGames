using System;
using UnityEngine;

[Serializable]
public class UserCharacter
{
    public int Id;
    public int CharacterId;
    public int UserId;
    public string CharacterCode;
    public DateTime Created;
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
    public void Print()
    {
        CharacterDatabase characterDatabase = CharacterDatabase.Instance();
        try
        {
            Character character  = characterDatabase.GetCharacterById(CharacterId);
            character.Print();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}