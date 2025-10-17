using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private Transform[] waypoints;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Movement();
        
    }

    private void Movement() {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        rb.linearVelocity = movement * baseSpeed;
    }
}
