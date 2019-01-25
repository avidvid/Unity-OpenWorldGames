using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class CharacterResearching 
{
    public int Id { get; set; }
    public CharacterResearch CharResearch { get; set; }
    public DateTime Time { get; set; }

    public CharacterResearching(CharacterResearch charResearch,  DateTime time)
    {
        Id = 0;
        CharResearch = charResearch;
        Time = time;
    }
    public CharacterResearching()
    {
        Id = -1;
    }

    internal void Print()
    {
        if (Id == -1)
            Debug.Log("CharacterResearching = " + Id + "-Empty "+ Time);
        else
            Debug.Log("CharacterResearching = " + Id + "-" + CharResearch.ResearchId + " (Level: " + CharResearch.Level + ")"+ Time);
    }
}
