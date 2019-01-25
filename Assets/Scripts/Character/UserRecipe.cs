using System;
using UnityEngine;

[Serializable]
public class UserRecipe
{
    public int RecipeId { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }


    public UserRecipe()
    {
        RecipeId = -1;
        UserId = -1;
        Created = DateTime.Now;
    }

    public UserRecipe(int recipeId, int playerId)
    {
        RecipeId = recipeId;
        UserId = playerId;
        Created = DateTime.Now;
    }

    public virtual string GetDescription()
    {

        ItemDatabase itemDatabase= ItemDatabase.Instance();
        try
        {
            var recipe = itemDatabase.FindRecipes(RecipeId);
            return "You have " + recipe.Print();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return " Error in User Recipe";
        }
    }
}