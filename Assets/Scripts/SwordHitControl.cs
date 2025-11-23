using UnityEngine;

public class SwordHitControl : MonoBehaviour
{
    public BaseEntity parentEntity;
    public Collider swordCollider;
    public float repulsionForce = 10f;
    public bool isAttacking = false;
    void Awake()
    {
        swordCollider = GetComponent<Collider>();
        isAttacking = false;
    }
    public void ApplyRepellingForce()
    {
        if (!isAttacking) return;
        // 1. Detect Colliders
        // We use OverlapBox based on the 'bar' bounds. 
        // This creates an invisible box around the target collider.
        Collider[] hits = Physics.OverlapBox(
            swordCollider.bounds.center, 
            swordCollider.bounds.extents, 
            swordCollider.transform.rotation
        );
        
        foreach (Collider hit in hits)
        {
            // prevent the object from repelling itself
            if (hit == swordCollider || !hit.CompareTag("Enemy")) continue;
            Debug.Log("Applying repelling force to " + hit.name);
            // 2. Get Rigidbody
            Rigidbody rb = hit.attachedRigidbody;
            BaseEntity hitEntity = hit.GetComponent<BaseEntity>();
            if (hitEntity != null && hitEntity != parentEntity && rb != null)
            {
                hitEntity.HealthModify(-parentEntity._statsManager.characterStatsSO.attack);
            
                // Add an "Instant" force (Impulse)
                rb.AddExplosionForce(
                    repulsionForce, 
                    swordCollider.bounds.center, 
                    swordCollider.bounds.extents.magnitude, 
                    0f, 
                    ForceMode.Impulse
                );
            }
        }
    }
}