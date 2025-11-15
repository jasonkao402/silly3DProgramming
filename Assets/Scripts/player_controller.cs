using UnityEngine;
using UnityEngine.InputSystem;
public class player_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public targetSetter cam;
    Rigidbody rb;
    Vector2 actualMovement, targetMovement;
    public float rotationAlpha = 0.1f;
    public float jumpVelocity = 5f;
    public float jumpCD = 0.5f;
    float jumpCDTimer = 0f;
    // base speed, sprint boost, smoothing factor
    public Vector3 walkParam = new Vector3(4f, 2.0f, 0.2f);
    public Animator animationcontroller;
    bool isSprinting = false, IsClicking = false;
    int groundDet = 0;
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
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpCDTimer < 0f && groundDet > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
            animationcontroller.SetFloat("jumpCD", 1f);
            jumpCDTimer = jumpCD;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null)
        {
            cam = Camera.main.GetComponent<targetSetter>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle movement input
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cam.rotationX, 0), rotationAlpha);
        actualMovement = Vector2.Lerp(actualMovement, targetMovement * (isSprinting ? walkParam.y : 1f), walkParam.z);
        animationcontroller.SetFloat("x_velocity", actualMovement.x);
        animationcontroller.SetFloat("z_velocity", actualMovement.y);
        if (actualMovement.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = transform.rotation * new Vector3(actualMovement.x, 0, actualMovement.y);
            // transform.position += walkParam.x * moveDirection * Time.fixedDeltaTime;
            rb.AddForce(moveDirection * walkParam.x, ForceMode.VelocityChange);
            rb.angularVelocity = Vector3.zero;
        }
        // else
        // {
        //     rb.linearVelocity *= 0.95f;
        // }
        if (jumpCDTimer >= 0f)
        {
            jumpCDTimer -= Time.fixedDeltaTime;
            if (jumpCDTimer < 0f)
            {
                animationcontroller.SetFloat("jumpCD", 0f);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            animationcontroller.SetFloat("jumpCD", 0f);
            groundDet++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            groundDet--;
        }
    }
}
