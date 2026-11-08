using UnityEngine;
using UnityEngine.InputSystem;

public class AquariumWalk : MonoBehaviour
{
    [SerializeField] float walkSpeed = 2.6f;
    [SerializeField] float lookSensitivity = 0.10f;
    CharacterController body;
    float yaw, pitch;

    void Awake()
    {
        body = gameObject.AddComponent<CharacterController>();
        body.height = 1.8f;
        body.radius = 0.28f;
        body.center = new Vector3(0, -0.7f, 0);
        body.stepOffset = 0.18f;
        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.Locked;
        if (keyboard.escapeKey.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.None;
        if (mouse != null && Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 delta = mouse.delta.ReadValue() * lookSensitivity;
            yaw += delta.x;
            pitch = Mathf.Clamp(pitch - delta.y, -80f, 80f);
        }
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        float x = (keyboard.dKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed ? 1 : 0);
        float z = (keyboard.wKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed ? 1 : 0);
        var direction = Quaternion.Euler(0, yaw, 0) * Vector3.ClampMagnitude(new Vector3(x, 0, z), 1);
        body.SimpleMove(direction * walkSpeed);
    }

    void OnDisable() => Cursor.lockState = CursorLockMode.None;
}
