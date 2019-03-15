using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo Rename to CharacterResearch
[Serializable]
public class UserResearch
{
    public int Id { get; set; }
    public int UserPlayerId { get; set; }
    public int ResearchId { get; set; }
    public int Level { get; set; }
    public int HealthCheck { get; set; }

    public UserResearch(int researchId, int userPlayerId, int nextLevel)
    {
        Id = 0;
        ResearchId = researchId;
        UserPlayerId = userPlayerId;
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