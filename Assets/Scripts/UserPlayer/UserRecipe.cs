using System;
using UnityEngine;

[Serializable]
public class UserRecipe
{
    public int Id;
    public int RecipeId;
    public int UserId;
    public string RecipeCode;

    public UserRecipe()
    {
        RecipeId = 0;
        UserId = 0;
        RecipeCode = "";
    }
    public UserRecipe(int recipeId, int playerId, string recipeCode = null)
    {
        RecipeId = recipeId;
        UserId = playerId;
        RecipeCode = recipeCode;
    }

    public void Print()
    {
        ItemDatabase itemDatabase= ItemDatabase.Instance();
        try
        {
            var recipe = itemDatabase.FindRecipe(RecipeId);
            recipe.Print();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}