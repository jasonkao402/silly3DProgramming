using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui_gravity : MonoBehaviour
{
    public float gravityStrength = Physics.gravity.y;
    public Dropdown gravityDropdown;
    public void OnGravityDropdownChanged(int index)
    {
        float targetGravity;
        switch (index)
        {
            case 0:
                targetGravity = gravityStrength;
                break;
            case 1:
                targetGravity = 0.2f * gravityStrength;
                break;
            case 2:
                targetGravity = 5f * gravityStrength;
                break;
            case 3: // Reverse
                targetGravity = -1f * gravityStrength;
                break;
            default:
                targetGravity = gravityStrength;
                break;
        }
        Physics.gravity = new Vector3(0, targetGravity, 0);
    }
    void Start()
    {
        // gravityDropdown.onValueChanged.AddListener(OnGravityDropdownChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
