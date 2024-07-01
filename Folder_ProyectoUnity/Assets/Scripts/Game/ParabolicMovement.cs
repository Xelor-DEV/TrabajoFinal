using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMovement : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    private Vector3 initialPosition;
    private float timeElapsed;
    public Transform TargetObject
    {
        get 
        { 
            return targetObject; 
        }
        set 
        { 
            targetObject = value; 
        }
    }
    private void Start()
    {
        initialPosition = transform.position;
        timeElapsed = 0f;
    }
    private void Update()
    {
        Vector3 targetPosition = targetObject.position;
        Vector3 horizontalDirection = targetPosition - initialPosition;
        float horizontalDistance = horizontalDirection.magnitude;
        float timeOfFlight = (2f * speed * Mathf.Sin(Mathf.PI / 4f)) / gravity;
        timeElapsed = timeElapsed + Time.deltaTime;
        float horizontalVelocity = speed * Mathf.Cos(Mathf.PI / 4f);
        float currentX = initialPosition.x + horizontalVelocity * timeElapsed;
        float currentY = initialPosition.y + (speed * Mathf.Sin(Mathf.PI / 4f) - 0.5f * gravity * timeElapsed * timeElapsed);
        float currentZ = initialPosition.z + horizontalDirection.normalized.z * horizontalDistance;
        transform.position = new Vector3(currentX, currentY, currentZ);
    }
}

