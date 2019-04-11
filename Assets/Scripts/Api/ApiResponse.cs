using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Headers
{
    public string Header1;

    internal string MyInfo()
    {
        return "Header1=" + Header1 ;
    }
}
[Serializable]
public class BodyResponse
{
    public string Message;
    public int RandomNum=0;
    public List<ItemContainer> Items;
    public List<Recipe> Recipes;
    public List<Offer> Offers;

    public List<Character> Characters;
    public List<Research> Researches;

    public List<Region> Regions;
    public List<TerrainIns> Terrains;
    public List<ElementIns> Elements;
    public List<InsideStory> InsideStories;

    public UserPlayer UserPlayer;
    public List<UserItem> UserInventory;
    public List<UserCharacter> UserCharacters;
    public CharacterMixture CharacterMixture;
    public List<UserResearch> CharacterResearches;
    public CharacterResearching CharacterResearching;
    public CharacterSetting CharacterSetting;
    public List<UserRecipe> UserRecipes;

    internal string MyInfo()
    {
        string info = " Message=" +  Message;
        if (RandomNum!=0)
            info += " RandomNum=" + RandomNum;

        if (Items != null)
            info += " Items=" + Items.Count;
        if (Recipes != null)
            info += " Recipes=" + Recipes.Count;
        if (Offers != null)
            info += " Offers=" + Offers.Count;

        if (Characters != null)
            info += " Characters=" + Characters.Count;
        if (Researches != null)
            info += " Researches=" + Researches.Count;

        if (Regions != null)
            info += " Regions=" + Regions.Count;
        if (Terrains != null)
            info += " Terrains=" + Terrains.Count;
        if (Elements != null)
            info += " Elements=" + Elements.Count;
        if (InsideStories != null)
            info += " InsideStories=" + InsideStories.Count;
        
        if (UserPlayer != null)
            info += " UserPlayer=" + UserPlayer.MyInfo();
        if (UserInventory != null)
            info += " UserInventory=" + UserInventory.Count;
        if (UserCharacters != null)
            info += " UserCharacters=" + UserCharacters.Count;
        if (CharacterMixture != null)
            info += " CharacterMixture=" + CharacterMixture.MyInfo();
        if (CharacterResearches != null)
            info += " CharacterResearches=" + CharacterResearches.Count;
        if (CharacterResearching != null)
            info += " CharacterResearching=" + CharacterResearching.MyInfo();
        if (CharacterSetting != null)
            info += " CharacterSetting=" + CharacterSetting.MyInfo();
        if (UserRecipes != null)
            info += " UserRecipes=" + UserRecipes.Count;
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