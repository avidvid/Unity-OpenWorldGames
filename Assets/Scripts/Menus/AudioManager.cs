using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //private List<AudioClip> _bgAudioClips = new List<AudioClip>();
    //private List<AudioClip> _insideAudioClips = new List<AudioClip>();
    private AudioClip[] _bgAudioClips;
    private AudioClip[] _insideAudioClips;
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
        _audioSource = gameObject.GetComponent<AudioSource>();
        print("Music : Main=" + _bgAudioClips.Length + " Inside=" + _insideAudioClips.Length);
    }
	
    public void PlayBgMusic(Vector3 pos, int key)
    {
        if (_playingBg)
            return;
        var ac = _bgAudioClips[RandomHelper.Range(pos, key, _bgAudioClips.Length)];
        _audioSource.clip = ac;
        _audioSource.Play();
        _playingBg = true;
    }

    public void PlayInsideMusic(Vector3 pos, int key)
    {
        _playingBg = false;
        var ac = _insideAudioClips[RandomHelper.Range(pos, key, _insideAudioClips.Length)];
        _audioSource.clip = ac;
        _audioSource.Play();
    }


    public static AudioManager Instance()
    {
        if (!_audioManager)
        {
            _audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            if (!_audioManager)
                Debug.LogError("There needs to be one active AudioManager script on a GameObject in your scene.");
        }
        return _audioManager;
    }
}
