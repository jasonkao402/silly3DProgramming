using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    [SerializeField] private PoolConfigDatabase database;
    private readonly Dictionary<GameObject, ObjectPool> pools = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
        foreach (var entry in database.entries)
        {
            if (entry.prefab == null) continue;
            GameObject poolParent = new GameObject($"Pool_{entry.prefab.name}");
            poolParent.transform.SetParent(transform);
            pools[entry.prefab] = new ObjectPool(entry, poolParent.transform);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(prefab))
        {
            Debug.LogError($"[PoolManager] No pool found for prefab: {prefab.name}");
            return null;
        }
        var pool = pools[prefab];

        GameObject obj = pool.Take();
        obj.transform.SetPositionAndRotation(pos, rot);

        // 記下 prefabRef，以便回收
        if (obj.TryGetComponent<PoolObjectBase>(out var p))
            p.prefabRef = prefab;
        // if (obj.TryGetComponent<enemyController>(out var q))
        //     q.prefabRef = prefab;

        obj.SetActive(true);
        (p as IPoolable)?.OnSpawned();
        
        return obj;
    }

    public void Despawn(GameObject prefab, GameObject instance)
    {
        var pool = pools[prefab];
        var p = instance.GetComponent<PoolObjectBase>();
        (p as IPoolable)?.OnDespawned();

        pool.Return(instance);
    }
}

public class ObjectPool
{
    private readonly PoolConfigEntry entry;
    private readonly Transform parent;
    // private readonly Queue<GameObject> idleQueue = new();
    private GameObject[] allObjects;
    private int _head = 0; // Next to take
    private int _tail = 0; // Next to return
    private int _availableCount = 0; // Count of available objects
    private int poolCapacity;
    public ObjectPool(PoolConfigEntry entry, Transform parent)
    {
        this.entry = entry;
        this.parent = parent;
        poolCapacity = entry.initialSize;
        allObjects = new GameObject[poolCapacity];
        for (int i = 0; i < poolCapacity; i++)
        {
            allObjects[i] = CreateNew();
        }
        _availableCount = poolCapacity;
    }
    GameObject CreateNew()
    {
        var obj = Object.Instantiate(entry.prefab, parent);
        obj.SetActive(false);
        return obj;
    }

    public GameObject Take()
    {
        if (_availableCount == 0)
        {
            Debug.LogWarning($"[ObjectPool] Pool for {entry.prefab.name} is exhausted. Applying limit strategy: {entry.limitStrategy}");
            // ExpandPool = capacity ×2
            if (entry.limitStrategy == PoolLimitStrategy.ExpandPool)
            {
                int oldCapacity = poolCapacity, i = 0;
                poolCapacity *= 2;
                var newBuffer = new GameObject[poolCapacity];
                for (i = 0; i < oldCapacity; i++)
                {
                    newBuffer[i] = allObjects[(_head + i) % oldCapacity];
                }
                for (; i < poolCapacity; i++)
                {
                    newBuffer[i] = CreateNew();
                }
                _head = 0;
                _tail = oldCapacity;
                _availableCount = poolCapacity;
                allObjects = newBuffer;
            }
            else // RecycleOldest
            {
                var obj = allObjects[_head];
                _head = (_head + 1) % poolCapacity;
                _tail = _head; // Overwrite oldest
                return obj;
            }
        }

        var result = allObjects[_head];
        _head = (_head + 1) % poolCapacity;
        _availableCount--;
        return result;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        allObjects[_tail] = obj;
        _tail = (_tail + 1) % poolCapacity;
        _availableCount++;
    }
}