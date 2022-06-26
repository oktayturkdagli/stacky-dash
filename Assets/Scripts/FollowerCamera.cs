using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    public Transform target;
    public Transform camTransform;
    public Vector3 offset;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        offset = camTransform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}