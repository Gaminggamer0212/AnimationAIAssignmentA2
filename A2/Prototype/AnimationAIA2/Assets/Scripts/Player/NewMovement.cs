using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NewMovement : MonoBehaviour
{ 
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]private bool groundedPlayer;
    private Transform cam;
    private Vector2 inputVector;
    private Vector3 moveDirection;
    private Animator animator;
    public static bool controllable = true;
    
    public float rotationSpeed = 20f;
    public bool translationDisabled = false;
    
    [Header("Jump")]
    public float minJumpHeight = 0.5f;
    public float maxJumpHeight = 1.5f;
    public float timeToMaxHeight = 0.5f;
    
    private float baseGravity;
    private float initialJumpVelocity;
    private float minJumpCutVelocity;
    

    [Header("Input Actions")]
    InputAction moveAction;
    InputAction jumpAction;
    private InputAction lookAction;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (InputSystem.actions)
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            lookAction = InputSystem.actions.FindAction("Look");
        }
        
        //Like wtf physics is this, just found it online
        baseGravity = -(2f * maxJumpHeight) / (timeToMaxHeight * timeToMaxHeight);
        initialJumpVelocity = (2f * maxJumpHeight) / timeToMaxHeight;
        
        minJumpCutVelocity = Mathf.Sqrt(2f * Mathf.Abs(baseGravity) * minJumpHeight);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        lookAction.Enable();
        
        
        if (jumpAction != null)
        {
            jumpAction.performed += OnJumpPerformed;
            jumpAction.canceled  += OnJumpCanceled;
        }
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        lookAction.Disable();
        
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJumpPerformed;
            jumpAction.canceled  -= OnJumpCanceled;
        }
    }

    public void EnableMovement()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }
    
    public void DisableMovement()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (controllable){
            Jump();
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if (controllable){
            if (playerVelocity.y > 0f)
            {
                playerVelocity.y = Mathf.Min(playerVelocity.y, minJumpCutVelocity);
            }
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        
        /*if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = gravityValue;
        }*/

        if (controllable){
            moveDirection = translationDisabled ? Vector3.zero : GetInputVector();
        
            HandleRotation(moveDirection);
            
            animator.SetBool("Moving", moveDirection.magnitude > 0.01f & groundedPlayer);

            // Apply gravity
            if (groundedPlayer && playerVelocity.y <= 0f)
            {
                playerVelocity.y = -2f;
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            
            // Combine horizontal and vertical movement
            Vector3 finalMove = (moveDirection * playerSpeed) + (playerVelocity.y * Vector3.up);
            controller.Move(finalMove * Time.deltaTime);
        }
        
        animator.SetBool("Jump", !groundedPlayer);
    }

    private void Jump()
    {
        if (!groundedPlayer) return;
        
        if (playerVelocity.y < 0f) playerVelocity.y = 0f;
        playerVelocity.y = initialJumpVelocity;
    }
    
    private void HandleRotation(Vector3 moveDir){
        if (moveDir.sqrMagnitude < 0.0001f) return;
        
        moveDir.y = 0f;
        moveDir.Normalize();

        Quaternion target = Quaternion.LookRotation(moveDir);
        
        float t = 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
    }
    
    public Vector3 GetInputVector()
    {
        inputVector = moveAction.ReadValue<Vector2>();

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 move = (inputVector.x * camRight + inputVector.y * camForward).normalized;

        return move;
    }
    
    public void SetTranslationDisabled(bool disabled)
    {
        translationDisabled = disabled;
    }

    public void SetGravity(bool disabled)
    {
        if  (!groundedPlayer) return;

        if (disabled)
        {
            
        }
    }
}
