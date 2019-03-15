using System;
using UnityEngine;

[Serializable]
public class ResearchingDelete
{
    public int Id { get; set; }
    public int UserPlayerId { get; set; }
    public int ResearchId { get; set; }
    public DateTime Time { get; set; }


    internal void Print()
    {
        Debug.Log("CharacterMixture = " + Id + "-" + ResearchId + " (" + Time + ")");
    }
}
