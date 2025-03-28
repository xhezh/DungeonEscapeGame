using UnityEngine;

public class TowerProjectile : MonoBehaviour {
    public int damage = 5;
    public float speed = 10f;
    public float lifetime = 5f;

    private Vector2 direction;

    public void Initialize(Vector2 dir, int dmg, float spd) {
        direction = dir.normalized;
        damage = dmg;
        speed = spd;
        Destroy(gameObject, lifetime);
    }

    void Update() {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
