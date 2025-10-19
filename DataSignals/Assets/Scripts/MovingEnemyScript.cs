using UnityEngine;

public class MovingEnemyScript : MonoBehaviour
{
    [SerializeField] private float speed = -3f;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.linearVelocity = new Vector2(speed, 0f);
    }
}
