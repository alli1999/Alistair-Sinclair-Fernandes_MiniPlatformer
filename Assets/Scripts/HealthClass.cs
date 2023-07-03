using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthClass
{
    //Fields
    int _currentHealth;
    int _maxHealth;

    //Properties
    public int Health
    {
        get {
            return _currentHealth;
        }
        set{
            _currentHealth = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    //Constructor
    public HealthClass(int health, int maxHealth)
    {
        _currentHealth = health;
        _maxHealth = maxHealth;
    }

    //Methods
    public void DealDamage(int damageAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damageAmount;
        }
        if(_currentHealth < 0)
        {
            _currentHealth = 0;
        }
    }

    public void Heal(int healAmount)
    {
        if (_currentHealth < _maxHealth)
        {
            _currentHealth += healAmount;
        }
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}
