using UnityEngine;

public class MoveManager : MonoBehaviour {
    
    public KeyCode EnableFastSpeedWithKey = KeyCode.LeftShift;
    //public Transform TurnWithMovement;

    private int _baseSortIndex = 0;
    private CharacterManager _characterManager;
    private Character _playerCharacter;
    private CharacterSetting _playerCharacterSetting;

    private Sprite _up;
    private Sprite _down;
    private Sprite _right;
    private Sprite _left;
    
    private RuntimeAnimatorController _playerAnimeCtrl;

    private GameObject _player;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private Vector3 _movement;
    private Vector3 _destiny;
    
    private bool _stop;

    // Use this for initialization
    private void Start ()
    {
        _characterManager = CharacterManager.Instance();
        _playerCharacter = _characterManager.MyCharacter;
        _playerCharacterSetting = _characterManager.CharacterSetting;

        _player = GameObject.FindGameObjectWithTag("Player");
        _renderer = _player.GetComponent<SpriteRenderer>();

        _animator = _player.GetComponent<Animator>();
        if (_animator == null)
            _animator = _player.AddComponent<Animator>();

        SetMoveSprites();
        SetMoveAnimation();

        if (_playerCharacter.IsAnimated)
            _animator.runtimeAnimatorController = _playerAnimeCtrl;
        else
            _renderer.sprite = _down;
    }
    // Update is called once per frame
    void Update ()
    {
        if (Vector2.Distance(_destiny, transform.position) < 0.2 && _stop)
            _movement = Vector3.zero;
        else
        {
            //todo: Debug: Use keyboard 
            Vector3 keyboardUse = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            if (keyboardUse != Vector3.zero)
                _movement = keyboardUse;
        }
        //Change Sprite or Animation according to the direction of moving
        if (_playerCharacter.IsAnimated)
            HandleAnimation(_movement);
        else
            HandleSprite(_movement);
    }
    internal void SetStop(bool value)
    {
        _stop = value;
    }
    public void SetMovement(Vector3 location)
    {
        _stop = true;
        var direction = (location -transform.position);
        
        _movement = direction.normalized;
        _destiny = location;
    }
    private void FixedUpdate()
    {
        if (!_playerCharacterSetting.FightMode)
            DoMove(_movement);
    }
    private void DoMove(Vector3 movement)
    {
        var currentSpeed = _playerCharacterSetting.Speed;
        //todo:delete
        if (Input.GetKey(EnableFastSpeedWithKey))
        {
            currentSpeed += 2;
        }
        //Old moving system
        transform.Translate(movement * currentSpeed * Time.deltaTime );
        //transform.position = Vector3.Lerp(transform.position, _destiny, Time.deltaTime * currentSpeed);
    
        if (_playerCharacter.Move == Character.CharacterType.Fly)
            _renderer.sortingOrder = _baseSortIndex + 6;
        else
            _renderer.sortingOrder = _baseSortIndex + 3;
    }
    private void HandleAnimation(Vector3 movement)
    {
        if (_playerCharacter.Move != Character.CharacterType.Fly && movement == Vector3.zero)
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
    //TODO: Attach animation and layer
    //https://www.youtube.com/watch?v=aOqQuD_1ylA
    //https://www.youtube.com/watch?v=Y03jBu6enf8 (some movement improvement animantions on layers)
    
    private void SetMoveSprites()
    {
        var sprites = _playerCharacter.GetSprites();
        _right = sprites[0];
        _left = sprites[1];
        _up = sprites[2];
        _down = sprites[3];
    }
    private void SetMoveAnimation()
    {
        // Load Animation Controllers
        _playerAnimeCtrl = _playerCharacter.GetAnimator();
    }
}