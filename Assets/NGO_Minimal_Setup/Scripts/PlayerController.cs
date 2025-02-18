using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public AudioSource footstepSound;
  
    private float originalHeight;
    public float crouchHeight = 0.5f;
    private float normalFOV;
    private float zoomedFOV = 30f; 
    private bool isZoomed = false;

    public bool canMove = true;

    CharacterController characterController;
    public Vector3 moveDirection;
    private bool isCrouching = false;
    Vector2 rotation = Vector2.zero;

    

    void Start()
    {
        originalHeight = GetComponent<CharacterController>().height;
        characterController = GetComponent<CharacterController>();  
        Cursor.lockState = CursorLockMode.Locked;
        rotation.y = transform.eulerAngles.y;
       
        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera is not assigned.");
        }
        normalFOV = playerCamera.fieldOfView;
    }
        void Update()
   {
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                isCrouching = !isCrouching;

                if (isCrouching)
                {
                    GetComponent<CharacterController>().height = crouchHeight;
                    speed = 6f;
                }
                else
                {
                    GetComponent<CharacterController>().height = originalHeight;
                    speed = 7.5f;
                }
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }

        PlayFootstepSound();
   


    }



    
    void PlayFootstepSound()
        {
            if (characterController.isGrounded && characterController.velocity.magnitude > 0 && !footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
        }
    
}