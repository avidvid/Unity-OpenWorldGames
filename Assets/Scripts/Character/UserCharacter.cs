using System;
using UnityEngine;

[Serializable]
public class UserCharacter 
{
    public int CharacterId { get; set; }
    public int UserId { get; set; }
    public string CharacterCode { get; set; }
    public DateTime Created { get; set; }

    public UserCharacter()
    {
        CharacterId = -1;
        UserId = -1;
        CharacterCode = null;
        Created = DateTime.Now;
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

        //ItemDatabase itemDatabase = ItemDatabase.Instance();
        //try
        //{
        //    var recipe = itemDatabase.FindRecipes(RecipeId);
        //    recipe.Print();
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError(e.Message);
        //}
    }

}
