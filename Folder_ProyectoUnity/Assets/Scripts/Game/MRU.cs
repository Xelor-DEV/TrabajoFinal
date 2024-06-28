using UnityEngine;
public class MRU : MonoBehaviour
{
    private Rigidbody _compRigidbody;
    [SerializeField] private Vector3 velocity;
    private void Start()
    {
        _compRigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        // x = x0 + v * t
        Vector3 newPosition = transform.position + displacement;
        _compRigidbody.MovePosition(newPosition);
    }
}
