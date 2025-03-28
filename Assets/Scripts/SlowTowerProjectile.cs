using UnityEngine;

public class SlowTowerProjectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 5f;

    private float slowEffect;
    private float slowDuration;
    private Vector2 direction;

    public void Initialize(Vector2 dir, float effect, float duration, float spd) {
        direction = dir.normalized;
        slowEffect = effect;
        slowDuration = duration;
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
                enemy.SlowDown(slowEffect, slowDuration);
            }
            Destroy(gameObject);
        }
    }
}
