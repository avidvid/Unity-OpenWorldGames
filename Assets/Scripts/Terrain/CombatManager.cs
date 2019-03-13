using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    private TerrainDatabase _terrainDatabase;
    private int _horizontalTiles = 11;
    private int _verticalTiles = 7;
    private int _key;
    private string _monsterInfo;

    //private Vector3 _previousPosition = Vector3.zero;
    private Vector2 _mapPosition = Vector2.zero;
    private GameObject _mapPanel;

    void Start()
    {
        _terrainDatabase = TerrainDatabase.Instance();
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
        {
            _mapPosition = starter.MapPosition;
            _key = starter.Key; 
            _monsterInfo = starter.Content;
            starter.LastScene = "Combat";
            //_previousPosition = starter.PreviousPosition;
        }
        SetMonster(_monsterInfo);
        DrawMap();
    }
    public TerrainIns GetTerrain(float x, float y, int key)
    {
        var availableTerrainTypes = _terrainDatabase.GetTerrains();
        x = (int)x >> 4;
        y = (int)y >> 4;
        return availableTerrainTypes[RandomHelper.Range(x , y , key, availableTerrainTypes.Count)];
    }     void DrawMap()
    {
        var terrain = GetTerrain(_mapPosition.x, _mapPosition.y, _key);
        Debug.Log("CombatManager-Terrain:" + terrain.MyInfo());
        var renderers = new SpriteRenderer[_horizontalTiles, _verticalTiles];
        var offset = new Vector3(0 - _horizontalTiles / 2, 0 - _verticalTiles / 2, 0);
        for (int x = 0; x < _horizontalTiles; x++)
        {
            for (int y = 0; y < _verticalTiles; y++)
            {
                var tile = new GameObject();
                tile.transform.position = new Vector3(x, y, 0) + offset;
                var spriteRenderer = renderers[x, y] = tile.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 0;
                tile.name = "Terrain " + tile.transform.position;
                tile.transform.parent = transform;
                if (terrain.Tiles > 0)
                    spriteRenderer.sprite = terrain.GetTile(x,y,_key);
                var animator = spriteRenderer.gameObject.GetComponent<Animator>();
                if (terrain.AnimationController > 0)
                {
                    if (animator == null)
                    {
                        animator = spriteRenderer.gameObject.AddComponent<Animator>();
                        animator.runtimeAnimatorController = terrain.GetAnimation(x, y, _key);
                    }
                }
                else
                    if (animator != null)
                        GameObject.Destroy(animator);
            }
        }
    }

    private void SetMonster(string monsterInfo)
    {
        List<int> monsterData = monsterInfo.Split(',').Select(Int32.Parse).ToList();
        //monsterData[0]=CharacterId
        //monsterData[1]=Level
        Character monsterCharacter = _terrainDatabase.GetMonsterById(monsterData[0]);
        var monster = GameObject.FindGameObjectWithTag("Monster");
        monster.name = "Monster " + monsterCharacter.Name + "("+ monsterData[1] + ")"+ monster.transform.position;
        var active = monster.GetComponent<ActiveMonsterType>();
        active.Location = transform.position;
        active.Alive = true;
        active.Hidden = false;
        active.SawTarget = true;
        active.AttackMode = false; 
        active.MonsterType = new MonsterIns(monsterCharacter, monsterData[1]);
        var textLevelRenderer = monster.GetComponentInChildren<TextMeshPro>();
        textLevelRenderer.text = "Level " + active.MonsterType.Level;
    }
    public void BackToMainScene()
    {
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
            starter.Content = "Exit from Fight";
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
}