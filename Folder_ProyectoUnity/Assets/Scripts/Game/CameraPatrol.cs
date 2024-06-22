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
            Vector3 direction = SubtractVectors(destination, transform.position);
            direction = NormalizeVector(direction);
            transform.position = AddVectors(transform.position, MultiplyVector(direction, speed * Time.deltaTime));
            if (DistanceBetweenVectors(transform.position, destination) < pointSpacing)
            {
                if (index < patrolPoints.Length - 1)
                {
                    index = index + 1;
                }
                else
                {
                    index = 0;
                }
                destination = patrolPoints[index].position;
            }
        }
    }
    private Vector3 AddVectors(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    private Vector3 SubtractVectors(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    private Vector3 MultiplyVector(Vector3 a, float scalar)
    {
        return new Vector3(a.x * scalar, a.y * scalar, a.z * scalar);
    }
    private float DistanceBetweenVectors(Vector3 a, Vector3 b)
    {
        Vector3 differenceVector = SubtractVectors(a, b);
        return Mathf.Sqrt(differenceVector.x * differenceVector.x + differenceVector.y * differenceVector.y + differenceVector.z * differenceVector.z);
    }
    private Vector3 NormalizeVector(Vector3 vector)
    {
        float length = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        return new Vector3(vector.x / length, vector.y / length, vector.z / length);
    }
}
