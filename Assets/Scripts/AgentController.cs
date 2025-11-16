using UnityEngine;
using UnityEngine.AI; // Important: This is the namespace for NavMesh
using System.Collections.Generic; // Needed for the List

/// <summary>
/// This script requires a NavMeshAgent component to be on the same GameObject.
/// It calculates a path to the target but does NOT move the character automatically.
/// It provides the next "checkpoint" for a Rigidbody controller and visualizes the path.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    [Header("Pathfinding")]
    // This is the target your character will move towards.
    public Transform target;

    [Header("Path Visualization")]
    // Prefab to spawn along the path (e.g., a small sphere)
    public GameObject pathMarkerPrefab;
    // Minimum distance between spawned markers
    public float minMarkerDistance = 1.0f;

    [Header("Optimization")]
    [Tooltip("How often to recalculate the path (in seconds). 0 = every frame.")]
    public float pathUpdateInterval = 0.25f;

    [Header("Debug/Output")]
    // The next corner in the path for your Rigidbody to move towards
    public Vector3 nextCheckpoint;
    // Simple boolean to check if a path is available
    public bool hasPath;

    // A private reference to the NavMeshAgent component
    private NavMeshAgent agent;
    // List to keep track of spawned markers for cleanup
    private List<GameObject> spawnedMarkers = new List<GameObject>();

    // Timer to track path update intervals
    private float pathUpdateTimer = 0f;
    // Object pool for path markers
    private Queue<GameObject> markerPool = new Queue<GameObject>();
    public Animator animationcontroller;

    // Use this for initialization
    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();

        // Disable the agent's automatic movement and rotation
        // We will only use it for path calculation.
        // agent.updatePosition = false;
        // agent.updateRotation = false;

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
    }
    void Update()
    {
        // Increment the path update timer
        pathUpdateTimer += Time.deltaTime;

        // Check if we have a valid agent, a target, and the timer is up
        if (agent != null && target != null && pathUpdateTimer >= pathUpdateInterval)
        {
            // Calculate the path to the target's current position
            agent.SetDestination(target.position);
             // Update the visual path markers
            UpdatePathVisualization();
            // Reset the timer
            pathUpdateTimer = 0f;
        }

        // --- This logic runs every frame, which is cheap ---
        // It just reads the *current* path data, it doesn't recalculate it.

        // Update our public "output" variables
        hasPath = agent.hasPath;

        if (hasPath && agent.path.corners.Length > 1)
        {
            // The "next checkpoint" is the second corner in the path array.
            // corners[0] is always the agent's current position.
            nextCheckpoint = agent.path.corners[1];
        }
        else
        {
            nextCheckpoint = agent.pathEndPosition;
        }

    }

    void FixedUpdate()
    {
        // Map the velocity to the animation parameters
        // We dont actuall move sideways, so we only care about forward speed.
        // animationcontroller.SetFloat("x_velocity", velocity.x);
        animationcontroller.SetFloat("z_velocity", agent.velocity.magnitude);
    }

    /// <summary>
    /// Spawns prefabs along the calculated path using object pooling.
    /// Markers are placed in reverse and remain stationary unless the path changes significantly.
    /// </summary>
    void UpdatePathVisualization()
    {
        // Conditions to stop: no prefab, no path, or path is too short
        if (pathMarkerPrefab == null || !hasPath || agent.path.corners.Length < 2 || minMarkerDistance <= 0.01f)
        {
            return;
        }

        // Clear existing markers only if the path has changed significantly
        if (spawnedMarkers.Count == 0 || PathHasChanged())
        {
            foreach (GameObject marker in spawnedMarkers)
            {
                marker.SetActive(false);
                markerPool.Enqueue(marker);
            }
            spawnedMarkers.Clear();

            float distanceAlongPath = 0f;
            float nextMarkerSpawnDistance = minMarkerDistance;

            // Iterate through the path in reverse
            for (int i = agent.path.corners.Length - 1; i > 0; i--)
            {
                Vector3 start = agent.path.corners[i];
                Vector3 end = agent.path.corners[i - 1];
                Vector3 segment = end - start;
                float segmentLength = segment.magnitude;
                Vector3 direction = segment.normalized;

                // While the end of this segment is farther than the next marker's spawn point...
                while (distanceAlongPath + segmentLength >= nextMarkerSpawnDistance)
                {
                    float distanceIntoSegment = nextMarkerSpawnDistance - distanceAlongPath;
                    Vector3 markerPos = start + direction * distanceIntoSegment;

                    // Get a marker from the pool or instantiate a new one if the pool is empty
                    GameObject marker;
                    if (markerPool.Count > 0)
                    {
                        marker = markerPool.Dequeue();
                        marker.transform.position = markerPos;
                        marker.transform.rotation = Quaternion.identity;
                        marker.SetActive(true);
                    }
                    else
                    {
                        marker = Instantiate(pathMarkerPrefab, markerPos, Quaternion.identity);
                    }

                    spawnedMarkers.Add(marker);
                    nextMarkerSpawnDistance += minMarkerDistance;
                }

                distanceAlongPath += segmentLength;
            }
        }
    }

    /// <summary>
    /// Checks if the path has changed significantly (e.g., target moved or path recalculated).
    /// </summary>
    bool PathHasChanged()
    {
        if (spawnedMarkers.Count == 0 || agent.path.corners.Length != spawnedMarkers.Count)
        {
            return true;
        }

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.path.corners[i], spawnedMarkers[i].transform.position) > 0.1f)
            {
                return true;
            }
        }

        return false;
    }
}