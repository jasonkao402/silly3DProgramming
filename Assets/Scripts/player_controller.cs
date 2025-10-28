using UnityEngine;
using UnityEngine.InputSystem;
public class player_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public targetSetter cam;
    Rigidbody rb;
    Vector2 actualMovement, targetMovement;
    // base speed, sprint boost, smoothing factor
    public Vector3 walkParam = new Vector3(4f, 2.0f, 0.2f);
    public Animator animationcontroller;
    bool isSprinting = false, IsClicking = false;
    // float velocity = 0f;
    public void OnMovement(InputAction.CallbackContext context)
    {
        targetMovement = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        IsClicking = context.ReadValueAsButton();
        animationcontroller.SetBool("isAtk", IsClicking);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null)
        {
            cam = FindFirstObjectByType<targetSetter>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle movement input
        transform.rotation = Quaternion.Euler(0, cam.rotationX, 0);
        // velocity = Mathf.Lerp(velocity, isSprinting ? walkParam.y : walkParam.x, walkParam.z);
        actualMovement = Vector2.Lerp(actualMovement, targetMovement * (isSprinting ? walkParam.y : 1f), walkParam.z);
        animationcontroller.SetFloat("x_velocity", actualMovement.x);
        animationcontroller.SetFloat("z_velocity", actualMovement.y);
        if (actualMovement.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = transform.rotation * new Vector3(actualMovement.x, 0, actualMovement.y);
            // transform.position += walkParam.x * moveDirection * Time.fixedDeltaTime;
            rb.AddForce(moveDirection * walkParam.x, ForceMode.VelocityChange);
        }
        else
        {
            rb.linearVelocity *= 0.95f;
        }
    }
}
