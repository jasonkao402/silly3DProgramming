using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static BaseEntity;

public class SlimeEntity : BaseEntity
{
    public GameObject hitEffectPrefab, deathEffectPrefab;
    public int slimeSize = 1;
    public int splitCount = 2;
    public Transform target;
    public float bounceHeight;
    public float bounceInterval;
    float lastBounceTime;
    public override void Start()
    {
        
        transform.localScale = Vector3.one * Mathf.Pow(1.5f, slimeSize - 1);
        lastBounceTime = Time.time;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        base.Start();
    }
    void FixedUpdate()
    {
        if (target != null && Time.time - lastBounceTime > bounceInterval)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddForce((direction + Vector3.up * bounceHeight) * _statsManager.movementSpeed, ForceMode.VelocityChange);
                lastBounceTime = Time.time;
            }
        }
    }
    public override void OnDeath()
    {
        if (slimeSize > 1){
        for (int i = 0; i < splitCount; i++)
            {
                SlimeEntity newSlimeEntity = Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<SlimeEntity>();
                newSlimeEntity.slimeSize = slimeSize - 1;
            }
        }
        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public override void OnHit() 
    {
        if (hitEffectPrefab == null) return;
        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
    }
}
