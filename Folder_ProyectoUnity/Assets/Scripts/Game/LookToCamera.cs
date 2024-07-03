using UnityEngine;
public class LookToCamera : MonoBehaviour
{
    [SerializeField] private Transform thisObject;
    [SerializeField] private Camera targetCamera;

    private void Update()
    {
        thisObject.transform.forward = targetCamera.transform.forward;
    }
}