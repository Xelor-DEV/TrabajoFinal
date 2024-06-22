using UnityEngine;
using Cinemachine;
public class CameraSwitcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIManagerMenu uiManager;
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    [SerializeField] private CinemachineVirtualCamera patrollingCamera;
    private int currentActiveIndex = -1;
    private void OnEnable()
    {
        uiManager.OnWindowShow += SelectCamera;
        uiManager.OnWindowHide += ReturnToPatrolCamera;
    }
    private void OnDisable()
    {
        uiManager.OnWindowShow -= SelectCamera;
        uiManager.OnWindowHide -= ReturnToPatrolCamera;
    }
    void Start()
    {
        patrollingCamera.Priority = 1;
    }
    public void SelectCamera(int index)
    {
        if (index >= 0 && index < cameras.Length)
        {
            if (currentActiveIndex != -1)
            {
                cameras[currentActiveIndex].Priority = 0;
            }
            cameras[index].Priority = 10;
            currentActiveIndex = index;
            patrollingCamera.Priority = 0;
        }
    }
    public void ReturnToPatrolCamera()
    {
        if (currentActiveIndex != -1)
        {
            cameras[currentActiveIndex].Priority = 0;
            currentActiveIndex = -1;
        }
        patrollingCamera.Priority = 1;
    }
}
