using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float boostMultiplier = 20f;
    [SerializeField] private Vector2 movementSpeed;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        drawPath();
    }

    void Update() {
        Movement();
        
    }

    private void Movement()
    {
        float moveY = Input.GetAxis("Vertical");

        Vector2 playerPosition = transform.position;
        Vector2 closestPoint = getClosestPointOnPath(playerPosition);

        float distanceToPath = Vector2.Distance(playerPosition, closestPoint);
        Debug.Log("Distance to Path: " + distanceToPath+1);

        float horizontalSpeed = boostMultiplier / (distanceToPath + 1);
        Debug.Log("Horizontal Speed: " + horizontalSpeed);

        rb.linearVelocity = new Vector2(horizontalSpeed * baseSpeed, moveY * baseSpeed);
    }

    private Vector2 getClosestPointOnPath(Vector2 playerPosition) {
        Vector2 closestPoint = waypoints[0].position;
        float closestDistance = Vector2.Distance(playerPosition, closestPoint);

        for (int i = 0; i < waypoints.Length - 1; i++) {
            Vector2 segmentStart = waypoints[i].position;
            Vector2 segmentEnd = waypoints[i + 1].position;
            Vector2 closestPointOnSegment = getClosestPathOnSegment(playerPosition, segmentStart, segmentEnd);
            float distance = Vector2.Distance(playerPosition, closestPointOnSegment);
            if (distance < closestDistance) {
                closestPoint = closestPointOnSegment;
                closestDistance = distance;
            }
        }

        return closestPoint;
    }

    private Vector2 getClosestPathOnSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd) {
        Vector2 segment = segmentEnd - segmentStart;
        Vector2 pointVector = point - segmentStart;

        float pointLength = Vector2.Dot(pointVector, segment.normalized);
        float segmentLength = segment.magnitude;
        float clampedPointLength = Mathf.Clamp(pointLength, 0, segmentLength);

        Vector2 closestPoint = segmentStart + segment.normalized * clampedPointLength;
        return closestPoint;
    }

    private void drawPath() {
        lineRenderer.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++) {
            lineRenderer.SetPosition(i, waypoints[i].position);
        }
    }
}