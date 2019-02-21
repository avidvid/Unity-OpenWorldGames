using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterMixture
{

    public int Id { get; set; }
    public ItemContainer Item { get; set; }
    public DateTime Time { get; set; }


    public CharacterMixture(ItemContainer item, DateTime time)
    {
        Id = 0;
        Item = item;
        Time = time;
    }
    public CharacterMixture()
    {
        Id = -1;
    }

    internal void Print()
    {
        Debug.Log("CharacterMixture = " + Id + "-" + Item.Name + " (" + Time + ")");
    }
}
