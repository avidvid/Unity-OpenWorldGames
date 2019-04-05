using System;
using UnityEngine;

[Serializable]
public class UserCharacter 
{
    public int CharacterId { get; set; }
    public int UserId { get; set; }
    public string CharacterCode { get; set; }
    public DateTime Created { get; set; }
    public int HealthCheck { get; set; }
    public UserCharacter()
    {
        CharacterId = -1;
        UserId = -1;
        CharacterCode = null;
        Created = DateTime.Now;
        HealthCheck = 0;
    }
    public UserCharacter(int characterId, int playerId, string characterCode = null)
    {
        CharacterId = characterId;
        UserId = playerId;
        CharacterCode = characterCode;
        Created = DateTime.Now;
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