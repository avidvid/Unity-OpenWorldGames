using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo Rename to CharacterResearch
[Serializable]
public class CharacterResearch
{
    public int Id;
    public int UserId;
    public int ResearchId;
    public int Level;
    public int HealthCheck;

    public CharacterResearch(int researchId, int userId, int nextLevel)
    {
        Id = 0;
        ResearchId = researchId;
        UserId = userId;
        Level = nextLevel;
        HealthCheck = 0;
    }
    public CharacterResearch()
    {
        Id = -1;
    }
    internal void Print()
    {
        Debug.Log("CharacterResearch = " + Id + "-" + ResearchId + " (" + Level + ")");
    }
    internal void SetEmpty()
    {
        Id = -1;
        Print();
    }
}