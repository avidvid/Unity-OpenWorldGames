using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {
    public ActiveMonsterType Target;
    public float AttackValue;
    public string MonsterType;
    public float MoveSpeed = 2;
    private Vector2 _direction;
    private static GameObject _popupText;
    private static GameObject _canvas;

    void Start ()
    {
        _direction = Target.transform.position - transform.position;
        float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg +180;
        //Rotate the spell
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        _direction = Quaternion.AngleAxis(angle, Vector3.forward) * _direction;

        _canvas = GameObject.Find("Canvas");
        _popupText = Resources.Load<GameObject>("Prefabs/PopUpAttackParent");
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(Target.transform.position, transform.position);
        if (distance > 0.1)
            transform.Translate(_direction * MoveSpeed * Time.deltaTime);
        else
        {
            GameObject go = GameObject.FindWithTag("Player");
            var other = go.GetComponent<Attack>();
            other.AttackDealing(Target, AttackValue, MonsterType);
            CreateFloatingText(((int)AttackValue).ToString() , transform.position);
            GameObject.Destroy(gameObject);
        }
    }
    public static void CreateFloatingText(string text, Vector3 location)
    {
        var popupText = Instantiate(_popupText);
        //Randomize the location 
        location = new Vector3(location.x+Random.Range(-.2f,.2f), location.y + Random.Range(-.2f, .2f), location.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(location);
        popupText.transform.SetParent(_canvas.transform, false);
        popupText.transform.position = screenPosition;
        popupText.GetComponent<FloatingText>().SetText(text);
    }
}