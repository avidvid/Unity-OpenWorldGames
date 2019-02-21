using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterResearch
{
    private int nextLevel;
    private DateTime time;


    public int Id { get; set; }
    public int UserPlayerId { get; set; }
    public int ResearchId { get; set; }
    public int Level { get; set; }

    public CharacterResearch(int researchId, int userPlayerId, int nextLevel)
    {
        Id = 0;
        ResearchId = researchId;
        UserPlayerId = userPlayerId;
        Level = nextLevel;
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
