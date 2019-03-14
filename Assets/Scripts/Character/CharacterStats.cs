using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    private CharacterManager _characterManager;

    private CharacterSetting _settings;
    private UserPlayer _player;

    private bool _inCombat;

    private BarHandler _health;
    private BarHandler _mana;
    private BarHandler _energy;
    private BarHandler _experience;
    private ContainerValueHandler _coin;
    private ContainerValueHandler _gem;

    // Use this for initialization
    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        var combat = GameObject.Find("Combat");
        if (combat != null)
            _inCombat = true;
        if (_health == null)
            _health = GameObject.FindGameObjectWithTag("Health").GetComponent<BarHandler>();
        if (_mana == null)
            _mana = GameObject.FindGameObjectWithTag("Mana").GetComponent<BarHandler>();
        if (!_inCombat)
        {
            if (_energy == null)
                _energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<BarHandler>();
            if (_experience == null)
                _experience = GameObject.FindGameObjectWithTag("Experience").GetComponent<BarHandler>();
            if (_coin == null)
                _coin = GameObject.FindGameObjectWithTag("Coin").GetComponent<ContainerValueHandler>();
            if (_gem == null)
                _gem = GameObject.FindGameObjectWithTag("Gem").GetComponent<ContainerValueHandler>();
        }
    }

    void Start ()
    {
        _settings = _characterManager.CharacterSetting;
        _player = _characterManager.UserPlayer;

        _health.UpdateValues(_settings.Health, _settings.MaxHealth);
        _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
        if (!_inCombat)
        {
            _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
            _experience.UpdateValues(_settings.Experience, _settings.MaxExperience, _settings.Level);
            _coin.UpdateValue(_settings.Coin);
            _gem.UpdateValue(_player.Gem);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_settings.Updated)
        {
            print("_settings.Updated");
            _health.UpdateValues(_settings.Health, _settings.MaxHealth);
            _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
            if (!_inCombat)
            {
                _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
                _experience.UpdateValues(_settings.Experience, _settings.MaxExperience, _settings.Level);
                _coin.UpdateValue(_settings.Coin);
                _gem.UpdateValue(_player.Gem);
            }
            _settings.Updated = false;
        }
    }
}
