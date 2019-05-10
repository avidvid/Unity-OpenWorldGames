using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CharacterResearch
{
    public int Id;
    public int UserId;
    public int ResearchId;
    public int Level;

    public CharacterResearch(int researchId, int userId)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        ResearchId = researchId;
        UserId = userId;
        Level = 1;
    }
    public CharacterResearch()
    {
        Id = 0;
    }
    public void SetLevel(int level)
    {
        Level = level;
    }
    internal string MyInfo()
    {
        if (ResearchId != 0)
        {
            var characterDatabase = CharacterDatabase.Instance();
            return Id + "-" + characterDatabase.GetResearchById(ResearchId).Name + " (" + Level + ") UserId=" + UserId;
        }
        return Id + "-Empty";
    }
    internal void Print()
    {
        Debug.Log("CharacterResearch = " + MyInfo());
    }

}