using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animBlendSpeed = 8.9f;

    private Rigidbody rb;
    private InputManager inputManager;
    private Animator anim;

    private bool hasAnim;

    private int xVelocity;
    private int yVelocity;

    private const float walkSpeed = 2f;
    private const float runSpeed = 6f;

    private Vector2 currentVelocity;

    //Camera movement
    [Header("Camera")]
    [Space(10)]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 20f;

    private float xRotation; // Rotation of camera
    private float yRotation; // Rotation of camera

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hasAnim = TryGetComponent<Animator>(out anim);
        inputManager = GetComponent<InputManager>();

        xVelocity = Animator.StringToHash("xVelocity");
        yVelocity = Animator.StringToHash("yVelocity");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void Move()
    {
        if (!hasAnim) return;

        float targetSpeed = inputManager.Run ? runSpeed : walkSpeed; // if we press shift -> run 

        if (inputManager.Move == Vector2.zero) targetSpeed = 0.01f;

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.Move.x * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.Move.y * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);

        var xVelDifference = currentVelocity.x - rb.velocity.x;
        var zVelDifference = currentVelocity.y - rb.velocity.z;

        rb.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

        anim.SetFloat(xVelocity, currentVelocity.x);
        anim.SetFloat(yVelocity, currentVelocity.y);
    }

    private void CameraMovement()
    {
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;

        xRotation -= mouseY * mouseSensitivity * Time.deltaTime; // using "-=" because it's opposite of our direction
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up, mouseX * mouseSensitivity * Time.deltaTime);
    }
}
