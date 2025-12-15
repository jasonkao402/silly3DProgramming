using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshPro))]
public class DamagePopup : PoolObjectBase, IPoolable
{
    
    public float lifetime = 1f;
    public float offsetY = 1f;
    public Vector2 velocityParam = new(1f, 3f);
    TextMeshPro tmpComp;
    Vector3 fakeVelocity;
    float timer;
    Camera mainCam;
    public void Setup(int damage, Vector3 worldPos)
    {
        tmpComp.text = damage.ToString();
        tmpComp.color = Color.white;
        
        transform.position = worldPos + offsetY * Vector3.up;
        timer = lifetime;
        
        fakeVelocity = Random.insideUnitCircle * velocityParam.x;
        fakeVelocity = new Vector3(fakeVelocity.x, velocityParam.y, fakeVelocity.y);
    }

    public void SetupText(string text, Vector3 worldPos)
    {
        SetupText(text, worldPos, Color.white);
    }

    public void SetupText(string text, Vector3 worldPos, Color color)
    {
        tmpComp.text = text;
        tmpComp.color = color;
        transform.position = worldPos + offsetY * Vector3.up;
        timer = lifetime;
        
        fakeVelocity = Random.insideUnitCircle * velocityParam.x;
        fakeVelocity = new Vector3(fakeVelocity.x, velocityParam.y, fakeVelocity.y);
    }

    public override void OnSpawned()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        if (tmpComp == null)
        {
            tmpComp = GetComponent<TextMeshPro>();
        }
    }

    public override void OnDespawned()
    {
        // reset if needed
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PoolManager.Instance.Despawn(prefabRef, gameObject);
        }

        // 移動與淡出動畫
        transform.position += fakeVelocity * Time.deltaTime;
        fakeVelocity += Physics.gravity * Time.deltaTime;
        var col = tmpComp.color;
        col.a = timer / lifetime;
        tmpComp.color = col;
    }
    void LateUpdate()
    {
        // 讓文字「只旋轉 Y 軸朝向攝影機」：常用於 RPG
        // Vector3 dir = transform.position - mainCam.transform.position;
        // dir.y = 0f; 
        // transform.rotation = Quaternion.LookRotation(dir);

        // 若想完全朝向相機（2D 的文字），改用以下：
        transform.rotation = mainCam.transform.rotation;
    }
}
