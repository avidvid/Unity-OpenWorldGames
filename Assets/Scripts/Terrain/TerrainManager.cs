﻿using System; using System.Collections.Generic; using TMPro; using UnityEngine; using UnityEngine.SceneManagement;  public class TerrainManager : MonoBehaviour {      public Sprite Dig;     private static TerrainManager _terrainManager;     private InventoryHandler _inv;     private ItemDatabase _itemDatabase;     private CharacterManager _characterManager;     private TerrainDatabase _terrainDatabase;     private Cache _cache;     private AudioManager _audioManager;      private Transform _player;      private float _maxDistanceFromCenter = 7;     private Vector2 _mapOffset = new Vector2(0.5f, 0.5f);     public Region Region;     public int Key;      private int _baseSortIndex = 0;      private List<TerrainIns> _terrainTypes;      private List<ElementIns> _allElements;
    private List<ElementIns> _elementTypes = new List<ElementIns>();     private List<ActiveElementType> _elements = new List<ActiveElementType>();      private List<Character> _allMonster;     private List<Character> _monsterTypes = new List<Character>();     private List<ActiveMonsterType> _monsters = new List<ActiveMonsterType>();      private List<ActiveItemType> _activeItems = new List<ActiveItemType>();       private int _horizontalTiles = 25;
    private int _verticalTiles = 25;     private SpriteRenderer[,] _renderers;     private IEnumerable<Marker> _markers;     private List<GameObject> _digs = new List<GameObject>();       private GameObject _monsterObj;      void Awake()     {         _terrainManager = TerrainManager.Instance();         _audioManager = AudioManager.Instance();         _itemDatabase = ItemDatabase.Instance();         _inv = InventoryHandler.Instance();         _terrainDatabase = TerrainDatabase.Instance();         _characterManager = CharacterManager.Instance();         _cache = Cache.Instance();         _monsterObj = Resources.Load<GameObject>("Prefabs/Monster");         _player = GameObject.FindGameObjectWithTag("PlayerCamera").transform;     }      void Start()     {         _terrainTypes = _terrainDatabase.GetTerrains();         _allElements = _terrainDatabase.GetElements();         _allMonster = _terrainDatabase.GetMonsters();         Region = _terrainDatabase.GetRegion();
        Key = Region.Key;         _player.position = _characterManager.GetMapLocation(Key);         print("Region= " + Region.MyInfo() + " Player.position = " + _player.position);         var offset = new Vector3(0 - _horizontalTiles / 2, 0 - _verticalTiles / 2, 0);         _renderers = new SpriteRenderer[_horizontalTiles, _verticalTiles];         for (int x = 0; x < _horizontalTiles; x++)         {             for (int y = 0; y < _verticalTiles; y++)             {                 var tile = new GameObject();                 tile.transform.position = new Vector3(x, y, 0) + offset;                 var spriteRenderer = _renderers[x, y] = tile.AddComponent<SpriteRenderer>();                 spriteRenderer.sortingOrder = _baseSortIndex;                 tile.name = "Terrain " + tile.transform.position;                 tile.transform.parent = transform;             }         }         print("Terrain Manager Health Check #1");         var starter = GameObject.FindObjectOfType<SceneStarter>();         if (starter != null)         {             _player.position = starter.PreviousPosition;             _inv.ShowInventory = starter.ShowInventory;         }         //else SetPlayerLocation();         print("Terrain Manager Health Check #2");         RedrawMap(true);         if (starter == null)             SetPlayerLocation();         else         {             if (starter.LastScene == "Combat")                 if (starter.Content != "FightRunAway")                 {                     foreach (var monster in GetMonster(starter.MapPosition, 0.3f))                         if (monster.Alive)                             if (DestroyMonster(monster, true))                                 break;                             else                                 throw new Exception("TM-Wasn't able to kill the monster!!!");                 }             Destroy(starter.gameObject);         }         print("Terrain Manager Health Check #3");         _audioManager.UpdateSoundVolume(_characterManager.UserPlayer.SoundVolume);         _audioManager.PlayBgMusic(_player.position, Key);     }     void Update()     {         if (_maxDistanceFromCenter < Vector3.Distance(_player.position, transform.position))             RedrawMap(false);     }
    internal void DeadMonster(Vector3 pos, Character character)
    {         TerrainDropItem(pos, character.DropChance, character.DropItems);
    }
     public Vector2 WorldToMapPosition(Vector3 worldPosition)     {         if (worldPosition.x < 0) worldPosition.x--;         if (worldPosition.y < 0) worldPosition.y--;                 return new Vector2((int)(worldPosition.x + _mapOffset.x), (int)(worldPosition.y + _mapOffset.y));     }     void RedrawMap(bool isStart)     {         transform.position = new Vector3( (int)_player.position.x, (int)_player.position.y,_player.position.z);         //print("RedrawMap (" + transform.position.x + "," + transform.position.y +"RedrawMap (" + Key);          _markers = Marker.GetMarkers(transform.position.x, transform.position.y, Key, _terrainTypes);         var offset = new Vector3(                 transform.position.x - _horizontalTiles / 2,                  transform.position.y - _verticalTiles / 2,                  0);         for (int x = 0; x < _horizontalTiles; x++)         {             for (int y = 0; y < _verticalTiles; y++)             {                 var spriteRenderer = _renderers[x, y];                 var terrain = SelectTerrain(                         offset.x + x,                         offset.y + y);                 if (terrain.Tiles > 0)                     spriteRenderer.sprite = terrain.GetTile(                         offset.x + x,                         offset.y + y,                         Key);                 var animator = spriteRenderer.gameObject.GetComponent<Animator>();                 if (terrain.AnimationController > 0)                 {                     if(animator == null )                     {                         animator = spriteRenderer.gameObject.AddComponent<Animator>();                         animator.runtimeAnimatorController = terrain.GetAnimation(                                                                             offset.x + x,                                                                             offset.y + y,                                                                             Key);                     }                 }                 else                 {                     if (animator != null)                         GameObject.Destroy(animator);                 }             }         }          //Destruction happen at the end of the frame not immediately          _elements.ForEach(x => Destroy(x.gameObject));         _elements.Clear();         _monsters.ForEach(x => Destroy(x.gameObject));         _monsters.Clear();         _activeItems.ForEach(x => Destroy(x.gameObject));         _activeItems.Clear();         _digs.ForEach(x => Destroy(x.gameObject));         _digs.Clear();         foreach (var marker in _markers)         {             //Start up sets ups             if (marker.HasElement)             {                 SetAvailableMarketElements(marker.Terrain);                 LoadElements(marker);             }             if (marker.HasMonster)             {                 SetAvailableMarketMonsters(marker.Terrain);                 LoadMonsters(marker);             }         }
        //Todo:Performance: We create elements and we delete them here !!! If it has been consumed recently delete them         UnCacheElements();         LoadCaches();         UnCacheMonsters();     } 

    public void OpenMiniMap()     {         Vector3 currentPos = _player.position;         Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);         Debug.Log("OpenMiniMap " + "currentPos =" + currentPos + "mapPos =" + mapPos);         //Preparing to switch the scene         GameObject go = new GameObject();         //Make go unDestroyable         GameObject.DontDestroyOnLoad(go);         var starter = go.AddComponent<SceneStarter>();         starter.Key = Key;         starter.MapPosition = mapPos;         starter.PreviousPosition = currentPos;         go.name = "Scene Starter";         //switch the scene         SceneManager.LoadScene(SceneSettings.SceneIdForMiniMap);     }      private void LoadCaches()     {         foreach (var item in _cache.Find("Digging", transform.position, _horizontalTiles, false))             CreateDigging(item.Location,false);         foreach (var item in _cache.Find("Item", transform.position, _horizontalTiles, false))             CreateItem(item.Location, Int32.Parse(item.Content));     }     //Terrains     public TerrainIns SelectTerrain(float x, float y)     {         return SelectTerrain(new Vector2(x, y));     }     public TerrainIns SelectTerrain(Vector2 pos)     {         return Marker.Closest(_markers, pos, Key).Terrain;     }     //Elements     private void SetAvailableMarketElements(TerrainIns terrain)     {         _elementTypes.Clear();         if (terrain.HasElement)             for (int i = 0; i < _allElements.Count; i++)                 if (_allElements[i].FavoriteTerrainTypes == terrain.Type)                     _elementTypes.Add(_allElements[i]);     }     void SetElements(char[,] charMap, Vector2 location)     {         int elementMass = RandomHelper.Range(location.x, location.y, Key, 14 * 14) / 4;         for (int i = 0; i < elementMass; i++)         {
            //make x,y between 1-15 leave the boundaries clear 
            int x = RandomHelper.Range(                         location.x + i,                         location.y,                         Key,                         14                     ) + 1;             int y = RandomHelper.Range(                         location.x,                         location.y + i,                         Key,                         14                     ) + 1;             charMap[x, y] = 'B';         }     }     void LoadElements(Marker marker)     {         Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);         if (marker.HasElement)             SetElements(marker.CharMap, marker.Location);         else             return;         //Set up the map based on marker.CharMap          for (int x = 0; x < 16; x++)         {             for (int y = 0; y < 16; y++)             {                 //Skip Empty places                 if (marker.CharMap[x, y] == 'E' || marker.CharMap[x, y] == 'M')                     continue;                 var bLoc = new Vector3(                     rightCornerLocation.x + x,                     rightCornerLocation.y + y,                     0.01f);                 //So player doesn't land on a element                 if (bLoc.x == 0 && bLoc.y == 0) continue;                 if (marker.CharMap[x, y] == 'B')                     CreateElements(bLoc);             }         }     }     public ActiveElementType CreateElements(Vector3 location)     {         var element = new GameObject();         var active = element.AddComponent<ActiveElementType>();         _elements.Add(active);         element.transform.position = location;         var renderer = element.AddComponent<SpriteRenderer>();         var elementInfo = _elementTypes[RandomHelper.Range(element.transform.position, Key, _elementTypes.Count)];         renderer.sprite = elementInfo.GetSprite();         renderer.sortingOrder = _baseSortIndex+  5;         active.ElementTypeInUse = elementInfo;          element.name = "Element " + element.transform.position;         element.transform.parent = transform;         return active;     }     public ActiveElementType GetElement(Vector2 pos)     {         foreach (var element in _elements)         {             var bLoc = element.transform.position;
            float distance = Vector2.Distance(bLoc, pos);             if (distance < 0.5)                 return element;         }         return null;     }     internal bool DestroyElement(ActiveElementType element, bool useTool)     {         if (useTool)             if (!_inv.ElementToolUse(element.ElementTypeInUse))                 return false;         _elements.Remove(element);         Destroy(element.gameObject);         return true;     }     //Monsters     private void SetAvailableMarketMonsters(TerrainIns terrain)     {
        //Todo:Performance: this get executed 9 time each time the terrain load not good          _monsterTypes.Clear();         if (terrain.HasMonster)             for (int i = 0; i < _allMonster.Count; i++)                 if (_allMonster[i].FavoriteTerrainTypes == terrain.Type )                     _monsterTypes.Add(_allMonster[i]);     }     private void SetMonsters(char[,] charMap, Vector2 location)     {         int monstersMass = RandomHelper.Range(location.x, location.y, Key, 14 * 14) /30;         for (int i = 0; i < monstersMass; i++)         {
            //make x,y between 2-14 leave the boundaries clear 
            int x = RandomHelper.Range(                         location.x - i,                         location.y,                         Key,                         14                     ) + 1;             int y = RandomHelper.Range(                         location.x,                         location.y - i,                         Key,                         14                     ) + 1;             if (charMap[x, y] != 'E')                 continue;             charMap[x, y] = 'M';         }     }     void LoadMonsters(Marker marker)     {         Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);          if (marker.HasMonster)             SetMonsters(marker.CharMap, marker.Location);         else              return;         //Set up the map based on marker.CharMap          for (int x = 0; x < 16; x++)         {             for (int y = 0; y < 16; y++)             {                 //Skip Empty places                 if (marker.CharMap[x, y] == 'E' || marker.CharMap[x, y] == 'B')                     continue;                 var bLoc = new Vector3(                     rightCornerLocation.x + x,                     rightCornerLocation.y + y,                     0.01f);                 //todo: destroy anything that is close by player                 //So player doesn't land on a element                 if (bLoc.x == 0 && bLoc.y == 0) continue;                 if (marker.CharMap[x, y] == 'M')                     CreateMonsters(bLoc);             }         }     }     private Character SetMonsterTypeBasedOnRarity(Vector3 position)     {         var rarity = RandomHelper.Range(position, DateTime.Now.DayOfYear, (int)DataTypes.Rarity.Common);         List<Character> monsterList = new List<Character>();         for (int i = 0; i < _monsterTypes.Count; i++)             if ((int)_monsterTypes[i].Rarity >= rarity)                 monsterList.Add(_monsterTypes[i]);         //print("Rarity=" + rarity + " CNT=" + monsterList.Count+" Monster =" + monsterList[RandomHelper.Range(position, Key, monsterList.Count)].Name);         return monsterList[RandomHelper.Range(position, Key, monsterList.Count)];     }
    private void CreateMonsters(Vector3 location)     {         GameObject monster = Instantiate(_monsterObj);         monster.transform.position = location;         monster.transform.SetParent(transform);          monster.name = "Monster " + monster.transform.position;           var active = monster.GetComponent<ActiveMonsterType>();         var spriteRenderer = monster.GetComponent<SpriteRenderer>();         active.Location = location;         active.Alive = true;         active.Hidden = true;          _monsters.Add(active);          var monsterCharacter = SetMonsterTypeBasedOnRarity(monster.transform.position);          spriteRenderer.sprite = monsterCharacter.GetSprite();          if (monsterCharacter.Move == Character.CharacterType.Fly)             spriteRenderer.sortingOrder = _baseSortIndex +  6;         else             spriteRenderer.sortingOrder = _baseSortIndex +  3;         if (monsterCharacter.IsAnimated)         {             var animator = monster.GetComponent<Animator>();             animator.runtimeAnimatorController = monsterCharacter.GetAnimator();             animator.speed = monsterCharacter.Move == Character.CharacterType.Fly ? 1 : 0;         }          var monsterLevel = _characterManager.CharacterSetting.Level;         if (monsterLevel < 3)             monsterLevel += 2;         RandomHelper.RangeMinMax(monsterLevel - 2, monsterLevel + 3);         active.MonsterType = _characterManager.GenerateMonster(monsterCharacter, monsterLevel);          var textLevelRenderer = monster.GetComponentInChildren<TextMeshPro>();         textLevelRenderer.text = "Level " + active.MonsterType.Level;         monster.GetComponentInChildren<MeshRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;         //Hide the monster in the map till they get in the line of Sight         monster.gameObject.SetActive(false);     }     private void UnCacheElements()     {         foreach (var element in _cache.FindPersist("VacantElement"))         {             var currentElement = GetElement(element.Location);             if (currentElement != null)                 DestroyElement(currentElement, false);         }     }     public IEnumerable<ActiveMonsterType> GetMonster(Vector3 pos, float radius)     {         pos.z = 0;         foreach (var monster in _monsters)         {             if (monster.Alive)             {                 var bLoc = monster.transform.position;                 //MapPos in between the monster                  var viewLimit = monster.MonsterType.View;                 if (Math.Abs(radius) > 0)                     viewLimit = radius;                 if (Vector2.Distance(pos, bLoc) < viewLimit)                     yield return monster;             }         }     }     internal bool DestroyMonster(ActiveMonsterType monster, bool drop)     {         print("Killing Monster " + monster.Location + (drop?" Drop Item":" "));         if (drop)         {             var deadPos = monster.Location;             _cache.PersistAdd(new CacheContent()                 {                     Location = deadPos,                     ObjectType = "DeadMonster",                     ExpirationTime = DateTime.Now.AddHours(-19)                   }             );             var monsterCharacter = monster.MonsterType.GetCharacter();             DeadMonster(monster.transform.position, monsterCharacter);         }         _monsters.Remove(monster);         Destroy(monster.gameObject);         return true;     }     private void UnCacheMonsters()     {         foreach (var monster in _cache.FindPersist("DeadMonster"))         {             foreach (var currentMonster in GetMonster(monster.Location, 3))                 DestroyMonster(currentMonster, false);         }     }     public static Vector3 StringToVector3(string sVector)     {         // Remove the parentheses         if (sVector.StartsWith("(") && sVector.EndsWith(")"))         {             sVector = sVector.Substring(1, sVector.Length - 2);         }          // split the items         string[] sArray = sVector.Split(',');          // store as a Vector3         Vector3 result = new Vector3(             Single.Parse(sArray[0]),             Single.Parse(sArray[1]),             Single.Parse(sArray[2]));          return result;     }     //Digging     public bool CreateDigging(Vector3 location,bool useTool)     {         if (useTool)             if (!_inv.ElementToolUse())                 return false;         GameObject dig = new GameObject();         _digs.Add(dig);         dig.transform.position = location;         var renderer = dig.AddComponent<SpriteRenderer>();         renderer.sprite = Dig;         renderer.sortingOrder = _baseSortIndex + 1;         dig.name = "Dig " + dig.transform.position;         dig.transform.parent = transform;         //print("CreateDigging : "+ dig.name);         return true;     }     //Items     public void CreateItem(Vector3 location, int itemId)     {         GameObject item = new GameObject();         var active = item.AddComponent<ActiveItemType>();         active.ItemTypeInUse = _itemDatabase.GetItemById(itemId);         location.z -= 0.001f;         active.Location = location;         _activeItems.Add(active);         item.transform.position = location;         item.transform.localScale += new Vector3(0.1f, 0, 0);         var renderer = item.AddComponent<SpriteRenderer>();         renderer.sprite = active.ItemTypeInUse.GetSprite();         renderer.sortingOrder =  _baseSortIndex+  2;         item.name = active.ItemTypeInUse.Name + item.transform.position;         item.transform.parent = transform;     }
    internal ActiveItemType GetDropItem(Vector3 pos)
    {         foreach (var item in _activeItems)         {             var bLoc = item.transform.position;
            float distance = Vector2.Distance(bLoc, pos);             if (distance<0.2)                 return item;         }         return null;     }     internal void DestroyItem(ActiveItemType item)     {         var bLoc = item.transform.position;         _activeItems.Remove(item);         Destroy(item.gameObject);         //Todo: also delete other items in that radius          foreach (var cacheItem in _cache.Find("Item", bLoc, 1, true))             print("DestroyItem:  item:" + cacheItem.Content + cacheItem.ObjectType + cacheItem.Location );     }     //Logic should match with BuildingDropItem
    public void TerrainDropItem(Vector3 pos, float chance, string dropItems)     {         int itemId;
        if (RandomHelper.GetLucky(pos, chance))             itemId = _itemDatabase.GetItemIdBasedOnRarity(pos, dropItems);         else //Drop coin/Gem/Recipe               itemId = _itemDatabase.GetItemIdBasedOnRarity(pos);         //print("itemId = "+ itemId +" in pos ="+ pos);         if (itemId != -1)         {             _terrainManager.CreateItem(pos, itemId);             _cache.Add(new CacheContent()                 {                     Location = pos,                     Content = itemId.ToString(),                     ObjectType = "Item"                 }             );         }
    }
    //Player
    private void SetPlayerLocation()     {         Marker marker = Marker.Closest(_markers, new Vector2(_player.position.x, _player.position.y), Key);         if (!marker.Terrain.Walkable)             foreach (var newMarker in _markers)             {                 if (newMarker.Terrain.Walkable)                 {                     marker = newMarker;                     break;                 }             }         for (int r = 0; r < 8; r++) //Radiate from the center to find closest empty open space in the middle              for (int x = 8 - r; x < 8 + r; x++)                 for (int y = 8 - r; y < 8 + r; y++)                     if (marker.CharMap[x, y] == 'E')                     {                         Vector2 rightCornerLocation = new Vector2(marker.Location.x - 8, marker.Location.y - 8);                         _player.position = new Vector3(                             rightCornerLocation.x + x,                             rightCornerLocation.y + y,                             0);                         //print("###Inside Set Player location After: " + Player.position + marker.Terrain.Name);                         return;                     }     }     public static TerrainManager Instance()     {         if (!_terrainManager)         {             _terrainManager = FindObjectOfType(typeof(TerrainManager)) as TerrainManager;             if (!_terrainManager)                 Debug.LogError("There needs to be one active TerrainManager script on a GameObject in your scene.");         }         return _terrainManager;     } }