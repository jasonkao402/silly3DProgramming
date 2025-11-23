using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static BaseEntity;

public class ChainEntity : BaseEntity
{
    public GameObject hitEffectPrefab, deathEffectPrefab;
    public override void OnDeath()
    {
        if (deathEffectPrefab == null) return;
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public override void OnHit() 
    {
        if (hitEffectPrefab == null) return;
        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
    }
}
