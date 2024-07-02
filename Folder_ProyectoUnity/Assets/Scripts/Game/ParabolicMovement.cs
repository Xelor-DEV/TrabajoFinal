using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMovement : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private bool hasInitialized = false;
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
        if (targetObject != null)
        {
            InitializeProjectile();
        }
    }
    private void InitializeProjectile()
    {
        initialPosition = transform.position;
        targetPosition = targetObject.position;

        Vector3 direction = targetPosition - initialPosition;
        float distance = direction.magnitude;
        float heightDifference = direction.y;
        direction.y = 0;
        float horizontalDistance = direction.magnitude;

        float angle = launchAngle * Mathf.Deg2Rad;
        float v0 = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * angle));
        float v0x = v0 * Mathf.Cos(angle);
        float v0y = v0 * Mathf.Sin(angle);

        Vector3 horizontalDirection = direction.normalized;
        velocity = horizontalDirection * v0x;
        velocity.y = v0y;

        hasInitialized = true;
    }
    private void Update()
    {
        if (hasInitialized == false)
        {
            if (targetObject != null)
            {
                InitializeProjectile();
            }
            else
            {
                return;
            }
        }

        float timeStep = Time.deltaTime;
        velocity.y -= gravity * timeStep;
        Vector3 displacement = velocity * timeStep;

        transform.position += displacement;
    }
}

