using UnityEngine;
using UnityEngine.Rendering;
using System;

[System.Serializable]
public class StatsManager
{
    public CharacterStatsSO characterStatsSO;
    public int currentHealth;
    public int maxHealth => characterStatsSO.maxHealth;
    public float movementSpeed => characterStatsSO.movementSpeed;
    public int MaxHealth()
    {
        currentHealth = characterStatsSO.maxHealth;
        return currentHealth;
    }
}
