using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Attack : MonoBehaviour {     
    private CharacterManager _characterManager;
    private GUIManager _GUIManager;
    private BuildingInterior _building;

    void Start ()     {
        _characterManager = CharacterManager.Instance();
        _GUIManager = GUIManager.Instance();
        var insideBuilding = GameObject.Find("Building Interior");
        if (insideBuilding != null)
            _building = insideBuilding.GetComponent<BuildingInterior>();     }     internal void AttackDealing(ActiveMonsterType monster, float dealAtt, string environmentType)
    {            
        //Make monster Aware of player when being hit
        if (environmentType == "Inside")
            monster.SawTarget = true;
        else //Terrain & Combat
            monster.AttackMode = true;
        if (dealAtt <= 0)
        {
            _GUIManager.PrintMessage("No damage dealt!!!", Color.yellow);
            print("No damage dealt!!! Env:"+environmentType + " " + monster.SawTarget+
                    " Att (" +
                  _characterManager.CharacterSetting.AbilityAttack + ","
                  + _characterManager.CharacterSetting.MagicAttack + ","
                  + _characterManager.CharacterSetting.PoisonAttack + ") Def ("
                  + monster.MonsterType.AbilityDefense + ","
                  + monster.MonsterType.MagicDefense + ","
                  + monster.MonsterType.PoisonDefense + ")");
            return;
        }
        monster.MonsterType.Health -= (int)dealAtt * 7; //Todo:Debug  remove this *#
        if (monster.MonsterType.Health <= 0)
        {
            _characterManager.AddCharacterSetting("Experience", monster.MonsterType.MaxHealth);
            monster.Alive = false;
            monster.gameObject.SetActive(false);
            var monsterCharacter = monster.MonsterType.GetCharacter();
            if (environmentType == "Combat")
            {
                var images = GameObject.Find("Capture").GetComponentsInChildren<Image>();
                if (images[1].enabled)
                    if (_characterManager.CaptureMonsterById(monsterCharacter.Id))
                        _GUIManager.PrintMessage("The monster Got captured", Color.green);
                //SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
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
        }
    } }