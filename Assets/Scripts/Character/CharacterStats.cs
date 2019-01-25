using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    private CharacterManager _characterManager;

    private CharacterSetting _settings;

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
        _settings = _characterManager.CharacterSetting;
    }

    void Start ()
    {
        if (_health == null)
            _health = GameObject.FindGameObjectWithTag("Health").GetComponent<BarHandler>();
        if (_mana == null)
            _mana = GameObject.FindGameObjectWithTag("Mana").GetComponent<BarHandler>();
        if (_energy == null)
            _energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<BarHandler>();
        if (_experience == null)
            _experience = GameObject.FindGameObjectWithTag("Experience").GetComponent<BarHandler>();
        if (_coin == null)
            _coin = GameObject.FindGameObjectWithTag("Coin").GetComponent<ContainerValueHandler>();
        if (_gem == null)
            _gem = GameObject.FindGameObjectWithTag("Gem").GetComponent<ContainerValueHandler>();
        
        _health.UpdateValues(_settings.Health, _settings.MaxHealth);
        _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
        _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
        _experience.UpdateValues(_settings.Experience, _settings.MaxExperience, _settings.Level);
        _coin.UpdateValue(_settings.Coin);
        _gem.UpdateValue(_settings.Gem);
    }
	
	// Update is called once per frame
	void Update () {
        if (_settings.Updated)
        {
            print("_settings.Updated");
            _health.UpdateValues(_settings.Health, _settings.MaxHealth);
            _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
            _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
            _experience.UpdateValues(_settings.Experience, _settings.MaxExperience, _settings.Level);
            _coin.UpdateValue(_settings.Coin);
            _gem.UpdateValue(_settings.Gem);
            _settings.Updated = false;
        }
    }
}
