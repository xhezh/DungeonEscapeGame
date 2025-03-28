using UnityEngine;
using System.Collections.Generic;

public class TowerControllerProjectile : MonoBehaviour {
    public float attackRate = 1f;
    public int damage = 10;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;
    public float attackRange = 3f;

    private float nextAttackTime = 0f;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    void Update() {
        if (enemiesInRange.Count > 0 && Time.time >= nextAttackTime) {
            Attack(enemiesInRange[0]);
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack(GameObject enemy) {
        if (enemy != null && projectilePrefab != null) {
            Vector2 direction = (enemy.transform.position - transform.position - new Vector3(0, 3f, 0)).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + new Vector3(0, 3f, 0), Quaternion.identity);
            TowerProjectile tp = projectile.GetComponent<TowerProjectile>();
            if (tp != null) {
                tp.Initialize(direction, damage, projectileSpeed);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy") && !enemiesInRange.Contains(collision.gameObject)) {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            enemiesInRange.Remove(collision.gameObject);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
