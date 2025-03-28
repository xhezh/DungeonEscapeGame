using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int health = 100;
    private GameUIManager uiManager;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    public int gold = 100;
    public bool isGrounded;
    public EnemySpawner spawner;
    public GameOverManager gameOverManager;

    private Animator animator;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject regularTowerPrefab;
    public GameObject slowTowerPrefab;
    public GameObject bombTowerPrefab;
    public LayerMask platformLayer;

    public int regularTowerCost = 5;
    public int slowTowerCost = 3;
    public int bombTowerCost = 8;

    public float fallThreshold = -20f;

    void Start() {
        if (gameOverManager == null) {
            Debug.LogError("GameOverManager not found in the scene!");
        }
        rb = GetComponent<Rigidbody2D>();
        uiManager = FindObjectOfType<GameUIManager>();
        animator = GetComponent<Animator>();

        if (uiManager != null) {
            uiManager.UpdateGold(gold);
            uiManager.UpdateHealth(health);
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (uiManager != null) {
            uiManager.UpdateHealth(health);
        }
        if (health <= 0) {
            Debug.Log("Player died!");
            if (gameOverManager != null) {
                gameOverManager.ShowGameOver();
            }
        }
        else {
            animator.SetTrigger("isHurt");
        }
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer | enemyLayer);

        if (transform.position.y < fallThreshold) {
            if (gameOverManager != null) {
                gameOverManager.ShowGameOver();
            }
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Mathf.Abs(moveInput) > 0) {
            animator.SetBool("isRunning", true);
        }
        else {
            animator.SetBool("isRunning", false);
        }

        if (moveInput > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        if (isGrounded && rb.velocity.y <= 0) {
            animator.SetBool("isJumping", false);
        }

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                BuildTower(regularTowerPrefab, regularTowerCost);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                BuildTower(slowTowerPrefab, slowTowerCost);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                BuildTower(bombTowerPrefab, bombTowerCost);
            }
        }
    }

    void BuildTower(GameObject towerPrefab, int cost) {
        if (gold >= cost) {
            Vector2 buildPosition = new Vector2(transform.position.x, transform.position.y - 1.8f);
            Instantiate(towerPrefab, buildPosition, Quaternion.identity);
            gold -= cost;

            if (uiManager != null) {
                uiManager.UpdateGold(gold);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.8f);
    }

    public int GetGold() {
        return gold;
    }

    public void SpendGold(int amount) {
        if (gold >= amount) {
            gold -= amount;
            Debug.Log("Gold spent: " + amount + ". Gold left: " + gold);
            if (uiManager != null) {
                uiManager.UpdateGold(gold);
            }
        }
        else {
            Debug.LogWarning("Not enough gold!");
        }
    }

    public void AddGold(int amount) {
        gold += amount;
        Debug.Log("Gold added: " + amount + ". Total gold: " + gold);
        if (uiManager != null) {
            uiManager.UpdateGold(gold);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Gold")) {
            gold++;
            if (uiManager != null) {
                uiManager.UpdateGold(gold);
            }
            Destroy(collision.gameObject);
        }
    }
}
