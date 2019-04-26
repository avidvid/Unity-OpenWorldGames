using System;
using UnityEngine;

[Serializable]
public class CharacterResearching 
{
    public int Id;
    public int UserId;
    public int ResearchId;
    public int Level;
    public DateTime ResearchTime;

    public CharacterResearching(int userId, int researchId, int level, DateTime time)
    {
        Id = 0;
        UserId = userId;
        ResearchId = researchId;
        Level = level;
        ResearchTime = time;
    }
    public CharacterResearching()
    {
    }
    internal string MyInfo()
    {
        var characterDatabase = CharacterDatabase.Instance();
        return Id + "-" + characterDatabase.GetResearchById(ResearchId).Name + " (" + Level + ") in " + ResearchTime;
    }
    internal void Print()
    {
        Debug.Log("CharacterResearching = " + MyInfo());
    }
}