using UnityEngine;
public class CameraPatrol : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] patrolPoints;
    [Header("Properties")]
    [SerializeField] private float speed;
    [SerializeField] private float pointSpacing;
    private int index = 0;
    private Vector3 destination;
    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            destination = patrolPoints[index].position + new Vector3(pointSpacing, pointSpacing, pointSpacing);
        }
    }
    void Update()
    {
        if (patrolPoints.Length != 0)
        {
            Vector3 currentPosition = transform.position;
            Vector3 direction = destination - currentPosition;
            float distanceToDestination = direction.magnitude;

            if (distanceToDestination < pointSpacing)
            {
                index = (index + 1) % patrolPoints.Length;
                destination = patrolPoints[index].position;
            }
            transform.position += direction.normalized * speed * Time.deltaTime;
        }
    }
}
