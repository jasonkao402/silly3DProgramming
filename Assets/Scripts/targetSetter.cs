using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class targetSetter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera cam;
    // public player_controller playerController;
    public Transform focusedTarget;
    public Transform targetPointer;
    public float zoomSpeed = 0.1f;
    public float lerpSpeed = 0.1f;
    public float sensitivity = 0.1f;
    public LayerMask layerMask;
    [HideInInspector] 
    public float rotationX, rotationY;
    Quaternion originalRotation;
    public Vector2 mousePosition, mouseDelta, scrollInput;
    bool isDragging = false;
    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
    public void OnDragActive(InputAction.CallbackContext context)
    {
        isDragging = context.ReadValueAsButton();
    }
    public void OnScroll(InputAction.CallbackContext context)
    {
        scrollInput = context.ReadValue<Vector2>();
    }
    public void SetNewDestination(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Ray ray = cam.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, cam.farClipPlane, layerMask))
                targetPointer.position = hit.point;
        }
    }
    public void SetFocusedTarget(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Ray ray = cam.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, cam.farClipPlane, layerMask))
                focusedTarget = hit.transform;
        }
    }
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        rotationX = transform.rotation.eulerAngles.y;
        rotationY = transform.rotation.eulerAngles.x;
        originalRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView *= 1.0f - zoomSpeed * scrollInput.y;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 15.0f, 90.0f);
        if (isDragging)
        {
            rotationX += mouseDelta.x * sensitivity;
            rotationY += -mouseDelta.y * sensitivity;
            rotationX = ClampAngle(rotationX, -360, 360);
            rotationY = ClampAngle(rotationY, -80, 80);
        }
        transform.rotation = originalRotation * Quaternion.Euler(rotationY, rotationX, 0);
    }
    void FixedUpdate()
    {
        if (focusedTarget)
        {
            transform.position = Vector3.Lerp(transform.position, focusedTarget.position, lerpSpeed);
        }
    }
    public static float ClampAngle (float angle, float min, float max)
    {
        return Mathf.Clamp (Mathf.Repeat(angle+180f, 360f)-180f, min, max);
	}
}
