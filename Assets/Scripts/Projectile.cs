using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
