using UnityEngine;

public class BombTowerController : MonoBehaviour {
    public int damage = 50;
    public float explosionRadius = 3f;
    public LayerMask enemyLayer;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            Explode();
        }
    }

    void Explode() {
        if (animator != null) {
            animator.SetTrigger("Explode");
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D enemy in enemies) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null) {
                enemyController.TakeDamage(damage); 
            }
        }

        Destroy(gameObject, 0.5f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
