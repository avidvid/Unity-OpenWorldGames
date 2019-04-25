﻿using System;
using UnityEngine;

[Serializable]
public class UserRecipe
{
    public int Id;
    public int RecipeId;
    public int UserId;
    public string RecipeCode;
    public DateTime Created;

    public UserRecipe()
    {
        RecipeId = -1;
        UserId = -1;
        RecipeCode = null;
        Created = DateTime.Now;
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