using UnityEngine;
public class AudioManager : MonoBehaviour 
{
    private AudioClip[] _bgAudioClips;
    private AudioClip[] _insideAudioClips;
    private AudioClip[] _fightAudioClips;
    private AudioSource _audioSource;
    private bool _playingBg;
    private static AudioManager _audioManager;

    void Awake()
    {
        //Make it Singleton
        _audioManager = AudioManager.Instance();
    }
    void Start () {
        _bgAudioClips = Resources.LoadAll<AudioClip>("Audio/BackGround");
        _insideAudioClips = Resources.LoadAll<AudioClip>("Audio/Inside");
        _fightAudioClips = Resources.LoadAll<AudioClip>("Audio/Inside");
        _audioSource = gameObject.GetComponent<AudioSource>();
        //print("Music : Main=" + _bgAudioClips.Length + " Inside=" + _insideAudioClips.Length);
    }
    public void UpdateSoundVolume(float value)
    {
        if (value == 0)
            _audioSource.mute = true;
        else
            _audioSource.mute = false;
        _audioSource.volume = value;
    }
    public void PlayBgMusic(Vector3 pos, int key)
    {
        if (_playingBg)
            return;
        if (_bgAudioClips.Length == 0)
            return;
        var ac = _bgAudioClips[RandomHelper.Range(pos, key, _bgAudioClips.Length)];
        _audioSource.clip = ac;
        _audioSource.Play();
        _playingBg = true;
    }
    public void PlayInsideMusic(Vector3 pos, int key)
    {
        if (_insideAudioClips.Length == 0)
            return;
        var ac = _insideAudioClips[RandomHelper.Range(pos, key, _insideAudioClips.Length)];
        _audioSource.clip = ac;
        _audioSource.Play();
    }
    public void PlayFightMusic(Vector3 pos, int key)
    {
        if (_fightAudioClips.Length == 0)
            return;
        var ac = _fightAudioClips[RandomHelper.Range(pos, key, _fightAudioClips.Length)];
        _audioSource.clip = ac;
        _audioSource.Play();
    }
    public static AudioManager Instance()
    {
        if (!_audioManager)
        {
            _audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            if (!_audioManager)
                Debug.LogWarning("####There needs to be one active AudioManager script on a GameObject in your scene.");
        }
        return _audioManager;
    }
}