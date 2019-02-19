using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class CharacterResearching 
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ResearchId { get; set; }
    public int Level { get; set; }
    public DateTime ResearchTime { get; set; }

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
        var characterManager = CharacterManager.Instance();
        return Id + "-" + characterManager.GetResearchById(ResearchId).Name + " (" + Level + ") in " + ResearchTime;
    }
    internal void Print()
    {
        Debug.Log("CharacterResearching = " + MyInfo());
    }

}
