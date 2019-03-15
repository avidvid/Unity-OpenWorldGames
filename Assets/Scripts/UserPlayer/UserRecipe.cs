using System;
using UnityEngine;

[Serializable]
public class UserRecipe
{
    public int RecipeId { get; set; }
    public int UserId { get; set; }
    public string RecipeCode { get; set; }
    public DateTime Created { get; set; }
    public int HealthCheck { get; set; }

    public UserRecipe()
    {
        RecipeId = -1;
        UserId = -1;
        RecipeCode = null;
        Created = DateTime.Now;
        HealthCheck = 0;
    }
    public UserRecipe(int recipeId, int playerId, string recipeCode = null)
    {
        RecipeId = recipeId;
        UserId = playerId;
        RecipeCode = recipeCode;
        Created = DateTime.Now;
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