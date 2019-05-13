using System;
using UnityEngine;

[Serializable]
public class UserRecipe
{
    public int Id;
    public int RecipeId;
    public int UserId;
    public string RecipeCode;

    public UserRecipe(int recipeId)
    {
        RecipeId = 0;
        UserId = 0;
        RecipeCode = "";
    }
    public UserRecipe(int recipeId, int userId, string recipeCode = null)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        RecipeId = recipeId;
        UserId = userId;
        if (recipeCode != null)
            RecipeCode = recipeCode;
    }

    internal void Print()
    {
        Debug.Log("UserRecipe: " + MyInfo());
    }

    internal string MyInfo()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            var recipe = itemDatabase.FindRecipe(RecipeId);
            return Id + "-" + recipe.MyInfo() + " RecipeCode= " + RecipeCode;
        }
        catch (Exception e)
        {
            return "Empty";
        }
    }

}