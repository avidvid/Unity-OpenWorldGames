using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ElementIns {

    public enum ElementType
    {
        Hole,
        Building,
        Tree,
        Bush,
        Rock
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string MultipleName { get; set; }
    public int MultipleId { get; set; }
    public bool Walkable { get; set; }
    public bool Flyable { get; set; }
    public bool Swimable { get; set; }
    public bool Enterable { get; set; }
    public bool Destroyable { get; set; }
    public string DropItems { get; set; }
    public float DropChance { get; set; }
    public ElementType Type { get; set; }
    public TerrainIns.TerrainType FavoriteTerrainTypes { get; set; }
    public bool IsEnable { get; set; }

    private Sprite _tile ;
    public Sprite GetSprite()
    {
        if (_tile == null)
            BuildSprite();
        return _tile;
    }

    public void BuildSprite()
    {
        if (MultipleId!=-1)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Elements/" + MultipleName);
            // Get specific sprite
            _tile = sprites[MultipleId];

        }
        else
            _tile = Resources.Load<Sprite>("Elements/"+ Name);
    }


    internal void Print()
    {
        Debug.Log(
            " Element:" + Id + 
            " Name:" + Name + 
            " Type:" + Type + 
            " Destroyable:" + Destroyable +
            " Enterable :" + Enterable +
            " IsEnable:" + IsEnable + 
            " FavoriteTerrainTypes:" + FavoriteTerrainTypes
            );
    }

}
