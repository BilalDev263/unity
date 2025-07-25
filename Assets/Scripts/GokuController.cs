using UnityEngine;
using UnityEngine.InputSystem;

public class GokuController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    
    [Header("Combat Settings")]
    public float kamehamehaRange = 20f;
    public int kamehamehaDamage = 50;
    
    [Header("Kamehameha 3D (Optionnel)")]
    public GameObject kamehameha3DPrefab; // Ton prefab Kamehameha
    public Transform kamehamehaSpawnPoint;
    public float kamehamehaSpeed = 20f;
    
    [Header("References - Optionnels")]
    public Animator animator;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    
    // Input Actions
    private PlayerInputActions inputActions;
    private Vector2 movementInput;
    
    private Rigidbody rb;
    private bool isGrounded;
    private bool canUseKamehameha = true;
    private Level2Manager gameManager;
    
    void Awake()
    {
        // Initialiser les Input Actions
        inputActions = new PlayerInputActions();
    }
    
    void OnEnable()
    {
        // Activer les inputs et s'abonner aux événements
        inputActions.Enable();
        
        // S'abonner aux actions
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Kamehameha.performed += OnKamehamehaPerformed;
    }
    
    void OnDisable()
    {
        // Se désabonner et désactiver
        if (inputActions != null)
        {
            inputActions.Player.Jump.performed -= OnJumpPerformed;
            inputActions.Player.Kamehameha.performed -= OnKamehamehaPerformed;
            inputActions.Disable();
        }
    }
    
    void Start()
    {
        // Récupérer les composants
        rb = GetComponent<Rigidbody>();
        
        // Si pas de Rigidbody, en ajouter un
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }
        
        // Trouver le game manager
        gameManager = FindObjectOfType<Level2Manager>();
        if (gameManager == null)
        {
            Debug.Log("Pas de Level2Manager trouvé - Mode libre");
        }
        
        // Animator optionnel
        if (animator == null)
            animator = GetComponent<Animator>();
            
        // Vérifier le spawn point
        if (kamehamehaSpawnPoint == null)
        {
            Debug.LogWarning("⚠️ KamehamehaSpawnPoint non assigné ! Utilisation de la position du joueur.");
        }
            
        Debug.Log("🥋 GokuController initialisé avec New Input System !");
    }
    
    void Update()
    {
        // Lire les inputs de mouvement en continu
        movementInput = inputActions.Player.Move.ReadValue<Vector2>();
        
        CheckGrounded();
        UpdateAnimations();
        
        // Debug retiré pour éviter les conflits Input System
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        // Convertir input 2D en mouvement 3D
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        
        // Appliquer le mouvement
        if (rb)
        {
            Vector3 velocity = movement * moveSpeed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
            
            // Rotation vers la direction du mouvement
            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
    
    // Callback pour le saut
    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("🚀 Input Jump détecté !");
        if (isGrounded)
        {
            Jump();
        }
    }
    
    // Callback pour le Kamehameha
    void OnKamehamehaPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("🔥 Input Kamehameha détecté !");
        if (canUseKamehameha)
        {
            FireKamehameha();
        }
    }
    
    void Jump()
    {
        if (rb)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            Debug.Log("🚀 Goku saute !");
        }
        
        if (animator && animator.runtimeAnimatorController != null)
            animator.SetTrigger("Jump");
    }
    
    void FireKamehameha()
    {
        Debug.Log("🔥 KAMEHAMEHA ACTIVÉ ! 🔥");
        
        // Si on a un prefab 3D, l'utiliser
        if (kamehameha3DPrefab != null)
        {
            FireKamehameha3D();
        }
        else
        {
            // Sinon, utiliser le raycast simple
            FireKamehamehaRaycast();
        }
        
        // Animation
        if (animator && animator.runtimeAnimatorController != null)
            animator.SetTrigger("Kamehameha");
        
        // Cooldown
        canUseKamehameha = false;
        Invoke("ResetKamehamehaCooldown", 2f);
    }
    
    void FireKamehameha3D()
    {
        Debug.Log("💥 Tir de Kamehameha 3D !");
        
        Vector3 spawnPos = kamehamehaSpawnPoint ? kamehamehaSpawnPoint.position : transform.position;
        Quaternion spawnRot = transform.rotation;
        
        GameObject kamehamehaInstance = Instantiate(kamehameha3DPrefab, spawnPos, spawnRot);
        
        // Ajouter vitesse au projectile
        Rigidbody kamehamehaRb = kamehamehaInstance.GetComponent<Rigidbody>();
        if (kamehamehaRb)
        {
            kamehamehaRb.linearVelocity = transform.forward * kamehamehaSpeed;
        }
        
        // Auto-destruction après 5 secondes
        Destroy(kamehamehaInstance, 5f);
    }
    
    void FireKamehamehaRaycast()
    {
        Debug.Log("⚡ Kamehameha Raycast !");
        
        // Effet visuel
        Debug.DrawRay(transform.position, transform.forward * kamehamehaRange, Color.cyan, 1f);
        
        // Chercher des ennemis
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, kamehamehaRange);
        
        bool hitSomething = false;
        
        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Kamehameha touche : " + hit.collider.name);
            
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy)
                {
                    enemy.TakeDamage(kamehamehaDamage);
                    Debug.Log("💥 Ennemi touché ! Dégâts: " + kamehamehaDamage);
                    hitSomething = true;
                }
            }
        }
        
        if (!hitSomething)
        {
            Debug.Log("Kamehameha n'a touché aucun ennemi");
        }
    }
    
    void ResetKamehamehaCooldown()
    {
        canUseKamehameha = true;
        Debug.Log("⚡ Kamehameha rechargé !");
    }
    
    void CheckGrounded()
    {
        if (groundCheck)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius);
        }
        else
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        }
    }
    
    void UpdateAnimations()
    {
        if (animator && animator.runtimeAnimatorController != null)
        {
            float speed = rb ? rb.linearVelocity.magnitude : 0f;
            animator.SetFloat("Speed", speed);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("💔 Goku prend " + damage + " dégâts !");
        
        if (gameManager)
        {
            gameManager.TakeDamage(damage);
        }
    }
    
    public void Heal(int healAmount)
    {
        Debug.Log("💚 Goku se soigne de " + healAmount + " PV !");
        
        if (gameManager)
        {
            gameManager.HealPlayer(healAmount);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Portée du Kamehameha
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * kamehamehaRange);
        
        // Ground check
        if (groundCheck)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        // Spawn point
        if (kamehamehaSpawnPoint)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(kamehamehaSpawnPoint.position, 0.5f);
        }
    }
}