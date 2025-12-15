using UnityEngine;

public abstract class PoolObjectBase : MonoBehaviour, IPoolable
{
    // PoolManager 在 Spawn 時會設定這個
    [HideInInspector] public GameObject prefabRef;

    public virtual void OnSpawned() { }
    public virtual void OnDespawned() { }

    // 呼叫回收
    public void Despawn()
    {
        PoolManager.Instance.Despawn(prefabRef, gameObject);
    }
}