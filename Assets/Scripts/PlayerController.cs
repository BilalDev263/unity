using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 5f;
    
    [Header("Animations")]
    public Animator animator;
    
    [Header("Input")]
    public PlayerInputActions inputActions;

    [Header("Respawn")]
    public Vector3 respawnPosition = new Vector3(385.76f, 1f, -45.14f); // Position de respawn
    public float fallThreshold = -10f; // Seuil de chute

    [Header("Audio")]
    public AudioClip respawnSound;
    private AudioSource audioSource;

    private Rigidbody rb;
    private Vector3 movement;
    private bool isMoving = false;
    private Vector2 moveInput;

    void Awake()
    {
        // Initialiser les Input Actions
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Activer les inputs et s'abonner aux événements
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Enable();
    }

    void OnDisable()
    {
        // Désactiver les inputs
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Initialiser l'audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Calculer le mouvement basé sur l'input
        movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        
        // Vérifier si le joueur bouge
        isMoving = movement.magnitude > 0;
        
        // Rotation du joueur vers la direction de mouvement
        if (isMoving)
        {
            transform.LookAt(transform.position + movement);
        }
        
        // Animation
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }

        // Respawn si le joueur tombe
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void FixedUpdate()
    {
        // Appliquer le mouvement
        Vector3 moveVelocity = movement * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    // Callbacks pour les événements d'input
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = respawnPosition;

        if (respawnSound != null)
            audioSource.PlayOneShot(respawnSound);
    }
}
