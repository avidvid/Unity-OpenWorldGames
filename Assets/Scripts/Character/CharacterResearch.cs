using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo Rename to CharacterResearch
[Serializable]
public class UserResearch
{
    public int Id;
    public int UserId;
    public int ResearchId;
    public int Level;
    public int HealthCheck;

    public UserResearch(int researchId, int userId, int nextLevel)
    {
        Id = 0;
        ResearchId = researchId;
        UserId = userId;
        Level = nextLevel;
        HealthCheck = 0;
    }
    public UserResearch()
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