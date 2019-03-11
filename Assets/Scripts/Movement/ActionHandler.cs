using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionHandler : MonoBehaviour
{
    private TerrainManager _terrainManager;
    private CharacterManager _characterManager;
    private Cache _cache;
    private InventoryHandler _inv;
    private BuildingInterior _building;

    private GameObject _popupAction;
    private Vector3 _location;
    private ActiveMonsterType _monster;
    private Transform _player;
    private ActiveItemType _item;
    private ActiveElementType _element;
    private TerrainIns _terrain;

    [SerializeField]
    private bool _isInside;

    public Button ActionButton;
    public Button WalkButton;

    private Sprite[] _toolSpriteList;
    public Sprite Walk;
    public Sprite Attack;
    public Sprite Grab;
    public Sprite Enter;
    public Sprite Question;

    void Start()
    {
        //Inside Building doesn't need these 
        if (_isInside)
            _building = GameObject.Find("Building Interior").GetComponent<BuildingInterior>();
        else
        {
            _cache = Cache.Instance();
            _terrainManager = TerrainManager.Instance();
        }
        _inv = InventoryHandler.Instance();
        _characterManager = CharacterManager.Instance();
        _popupAction = GameObject.Find("Popup Action");
        _toolSpriteList = Resources.LoadAll<Sprite>("Inventory/InventoryTools");
    }
    public void SetAction(Vector3 location, string action)
    {
        _location = location;
        switch (action)
        {
            case "Element":
                ActionButton.onClick.RemoveAllListeners();
                ActionButton.onClick.AddListener(CloseMe);
                ActionButton.onClick.AddListener(SetStop);
                if (_element != null)
                {
                    if (_element.ElementTypeInUse.Destroyable)
                    {
                        ActionButton.onClick.AddListener(ConsumeElement);
                        ActionButton.GetComponentsInChildren<Image>()[1].sprite = GetToolSprite(_element.ElementTypeInUse.Type);
                    }
                    if (_element.ElementTypeInUse.Enterable)
                        WalkButton.GetComponentsInChildren<Image>()[1].sprite = Enter;
                }
                break;
            case "Monster":
                ActionButton.onClick.RemoveAllListeners();
                ActionButton.onClick.AddListener(CloseMe);
                ActionButton.onClick.AddListener(SetStop);
                WalkButton.GetComponentsInChildren<Image>()[1].sprite = Attack;
                break;
            case "Item":
                ActionButton.onClick.RemoveAllListeners();
                ActionButton.onClick.AddListener(CloseMe);
                ActionButton.onClick.AddListener(SetStop);
                ActionButton.onClick.AddListener(GrabItem);
                ActionButton.GetComponentsInChildren<Image>()[1].sprite = Grab;
                break;
            case "Dig":
                ActionButton.onClick.RemoveAllListeners();
                ActionButton.onClick.AddListener(CloseMe);
                ActionButton.onClick.AddListener(SetStop);
                ActionButton.onClick.AddListener(TerrainDig);
                ActionButton.GetComponentsInChildren<Image>()[1].sprite = GetToolSprite(ElementIns.ElementType.Hole);
                break;
            default:
                ActionButton.onClick.RemoveAllListeners();
                ActionButton.onClick.AddListener(CloseMe);
                ActionButton.onClick.AddListener(SetStop);
                ActionButton.onClick.AddListener(ShowMapInfo);
                WalkButton.GetComponentsInChildren<Image>()[1].sprite = Walk;
                ActionButton.GetComponentsInChildren<Image>()[1].sprite = Question;
                break;
        }
    }
    public void CloseMe()
    {
        _popupAction.SetActive(false);
    }
    private Sprite GetToolSprite(ElementIns.ElementType type)
    {
        switch (type)
        {
            case ElementIns.ElementType.Hole:
                return _toolSpriteList[16];
            case ElementIns.ElementType.Building:
                return _toolSpriteList[10];
            case ElementIns.ElementType.Bush:
                return _toolSpriteList[8];
            case ElementIns.ElementType.Rock:
                return _toolSpriteList[0];
            case ElementIns.ElementType.Tree:
                print("Tree");
                return _toolSpriteList[2];
            default:
                return null;
        }
    }
    //Item
    internal void SetActiveItem(ActiveItemType currentItem, string environmentType)
    {
        _item = currentItem;
        _isInside = (environmentType == "Inside");
    }
    public void GrabItem()
    {
        if (_inv.AddItemToInventory(_item.ItemTypeInUse))
        {
            _inv.UpdateInventory(true);
            if (_isInside)
                _building.DestroyItem(_item);
            else
                _terrainManager.DestroyItem(_item);
        }
    }
    //Element
    internal void SetActiveElement(ActiveElementType currentElement)
    {
        _element = currentElement;
    }
    public void ConsumeElement()
    {
        if (_terrainManager.DestroyElement(_element, true))
        {
            Vector3 elementPos = _element.transform.position;
            //Remember Consume element to not draw them 
            _cache.PersistAdd(new CacheContent()
                {
                    Location = elementPos,
                    ObjectType = "VacantElement",
                    ExpirationTime = DateTime.Now.AddDays(2)
                }
            );
            _terrainManager.TerrainDropItem(elementPos, _element.ElementTypeInUse.DropChance, _element.ElementTypeInUse.DropItems);
        }
    }
    //Terrain
    internal void SetActiveTerrain(TerrainIns currentTerrain)
    {
        _terrain = currentTerrain;
    }
    public void TerrainDig()
    {
        if (_terrainManager.CreateDigging(_location, true))
        {
            _cache.Add(new CacheContent()
                {
                    Location = _location,
                    ObjectType = "Digging"
                }
            );
            _terrainManager.TerrainDropItem(_location, _terrain.DropChance, _terrain.DropItems);
        }
    }
    //Moving
    public void WalkToLocation()
    {
        var moveManager = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<MoveManager>();
        moveManager.SetMovement(_location);
    }
    internal void SetStop()
    {
        var moveManager = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<MoveManager>();
        moveManager.SetStop(false);
    }
    public void ShowMapInfo()
    {
         _inv.PrintMessage("Austin", Color.black); //todo:fix this 
    }
    //Monster
    public void SetActiveMonster(ActiveMonsterType monster,Transform player, string environmentType)
    {
        _monster = monster;
        _player = player;
        _isInside = (environmentType == "Inside");
    }
    public void AttackMonster()
    {
        //1-A Find Direction
        var direction = (_monster.transform.position - _player.position).normalized;
        //2-A calculate Att
        var attAmount = RandomHelper.AbsZero(_characterManager.CharacterSetting.AbilityAttack  - _monster.MonsterType.AbilityDefense)
                        + RandomHelper.AbsZero(_characterManager.CharacterSetting.MagicAttack  - _monster.MonsterType.MagicDefense)
                        + RandomHelper.AbsZero(_characterManager.CharacterSetting.PoisonAttack - _monster.MonsterType.PoisonDefense);
        //3-A calculate dealAtt
        var dealAtt = RandomHelper.CriticalRange(attAmount);
        print("Player " + _characterManager.CharacterSetting.GetInfo("Attack") +
              "-> Monster Lv" + _monster.MonsterType.Level +  _monster.MonsterType.GetInfo("Defense") +
              " MonsterHealth (" + _monster.MonsterType.Health + ")" +
              " = " + dealAtt + "/" + attAmount
              + " Critical =" + (dealAtt > attAmount));
        //4-A Use Energy
        if (!_characterManager.UseEnergy((int)attAmount))
        {
            _inv.PrintMessage("Not enough energy to attack", Color.yellow);
            return;
        }
        //5-A Cast Spell (Lunch spell-Drop dealAtt hit- AttackDealing)
        CastSpell(_player.position, _monster, dealAtt);
        //6- Show Ray cast
        Debug.DrawRay(_player.position, direction, Color.blue);
    }
    // CastSpell Logic
    private void CastSpell(Vector3 source, ActiveMonsterType monster, float attackDealt)
    {
        var spell = new GameObject();
        spell.transform.position = new Vector3(source.x, source.y, -1);
        spell.transform.parent = _player.parent.parent;
        spell.name = "Player Spell";
        var spellRenderer = spell.AddComponent<SpriteRenderer>();
        spellRenderer.sortingOrder = _player.GetComponent<SpriteRenderer>().sortingOrder + 1;
        spellRenderer.sprite = _characterManager.GetSpellSprite(); 
        spell.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        var spellManager = spell.AddComponent<SpellManager>();
        spellManager.Target = monster;
        spellManager.AttackValue = attackDealt;
        spellManager.MonsterType = _isInside?"Inside":"Terrain";
    }
}
