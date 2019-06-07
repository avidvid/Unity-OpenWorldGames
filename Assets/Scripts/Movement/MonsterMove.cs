using System.Collections;
using UnityEngine;

public class MonsterMove : MonoBehaviour {

    private CharacterManager _characterManager;

    private int _baseSortIndex = 0;
    
    private ActiveMonsterType _active;
    private Transform _player;
    private Character _character;
    

    private SpriteRenderer _renderer;
    private Animator _animator;
    private Sprite _up;
    private Sprite _down;
    private Sprite _right;
    private Sprite _left;

    private int _attackInterval = 2;
    private float _attackNextTime = 0;


    void Awake()
    {
        _characterManager = CharacterManager.Instance();
    }

    // Use this for initialization
    void Start ()
    {
        _active = transform.GetComponent<ActiveMonsterType>();
        _character = _active.MonsterType.GetCharacter();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _renderer = transform.GetComponent<SpriteRenderer>();
        _animator = transform.GetComponent<Animator>();
        if (_character.IsAnimated)
            _animator.runtimeAnimatorController = GetMoveAnimation();
        else
        {
            SetMoveSprites();
            _renderer.sprite = _down;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_characterManager.CharacterSetting.Alive)
        {
            if (_active.SawTarget)
            {
                if (_character.IsAnimated)
                    HandleAnimation(Vector3.zero);
                else
                    HandleSprite(Vector3.zero);
                _active.SawTarget = false;
            }
            return;
        }
        Vector3 targetPos = _player.position;
        Vector3 monsterPos = transform.position;
        float dist = Vector2.Distance(targetPos, monsterPos);
        if (_active.SawTarget)
        {
            print("I saw you");
            if (dist > (int)_character.AttackR)
            {
                if (_character.IsAnimated && _character.HasFightMode && _character.Speed == 0)
                    HandleFightMode(true);
                else if (_character.IsAnimated && _character.HasFightMode)
                    HandleFightMode(false);

                var movement = (targetPos - monsterPos).normalized;
                if (_character.IsAnimated)
                    HandleAnimation(movement);
                else
                    HandleSprite(movement);
                if (_character.Speed > 0)
                    DoMove(movement, (float)_character.Speed, dist);
            }
            else if (_character.HasFightMode)
                HandleFightMode(true);
            else
                _active.AttackMode = true;
        }
        else if (_character.IsAnimated && _character.HasFightMode)
            HandleFightMode(false);
        if (_active.AttackMode)
            AttackTimeCheck();
    }
    private void AttackTimeCheck()
    {
        //Do attack here every interval seconds
        if (Time.time >= _attackNextTime)
        {
            StartCoroutine(AttackPlayer(_active.MonsterType));
            if (_attackNextTime + _attackInterval < Time.time)
                _attackNextTime = Time.time;
            _attackNextTime += _attackInterval;
        }
    }
    private IEnumerator AttackPlayer(MonsterIns monsterType)
    {
        //todo: this is a hardcoded cast time for debugging 
        yield return new WaitForSeconds(1);
        //1-A Find Direction
        //2-A calculate Att
        var attAmount = RandomHelper.AbsZero(monsterType.AbilityAttack  - _characterManager.CharacterSetting.AbilityDefense)
                        + RandomHelper.AbsZero(monsterType.MagicAttack  - _characterManager.CharacterSetting.MagicDefense)
                        + RandomHelper.AbsZero(monsterType.PoisonAttack - _characterManager.CharacterSetting.PoisonDefense);
        //3-A calculate dealAtt
        var dealAtt = RandomHelper.CriticalRange(attAmount);

        print("MonsterLv" + monsterType.Level  + monsterType.GetInfo("Attack") + 
              "-> Player" + _characterManager.CharacterSetting.GetInfo("Defense") +
              " PlayerHealth (" + _characterManager.CharacterSetting.Health + ")" +
              " = " + dealAtt + "/" + attAmount 
              +" Critical =" + (dealAtt > attAmount));
        //5-A Cast Spell (Lunch spell-Drop dealAtt hit- AttackDealing)
        CastSpell(dealAtt);
    }
    private void DoMove(Vector3 movement, float speed,float distance)
    {
        movement = movement.normalized;
        //print("Monster Movement=" + movement + " " + movement.normalized + " " + distance+ " speed " + speed);
        //Old moving system
        //todo: if blocked do something logical
        transform.Translate(movement *  Time.deltaTime);
        _renderer.sortingOrder = _baseSortIndex + 7;
    }
    private void HandleFightMode(bool stat)
    {
        _active.AttackMode = stat;
        _animator.SetBool("fight", stat);
    }
    private void HandleAnimation(Vector3 movement)
    {
        if (_character.Move != Character.CharacterType.Fly && movement == Vector3.zero)
            _animator.speed = 0;
        else
            _animator.speed = 1;
        if (movement == Vector3.zero)
            return;
        if (movement.x > 0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            movement = Vector3.right;
        else if (movement.x < -0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            movement = Vector3.left;
        else if (movement.y > 0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            movement = Vector3.up;
        else if (movement.y < -0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            movement = Vector3.down;
        _animator.SetFloat("x", movement.x);
        _animator.SetFloat("y", movement.y);
    }
    private void HandleSprite(Vector3 movement)
    {
        if (movement.x > 0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            _renderer.sprite = _right;
        if (movement.x < -0.1f && Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            _renderer.sprite = _left;
        if (movement.y > 0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            _renderer.sprite = _up;
        if (movement.y < -0.1f && Mathf.Abs(movement.y) >= Mathf.Abs(movement.x))
            _renderer.sprite = _down;
    }
    private void SetMoveSprites()
    {
        var sprites = _character.GetSprites();
        _right = sprites[0];
        _left = sprites[1];
        _up = sprites[2];
        _down = sprites[3];
    }
    private RuntimeAnimatorController GetMoveAnimation()
    {
        // Load Animation Controllers
        return _character.GetAnimator();
    }
    // CastSpell Logic
    private void CastSpell(float attackDealt)
    {
        var spell = new GameObject();
        spell.transform.position = _active.transform.position;
        spell.transform.parent = _active.transform.parent;
        spell.name = "Monster Spell";
        var spellRenderer = spell.AddComponent<SpriteRenderer>();
        spellRenderer.sortingOrder = _active.transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
        spellRenderer.sprite = _active.MonsterType.GetSpellSprite(); ;
        spell.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        var spellManager = spell.AddComponent<MonsterSpellManager>();
        spellManager.Target = _player;
        spellManager.AttackValue = attackDealt;
        spellManager.MonsterType =  "Inside";
    }
}