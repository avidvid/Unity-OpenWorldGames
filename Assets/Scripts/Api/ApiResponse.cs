using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Headers
{
    public string Header;
    public int ResultCount = 0;
    public int ScannedCount=0;
    public int HealthCheck;

    internal string MyInfo()
    {
        return "Header=" + Header + " HealthCheck=" + HealthCheck + " Count=" + ResultCount + " ScannedCount=" + ScannedCount ;
    }
}
[Serializable]
public class BodyResponse
{
    public string Message;
    public int UserId = 0;
    public int HealthCheck;
    public List<ItemContainer> Items;
    public List<Recipe> Recipes;
    public List<Offer> Offers;

    public List<Character> Characters;
    public List<Research> Researches;

    public List<Region>  Regions;
    public List<TerrainIns> Terrains;
    public List<ElementIns> Elements;
    public List<InsideStory> InsideStories;

    public UserPlayer UserPlayer;
    public CharacterSetting CharacterSetting;
    public List<UserItem> UserInventory;
    public List<UserCharacter> UserCharacters;
    public CharacterMixture CharacterMixture;
    public List<CharacterResearch> CharacterResearches;
    public CharacterResearching CharacterResearching;
    public List<UserRecipe> UserRecipes;
    public UserRecipe UserRecipe;
    public UserCharacter UserCharacter;

    internal string MyInfo()
    {
        string info = " Message=" +  Message;
        if (UserId!=0) info += " UserId=" + UserId;

        if (Items.Count!=0) info += " Items=" + Items.Count;
        if (Recipes.Count != 0) info += " Recipes=" + Recipes.Count;
        if (Offers.Count != 0) info += " Offers=" + Offers.Count;

        if (Characters.Count != 0) info += " Characters=" + Characters.Count;
        if (Researches.Count != 0) info += " Researches=" + Researches.Count;

        if (Regions.Count != 0) info += " Terrains=" + Regions.Count;
        if (Terrains.Count != 0) info += " Terrains=" + Terrains.Count;
        if (Elements.Count != 0) info += " Elements=" + Elements.Count;
        if (InsideStories.Count != 0) info += " InsideStories=" + InsideStories.Count;
        
        if (UserPlayer!=null) info += " UserPlayer=" + UserPlayer.MyInfo();
        if (UserInventory.Count != 0) info += " UserInventory=" + UserInventory.Count;
        if (UserCharacters.Count != 0) info += " UserCharacters=" + UserCharacters.Count;
        if (CharacterMixture.Id != 0) info += " CharacterMixture=" + CharacterMixture.MyInfo();
        if (CharacterResearches.Count != 0) info += " CharacterResearches=" + CharacterResearches.Count;
        if (CharacterResearching.Id != 0) info += " CharacterResearching=" + CharacterResearching.MyInfo();
        if (CharacterSetting.Id != 0) info += " CharacterSetting=" + CharacterSetting.MyInfo();
        if (UserRecipes.Count != 0) info += " UserRecipes=" + UserRecipes.Count;
        return info;
    }
}
[Serializable] 
public class ApiResponse 
{
    public string StatusCode;
    public Headers Headers;
    public BodyResponse Body;
    internal void Print()
    {
        Debug.Log("ApiResponse(StatusCode=" + StatusCode + "): Headers: " + Headers.MyInfo() +
                  " Body: " + Body.MyInfo());
    }
}
//Items
//Recipes
//Offers

//Characters
//Researches

//Regions
//Terrains
//Elements
//InsideStories

//UserPlayer
//CharacterSetting
//CharacterMixture
//CharacterResearching
//UserInventory
//UserCharacters
//CharacterResearches
//UserRecipes