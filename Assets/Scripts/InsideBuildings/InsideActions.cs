using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InsideActions : MonoBehaviour {

    private CharacterManager _characterManager;
    private InventoryHandler _inv;
    private BuildingInterior _building;

    private static GameObject _popupAction;

    // Use this for initialization
    void Start ()
    {
        _characterManager = CharacterManager.Instance();
        _inv = InventoryHandler.Instance();
        _popupAction = GameObject.Find("Popup Action");
        _building = GameObject.Find("Building Interior").GetComponent<BuildingInterior>();
    }
	// Update is called once per frame
	void Update ()
	{
	    var pos = transform.position;
        InLineOfSight(pos);
	    pos.z += 0.01f;
	    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
	    {
	        var touchLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	        float distance = Vector2.Distance(touchLocation, pos);
	        _popupAction.SetActive(false);

            //AttackTarget Also check TerrainAction
            var monster = _building.GetMonster(touchLocation,0.3f).FirstOrDefault();
	        if (monster != null)
	        {
	            if (distance < (int) _characterManager.MyCharacter.AttackR)
	            {
                    _popupAction.GetComponent<ActionHandler>().SetActiveMonster(monster, transform, "Inside");
                    _popupAction.GetComponent<ActionHandler>().AttackMonster();
	                //Less click approach 
                    //CreateFloatingAction(touchLocation, "Monster");
	            }
	            else
	                _inv.PrintMessage("Too Far from this Target!", Color.yellow);
	            return;
            }
	        var currentItem = _building.GetItem(touchLocation).FirstOrDefault();
            if (currentItem != null)
	        {
	            if (distance < 1)
	            {
	                _popupAction.GetComponent<ActionHandler>().SetActiveItem(currentItem, "Inside");
	                _popupAction.GetComponent<ActionHandler>().GrabItem();
	                //Less click approach 
                    //CreateFloatingAction(touchLocation, "Item");
	                return;
	            }
	        }
            CreateFloatingAction(touchLocation, "Map");
        }
	}
    public static void CreateFloatingAction(Vector3 location, string action)
    {
        _popupAction.SetActive(true);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location);
        _popupAction.transform.position = screenPosition;
        _popupAction.GetComponent<ActionHandler>().SetAction(location, action);
    }
    private void InLineOfSight(Vector3 pos)     {         //You are in the line of sight a monster          foreach (var monster in _building.GetMonster(pos,0))             monster.SawTarget = true;     } }