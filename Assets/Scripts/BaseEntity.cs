using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static BaseEntity;

public class BaseEntity : MonoBehaviour
{
    public StatsManager _statsManager;
    public virtual void Init(){
        // _statsManager = new StatsManager();
        _statsManager.MaxHealth();
    }
    public void Awake()
    {
        
    }
    public virtual void Start()
    {
        Init();
    }

    public void HealthModify(int amount)
    {
        if (amount == 0) return;
        else if (amount < 0)
        {
            OnHit();
        }
        _statsManager.currentHealth += amount;
        if (_statsManager.currentHealth > _statsManager.maxHealth)
        {
            _statsManager.currentHealth = _statsManager.maxHealth;
        }
        else if (_statsManager.currentHealth <= 0)
        {
            _statsManager.currentHealth = 0;
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        
    }
    public virtual void OnHit() 
    {
        
    }
    public virtual void OnAttack()
    {
        
    }
}
