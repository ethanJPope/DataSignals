using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform[] stationaryPoints;
    [SerializeField] private float boostMultiplier = 20f;
    [SerializeField] private Vector2 movementSpeed;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private bool touchingEnemy = false;
    private bool touchingWall = false;
    private float gameTime = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        drawPath();
    }

    void Update() {
        gameTime += Time.deltaTime;
        Movement();
    }

    private void Movement() {
        float moveY = Input.GetAxis("Vertical");
        
        Vector2 playerPosition = transform.position;
        Vector2 closestPoint = getClosestPointOnPath(playerPosition);
        Vector2 closestStationaryPoint = getClosestStationaryPoint(playerPosition);

        float distanceToPath = Vector2.Distance(playerPosition, closestPoint);
        float horizontalSpeed = 0f;

        // Vector2 direction = closestStationaryPoint.transform.position - transform.position;
        // direction.Normalize();
        transform.position = Vector2.MoveTowards(this.transform.position, closestStationaryPoint, 1f * Time.deltaTime);

        if (touchingEnemy) {
            horizontalSpeed = 0.25f;
        }else if(touchingWall){
            horizontalSpeed = 0.25f;
        } else {
            horizontalSpeed = boostMultiplier / (distanceToPath + 1);
        }
        rb.linearVelocity = new Vector2(horizontalSpeed * baseSpeed, moveY * 7f);
    }

    private Vector2 getClosestStationaryPoint(Vector2 playerPosition) {
        Vector2 closestPoint = stationaryPoints[0].position;
        float closestDistance = Vector2.Distance(playerPosition, closestPoint);
        for (int i = 1; i < stationaryPoints.Length; i++) {
            Vector2 point = stationaryPoints[i].position;
            float distance = Vector2.Distance(playerPosition, point);
            if (distance < closestDistance) {
                closestPoint = point;
                closestDistance = distance;
            }
        }

        return closestPoint;
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
            float randomY = Random.Range(-4.7f, 4.7f);
            waypoints[i].position = new Vector3(waypoints[i].position.x, randomY, waypoints[i].position.z);
            lineRenderer.SetPosition(i, waypoints[i].position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            touchingEnemy = true;
        } else if (collision.gameObject.CompareTag("Wall")) {
            touchingWall = true;
        } else if(collision.gameObject.CompareTag("Finish")) {
            Debug.Log("You Win!");
            int finalTime = (int)gameTime;
            PlayerPrefs.SetInt("FinalTime", finalTime);
            SceneManager.LoadScene("WinScene");
        } 
    }

    private void OnCollisionExit2D(Collision2D collision) {
        touchingEnemy = false;
        touchingWall = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            touchingEnemy = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        touchingEnemy = false;
    }
}