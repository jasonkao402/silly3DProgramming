using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance { get; private set; }

    [SerializeField] private GameObject damageTextPrefab;
    // [SerializeField] private Canvas worldCanvas;  // UI canvas for popup text
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowDamage(int value, Vector3 worldPos)
    {
        // Spawn from your pool
        var obj = PoolManager.Instance.Spawn(damageTextPrefab, worldPos, Quaternion.identity);
        var popup = obj.GetComponent<DamagePopup>();
        
        popup.Setup(value, worldPos);
    }
    public void ShowText(string text, Vector3 worldPos, Color color)
    {
        // Spawn from your pool
        var obj = PoolManager.Instance.Spawn(damageTextPrefab, worldPos, Quaternion.identity);
        var popup = obj.GetComponent<DamagePopup>();

        popup.SetupText(text, worldPos, color);
    }
}
