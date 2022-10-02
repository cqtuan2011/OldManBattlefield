using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Anim transition speed")]
    [SerializeField] private float animBlendSpeed = 8.9f;

    private Rigidbody rb;
    private InputManager inputManager;
    private Animator anim;
    private PhotonView PV;

    private bool hasAnim;
    private bool isGrounded;

    private int xVelocity;
    private int yVelocity;

    private int jumpHash;
    private int groundHash;
    private int fallingHash;

    private const float walkSpeed = 2f;
    private const float runSpeed = 6f;
    private const float jumpForce = 20f;

    private Vector2 currentVelocity;

    //Camera movement
    [Header("Camera")]
    [SerializeField] private Transform headPosition;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 20f;

    [SerializeField] private GameObject objectToHide;

    private float xRotation; // Rotation of camera
    private float yRotation; // Rotation of camera

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hasAnim = TryGetComponent<Animator>(out anim);
        inputManager = GetComponent<InputManager>();
        PV = GetComponent<PhotonView>();

        xVelocity = Animator.StringToHash("xVelocity");
        yVelocity = Animator.StringToHash("yVelocity");
        jumpHash = Animator.StringToHash("isJumping");
        groundHash = Animator.StringToHash("isGrounded");
        fallingHash = Animator.StringToHash("isFalling");
    }

    private void Start()
    {
        HideHeadLocal(PV.IsMine);
        //AlignCameraPosition();
    }

    private void Update()
    {
        //Jump();        
        //HandleJump();
        //SetAnimation();
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

        if (inputManager.Move == Vector2.zero) targetSpeed = 0f;

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

        xRotation -= mouseY * mouseSensitivity * Time.smoothDeltaTime; // using "-=" because it's opposite of our direction
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, mouseX * mouseSensitivity * Time.smoothDeltaTime, 0));
    }

    private void HideHeadLocal(bool isLocal)
    {
        if (isLocal)
        {
            foreach (var obj in objectToHide.GetComponentsInChildren<Transform>())
            {
                int layerObjectToHide = LayerMask.NameToLayer("ObjectToHide");
                obj.gameObject.layer = layerObjectToHide;
            }
        }
    }

    private void HandleJump()
    {
        if (!hasAnim) return;

        if (!inputManager.Jump && !isGrounded) return;

        rb.AddForce(-rb.velocity.y * Vector3.up, ForceMode.VelocityChange);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void SetGroundedState(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }
}
