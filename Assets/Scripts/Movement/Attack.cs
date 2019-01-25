using System;
using System.Collections; using System.Collections.Generic; using UnityEngine;  public class Attack : MonoBehaviour {          private TerrainManager _terrainManager;
    private CharacterManager _characterManager;
    private Cache _cache;
    private GUIManager _GUIManager;
    private InventoryHandler _inv;
    [SerializeField]
    private BuildingInterior _building;


    private bool _monsterWarned;

    //private float nextActionTime = 0.0f;

    // Use this for initialization
    void Start ()     {
        if (_building == null)
        {
            _cache = Cache.Instance();
            _terrainManager = TerrainManager.Instance();
        }
        _GUIManager = GUIManager.Instance();
        _inv = InventoryHandler.Instance();
        _characterManager = CharacterManager.Instance();     } 	     internal void AttackDealing(ActiveMonsterType monster, float dealAtt, string environmentType)
    {
        if (dealAtt <= 0)
        {
            _GUIManager.PrintMessage("No damage dealt!!!", Color.yellow);
            print("No damage dealt!!! Att (" +
                  _characterManager.CharacterSetting.AbilityAttack + ","
                  + _characterManager.CharacterSetting.MagicAttack + ","
                  + _characterManager.CharacterSetting.PoisonAttack + ") Def ("
                  + monster.MonsterType.AbilityDefense + ","
                  + monster.MonsterType.MagicDefense + ","
                  + monster.MonsterType.PoisonDefense + ")");
            return;
        }

        monster.MonsterType.Health -= (int)dealAtt * 50; //Todo:Debug  remove this 50
        if (monster.MonsterType.Health <= 0)
        {
            _characterManager.AddCharacterSetting("Experience", monster.MonsterType.MaxHealth);
            monster.Alive = false;
            monster.gameObject.SetActive(false);
            var monsterCharacter = monster.MonsterType.GetCharacter();
            if (environmentType == "Terrain")
            {
                var deadPos = monster.Location;
                _cache.PersistAdd(new CacheContent()
                {
                    Location = deadPos,
                    ObjectType = "DeadMonster",
                    ExpirationTime = DateTime.Now.AddHours(-1)  //todo: make it day 
                }
                );
                _terrainManager.DeadMonster(monster.transform.position, monsterCharacter);
            }
            else
                _building.BuildingDropItem(monster.transform.position, monsterCharacter.DropChance, monsterCharacter.DropItems);
        }
        else
        {
            var healthBar = monster.transform.GetComponentsInChildren<Transform>()[1];
            healthBar.localScale =
                new Vector3((float)monster.MonsterType.Health / monster.MonsterType.MaxHealth / 3,
                    healthBar.localScale.y, healthBar.localScale.z);

            //Make monster Aware of player when being hit
            if (environmentType == "Inside")
                monster.SawTarget = true;
            else
                monster.AttackMode = true;

        }
    }  } 