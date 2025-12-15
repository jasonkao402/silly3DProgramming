using System.Collections.Generic;
using UnityEngine;

public enum PoolLimitStrategy
{
    RecycleOldest,   // 強制回收最舊
    ExpandPool       // 增生更多物件
}

[CreateAssetMenu(fileName = "PoolConfigDatabase", menuName = "Pooling/Pool Config Database")]
public class PoolConfigDatabase : ScriptableObject
{
    public List<PoolConfigEntry> entries = new();
}

[System.Serializable]
public class PoolConfigEntry
{
    public GameObject prefab;
    public int initialSize = 32;
    public PoolLimitStrategy limitStrategy = PoolLimitStrategy.RecycleOldest;
}

public interface IPoolable
{
    void OnSpawned();   // 從 Pool 拿出來時呼叫
    void OnDespawned(); // 回到 Pool 時呼叫
}