using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    private TerrainDatabase _terrainDatabase;
    private CharacterManager _characterManager;

    private int _horizontalTiles = 11;
    private int _verticalTiles = 7;
    private int _key;
    private List<TerrainIns> _availableTerrainTypes;

    private TerrainIns _terrain;
    private List<Marker> _markers;
    private Vector3 _previousPosition = Vector3.zero;
    private Vector2 _mapPosition = Vector2.zero;
    private GameObject _mapPanel;
    // Start is called before the first frame update
    void Start()
    {
        _terrainDatabase = TerrainDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
        {
            _mapPosition = starter.MapPosition;
            _key = starter.Key;
            _previousPosition = starter.PreviousPosition;
        }
        DrawMap();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public TerrainIns GetTerrain(float x, float y, int key)
    {
        x = (int)x >> 4;
        y = (int)y >> 4;
        return _availableTerrainTypes[RandomHelper.Range(x , y , key, _availableTerrainTypes.Count)];
    }     void DrawMap()
    {
        _availableTerrainTypes = _terrainDatabase.GetTerrains();
        Debug.Log("CombatManager-Available TerrainTypes.Count = " + _availableTerrainTypes.Count);
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
                {
                    if (animator != null)
                        GameObject.Destroy(animator);
                }
            }
        }
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