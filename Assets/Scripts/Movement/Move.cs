using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {
    private TerrainManager _terrainManager;
    private GUIManager _GUIManager;

    private Vector3 previousPosition = Vector3.zero;
    
    private CharacterManager _characterManager;
    private SpriteRenderer _renderer;
    private Cache _cache;

    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _terrainManager = TerrainManager.Instance();
        _GUIManager = GUIManager.Instance();
    }
    void Start ()
    {
        _renderer = transform.GetComponentsInChildren<SpriteRenderer>()[0];
    }
    private bool IsBlocked(Vector3 itemLocation, ActiveElementType element)
    {
        Vector3 currentPos = itemLocation;
        Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var terrain = _terrainManager.SelectTerrain(mapPos.x, mapPos.y);
        if (
            //Terrain Types and charcter type 
            (!terrain.Walkable && _characterManager.MyCharacter.Move == Character.CharacterType.Walk) ||
            (!terrain.Flyable && _characterManager.MyCharacter.Move == Character.CharacterType.Fly) ||
            (!terrain.Swimable && _characterManager.MyCharacter.Move == Character.CharacterType.Swim) ||
            //element + character
            (element != null &&  _characterManager.MyCharacter.Move != Character.CharacterType.Fly)
            )
        {
            return true;
        }
        return false;
    }
	void Update ()
	{
	    Vector3 currentPos = transform.position;
        Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var element = _terrainManager.GetElement(mapPos);
	    if (element != null)
	        if(element.transform.position.y> currentPos.y)
	            element.transform.GetComponent<SpriteRenderer>().sortingOrder = _renderer.sortingOrder -1;
            else
	            element.transform.GetComponent<SpriteRenderer>().sortingOrder = _renderer.sortingOrder + 1;
        if (element != null && element.ElementTypeInUse.Enterable)
        {
            //Use 1/4 of Max energy to enter a room
            if (_characterManager.UseEnergy(_characterManager.CharacterSetting.MaxEnergy / 1000))
            {
                //Preparing to switch the scene
                GameObject go = new GameObject();
                //Make go unDestroyable
                GameObject.DontDestroyOnLoad(go);
                var starter = go.AddComponent<SceneStarter>();
                starter.Key = TerrainManager.Key;
                starter.MapPosition = mapPos;
                starter.PreviousPosition = previousPosition;
                starter.LastScene = "Terrain";
                go.name = "Move SceneStarter";
                //switch the scene
                SceneManager.LoadScene(SceneSettings.SceneIdForInsideBuilding);
            }
            else
                _GUIManager.PrintMessage("Not enough energy to enter this place", Color.yellow);
        }
	    foreach (var monster in _terrainManager.GetMonster(mapPos, 0.3f))
	    {
	        if (monster.Alive)
	        {
	            //Preparing to switch the scene
	            GameObject go = new GameObject();
	            //Make go unDestroyable
	            GameObject.DontDestroyOnLoad(go);
	            var starter = go.AddComponent<SceneStarter>();
	            starter.Key = TerrainManager.Key;
	            starter.MapPosition = mapPos;
	            starter.PreviousPosition = previousPosition;
	            starter.LastScene = "Terrain";
	            go.name = "Move SceneStarter";
	            //switch the scene
	            SceneManager.LoadScene(SceneSettings.SceneIdForFightMonster);
	        }
        }
        if (IsBlocked(currentPos, element))
	        transform.position =  previousPosition;
        else
            previousPosition = currentPos;
    }
}
