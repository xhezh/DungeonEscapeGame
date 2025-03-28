using UnityEngine;
using System.Collections.Generic;

public class SlowTowerController : MonoBehaviour {
    public float attackRange = 3f;
    public float attackRate = 1f;
    public float slowEffect = 0.2f;
    public float slowDuration = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

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
            Vector2 direction = (enemy.transform.position - transform.position - new Vector3(0, 2f, 0)).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
            SlowTowerProjectile stp = projectile.GetComponent<SlowTowerProjectile>();
            if (stp != null) {
                stp.Initialize(direction, slowEffect, slowDuration, projectileSpeed);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
