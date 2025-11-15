using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;

public class objectGen : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public int spawnCount = 20;
    public float scaleVar = 1.0f;
    // size of the spawn area (box) centered on this GameObject
    public Vector3 spawnAreaSize = new Vector3(10f, 1f, 10f);
    public bool parentToThis = true;
    public NavMeshSurface navMeshSurface;
    void Start()
    {
        if (prefabs == null || prefabs.Count == 0)
        {
            Debug.LogWarning("objectGen: No prefabs assigned to 'prefabs'. Nothing to spawn.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];

            Vector3 half = spawnAreaSize * 0.5f;
            Vector3 localPos = new Vector3(
                Random.Range(-half.x, half.x),
                Random.Range(-half.y, half.y),
                Random.Range(-half.z, half.z)
            );
            Vector3 randomScale = Random.Range(1.0f - scaleVar, 1.0f + scaleVar) * Vector3.one;
            Vector3 spawnPos = transform.position + localPos;

            // random rotation
            Quaternion rot = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            GameObject instance = Instantiate(prefab, spawnPos, rot);
            instance.transform.localScale = randomScale;
            if (parentToThis) instance.transform.parent = this.transform;
        }

        AfterInit();
    }
    void AfterInit()
    {
        // After spawning all objects, build/rebuild the NavMesh
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("objectGen: NavMeshSurface reference is missing. Cannot build NavMesh.");
        }
    }
    // void Update()
    // {
        
    // }
}
