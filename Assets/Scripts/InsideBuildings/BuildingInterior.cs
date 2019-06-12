using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingInterior : MonoBehaviour {

    private ItemDatabase _itemDatabase;
    private TerrainDatabase _terrainDatabase;
    private AudioManager _audioManager;
    private CharacterManager _characterManager;

    //todo: make them private
    public Sprite[] FloorTiles;
    public Sprite Wall;
    public Sprite DoorShadow;
    public Transform Player;


    private int _key = 0;
    private Vector2 _mapPosition = Vector2.zero;
    private Vector3 _previousPosition = Vector3.zero;

    private int _maxWidth;
    private int _maxHeight;
    private Sprite _floor;
    private int _randomIndex = 0;
    private int _baseSortIndex = 0;

    private List<Rect> _walls = new List<Rect>();
    private Rect _exit;

    private InsideStory _story;
    private List<ActiveMonsterType> _monsters = new List<ActiveMonsterType>();
    private List<ActiveItemType> _items = new List<ActiveItemType>();
    List<Vector3> _floors = new List<Vector3>();

    private GameObject _monsterObj;

    void Start()
    {
        _audioManager = AudioManager.Instance();
        _terrainDatabase = TerrainDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        _itemDatabase = ItemDatabase.Instance();
        _key = _terrainDatabase.GetRegionKey();
        _monsterObj = Resources.Load<GameObject>("Prefabs/Monster");
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
        {
            _mapPosition = starter.MapPosition;
            _previousPosition = starter.PreviousPosition;
        }
        _floor = FloorTiles[RandomHelper.Range(new Vector2(_mapPosition.x + 0.01f, _mapPosition.y + 0.01f), _key, FloorTiles.Length)];
        //print("Building Interior _floor: "  +"  " + _floor.name);
        _story  = _terrainDatabase.GetStoryBasedOnRarity(_mapPosition, _key);
        GenerateInterior();
        if (_audioManager != null)
        {
            _audioManager.UpdateSoundVolume(_characterManager.UserPlayer.SoundVolume);
            _audioManager.PlayInsideMusic(_previousPosition, _key);
        }
    }
    private void GenerateInterior()
    {
        List<Vector3> applied = new List<Vector3>();
        List<Vector3> walls = new List<Vector3>();

        //Identify the lowest place to implant the door 
        Vector3 lowestFloor = new Vector3(0, int.MaxValue, 0); 
        //Whole area of the inside buildings 
        _maxWidth = _maxHeight = Range(12) + 8; //Range 8-20
        int roomCount = Range(7) + 2; //2-9

        var prevRoom = RandomRoom();

        //Generate n# Rooms (applied) and potential (walls)
        for (int roomIndex = 0; roomIndex < roomCount; roomIndex++)
        {
            var newRoom = RandomRoom();
            if (!prevRoom.Overlaps(newRoom))
            {
                roomCount++;
                continue;
            }
            for (int x = 0; x < newRoom.width; x++)
            {
                for (int y = 0; y < newRoom.height; y++)
                {
                    var tilePos = new Vector3(newRoom.x + x, newRoom.y + y, 0);
                    if (applied.Contains(tilePos))
                        continue;
                    applied.Add(tilePos);
                    _floors.Add(tilePos);
                    if (tilePos.y < lowestFloor.y)
                        lowestFloor = tilePos;
                    //Add potential Walls in all 8 surrounding  places 
                    walls.AddRange(new Vector3[] {
                        tilePos + Vector3.up,
                        tilePos + Vector3.down,
                        tilePos + Vector3.left,
                        tilePos + Vector3.right,
                        tilePos + Vector3.up + Vector3.left,
                        tilePos + Vector3.down + Vector3.left,
                        tilePos + Vector3.up + Vector3.right,
                        tilePos + Vector3.down + Vector3.right
                        });
                    var tile = new GameObject();
                    tile.transform.position = tilePos;
                    var tileRenderer = tile.AddComponent<SpriteRenderer>();
                    tileRenderer.sprite = _floor;
                    tile.transform.parent = transform;
                    tile.name = "Floor " + tile.transform.position;
                }
            }
            prevRoom = newRoom;
        }

        //Purge the (walls)
        List<Vector3> sortedWalls = walls.OrderByDescending(v => v.y).ToList();
        float wallOrder = -0.001f;
        foreach (var wallPos in sortedWalls)
        {
            //if it is already tiled then it should not be a wall
            if (applied.Contains(wallPos))
                continue;
            //add wall to applied so it get floor sprite 
            applied.Add(wallPos);
            var tile = new GameObject();
            //Helps the 3D look of the wall look better 
            tile.transform.position = new Vector3(wallPos.x, wallPos.y, wallPos.z + wallOrder);
            wallOrder -= 0.001f;
            var wallRenderer = tile.AddComponent<SpriteRenderer>();
            wallRenderer.sprite = Wall;
            tile.transform.parent = transform;
            tile.name = "Wall " + tile.transform.position;
            //Add the exit 
            if (wallPos + Vector3.up == lowestFloor)
            {
                _floors.Remove(lowestFloor);
                tile.name = "DoorFloor " + tile.transform.position;
                wallRenderer.sprite = _floor;
                _exit = new Rect(wallPos, Vector3.one);
                //Add the doorway shadow object
                var shadow = new GameObject();
                shadow.transform.position = new Vector3(wallPos.x, wallPos.y, wallPos.z + wallOrder);
                wallOrder -= 0.001f;
                var doorRenderer = shadow.AddComponent<SpriteRenderer>();
                doorRenderer.sprite = DoorShadow;
                shadow.transform.parent = transform;
                shadow.name = "DoorShadow " + shadow.transform.position;
            }
            else
                //Add to be blocked
                _walls.Add(new Rect(wallPos, Vector2.one));
        }
        lowestFloor.z = Player.position.z;
        Player.position = lowestFloor;

        int actorCount = 1;
        for (int actorIndex = 1; actorIndex <= actorCount; actorIndex++)
        {
            foreach (var storyActor in _story.Actors)
            {
                if (storyActor.CharacterCnt < actorIndex)
                    continue;
                actorCount++;
                var actorLocation = _floors[Random.Range(0, _floors.Count)];
                _floors.Remove(actorLocation);
                CreateMonsters(actorLocation, storyActor);
            }
        }
    }
    private Rect RandomRoom()
    {
        return new Rect(Range(_maxWidth / 2),           //X
            Range(_maxHeight / 2),                      //Y
            Range(_maxWidth / 4) + _maxWidth / 4,       //Width
            Range(_maxHeight / 4) + _maxHeight / 4);    //height
    }
    public bool IsExiting(Rect area)
    {
        return _exit.Overlaps(area);
    }
    public bool IsBlocked(Rect area)
    {
        foreach (var wall in _walls)
            if (wall.Overlaps(area))
                return true;
        return false;
    }
    private int Range(int max)
    {
        return RandomHelper.Range(_mapPosition, _key + _randomIndex++, max);
    }
    //Monster
    private void CreateMonsters(Vector3 location, Actor storyActor)
    {
        GameObject monster = Instantiate(_monsterObj);
        Character monsterCharacter = _terrainDatabase.GetInsMonsterById(storyActor.CharacterId);
        monster.transform.position = location;
        monster.transform.SetParent(transform);
        monster.name = "Monster " + monster.transform.position;
        var active = monster.GetComponent<ActiveMonsterType>();
        active.Location = location;
        active.Alive = true;
        active.Hidden = false;
        var spriteRenderer = monster.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = monsterCharacter.GetSprite();
        spriteRenderer.sortingOrder = _baseSortIndex + 7;
        var textLevelRenderer = monster.GetComponentInChildren<TextMeshPro>();
        textLevelRenderer.text = "Level " + _characterManager.CharacterSetting.Level;
        monster.GetComponentInChildren<MeshRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        if (monsterCharacter.IsAnimated)
        {
            var animator = monster.GetComponent<Animator>();
            animator.runtimeAnimatorController = monsterCharacter.GetAnimator();
            animator.speed = monsterCharacter.Move == Character.CharacterType.Fly ? 1 : 0;
        }
        active.MonsterType = new MonsterIns(monsterCharacter, storyActor.MinLevel+ _characterManager.CharacterSetting.Level);
        _monsters.Add(active); 
    }
    public IEnumerable<ActiveMonsterType> GetMonster(Vector3 pos, float radius)
    {
        pos.z = 0;
        foreach (var monster in _monsters)
        {
            if (monster.Alive)
            {
                var bLoc = monster.transform.position;
                //MapPos in between the monster 
                var viewLimit = monster.MonsterType.View;
                if (Math.Abs(radius) > 0)
                    viewLimit = radius;
                if (Vector2.Distance(pos, bLoc) < viewLimit)
                    yield return monster;
            }
        }
    }
    //Item
    internal void DestroyItem(ActiveItemType item)
    {
        _items.Remove(item);
        Destroy(item.gameObject);
    }
    //Logic should match with TerrainDropItem
    public void BuildingDropItem(Vector3 pos, float chance, string dropItems)
    {
        int itemId;
        if (RandomHelper.GetLucky(pos, chance))
            itemId = _itemDatabase.GetItemIdBasedOnRarity(pos, dropItems);
        else //Drop coin/Gem/Recipe
            itemId = _itemDatabase.GetItemIdBasedOnRarity(pos);
        if (itemId != -1)
            CreateItem(pos, itemId);
    }
    public void CreateItem(Vector3 location, int itemId)
    {
        GameObject item = new GameObject();
        var active = item.AddComponent<ActiveItemType>();
        active.ItemTypeInUse = _itemDatabase.GetItemById(itemId);
        location.z -= 0.001f;
        active.Location = location;
        item.transform.position = location;
        item.transform.localScale += new Vector3(0.1f, 0, 0);
        var renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = active.ItemTypeInUse.GetSprite();
        renderer.sortingOrder = _baseSortIndex + 2;
        item.name = active.ItemTypeInUse.Name + item.transform.position;
        item.transform.parent = transform;
        _items.Add(active);
    }
    public IEnumerable<ActiveItemType> GetItem(Vector3 pos)
    {
        pos.z = 0;
        foreach (var item in _items)
        {
            var bLoc = item.transform.position;
            //MapPos in between the monster 
            if (Vector2.Distance(pos, bLoc) <0.5)
                yield return item;
        }
    }
}