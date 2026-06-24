using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCharacterController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    [SerializeField] private InputActionAsset input;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private string mapName = "Player";
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private float moveSpeed = 5f;
    private float sprintMultiplier = 2f;
    private float rotationSpeed = 100f;
    private float jumpHeight = 2f;
    private float gravity = -20f;
    private float verticalVelocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        InputActionMap map = input.FindActionMap(mapName);
        moveAction  = map.FindAction("Move");
        jumpAction  = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        float speed = movementInput.y * moveSpeed;
        if (sprintAction.IsPressed()) speed *= sprintMultiplier; //sprinten

        Vector3 move = transform.forward * speed * Time.deltaTime; //De move variabele gebruiken we later in de Move() functie
        transform.Rotate(Vector3.up * movementInput.x * rotationSpeed * Time.deltaTime);
        Debug.Log(characterController.isGrounded);
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f; // kleine downward force om grounded te blijven

            if (jumpAction.WasPressedThisFrame())
            {
                verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight); //hoeveel kracht moet je geven om op de juiste hoogte uit te komen?
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // zwaartekracht trekt mij naar beneden
        }

        move.y = verticalVelocity * Time.deltaTime; //verticale berekening word meegegeven aan de move variabele
        characterController.Move(move); //hier geven we de uiteindelijke move variabele mee aan de Move functie
        animator.SetFloat("Speed", movementInput.y);
    }
}
