using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
    public float smoothing = 5f;
    public Vector3 offset;

    void Start() {
        offset = transform.position - player.position;
        offset.x = 0;
    }

    void FixedUpdate() {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
