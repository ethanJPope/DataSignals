using UnityEngine;

public class LockCameraY : MonoBehaviour
{
    public Transform target;
    public float fixedY = 0f;
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = target.position;
            newPosition.y = fixedY;
            transform.position = newPosition;
        }
    }
}
