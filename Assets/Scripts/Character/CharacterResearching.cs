using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class CharacterResearching 
{
    public int Id;
    public int UserId;
    public int ResearchId;
    public int Level;
    public string ResearchTime;

    public CharacterResearching()
    {
        Id = 0;
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

    internal void SetEmpty()
    {
        Id = 0;
        UserId = 0;
        ResearchId = 0;
        Level = 0;
        ResearchTime = "";
    }

    internal void SetValues(int userId, int researchId, int level, DateTime time)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        UserId = userId;
        ResearchId = researchId;
        Level = level;
        ResearchTime = time.ToString(CultureInfo.InvariantCulture);
    }
}