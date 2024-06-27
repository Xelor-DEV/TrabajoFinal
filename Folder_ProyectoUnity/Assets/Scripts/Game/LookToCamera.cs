using UnityEngine;
public class LookToCamera : MonoBehaviour
{
    [SerializeField] private Transform thisObject;
    [SerializeField] private Camera targetCamera;
    private Vector3 lookDirection;
    private void Update()
    {
        lookDirection = targetCamera.transform.position - thisObject.transform.position;
        thisObject.rotation = Quaternion.LookRotation(lookDirection);
    }
}
