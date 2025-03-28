using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float moveSpeed = 2f;
    public int health = 10;
    public int damage = 10;
    public float attackInterval = 1f;
    private float attackCooldown = 0f;

    public GameObject resourcePrefab;
    private Transform player;
    public LayerMask groundLayer;
    public EnemySpawner spawner;
    private GameUIManager uiManager;
    private PlayerController playerController;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector3 initialScale;
    private bool isDead = false;

    private Animator animator;
    private bool isAttacking = false;

    public float jumpForce = 5f;
    public float jumpCheckOffset = 1.0f;
    private float originalSpeed;
    private int jumpCount = 0;

    void Start() {
        initialScale = transform.localScale;
        uiManager = FindObjectOfType<GameUIManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();
        }
    }

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpCount = 0;
        TryJump();
    }

    void Update() {
        if (Mathf.Abs(rb.velocity.x) > 0) {
            animator.SetBool("isWalking", true);
        }
        else {
            animator.SetBool("isWalking", false);
        }

        if (isAttacking && attackCooldown <= 0f) {
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        FollowPlayer();
        attackCooldown -= Time.deltaTime;
    }

    void FollowPlayer() {
        if (player == null) return;

        float horizontalDifference = player.position.x - transform.position.x;
        float direction = Mathf.Sign(horizontalDifference);

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        if (!isGrounded) return;

        if (direction > 0) {
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else if (direction < 0) {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }

    void TryJump() {
        if (player == null) return;
        if (playerController != null && !playerController.isGrounded) return;
        if (jumpCount == 2) return;
        jumpCount++;

        if (player.position.y > transform.position.y + jumpCheckOffset) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void TakeDamage(int damage) {
        if (isDead) return;
        health -= damage;

        if (health <= 0) {
            isDead = true;
            DropResource();
            if (spawner != null) {
                spawner.OnEnemyDestroyed();
            }
            Destroy(gameObject);
        }
    }

    void DropResource() {
        if (resourcePrefab != null) {
            Instantiate(resourcePrefab, transform.position, Quaternion.identity);
        }
    }

    public void SlowDown(float factor, float duration) {
        if (originalSpeed == 0f) {
            originalSpeed = moveSpeed;
        }

        moveSpeed = originalSpeed * factor;
        CancelInvoke(nameof(RestoreSpeed));
        Invoke(nameof(RestoreSpeed), duration);
    }

    void RestoreSpeed() {
        moveSpeed = originalSpeed;
    }

    public void ApplyDamage() {
        if (player != null) {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.TakeDamage(damage);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (attackCooldown <= 0f && !isAttacking) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null) {
                    attackCooldown = attackInterval;
                    isAttacking = true;
                    animator.SetBool("isAttacking", true);
                }
            }
        }
    }
}
