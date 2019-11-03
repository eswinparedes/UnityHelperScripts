using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Make into instanced class that has a delegate dispatcher for on complete
[SerializeField]
public class C_MoveTowards
{
    #region Testing
    public float minSpeed;
    public float maxSpeed;
    public float acceleration;
    public float brakeDistance;
    public float steeringAccuracy;
    public Transform transform;

    private Vector3 moveVector = Vector3.zero;
    private float currentSpeed;

    public void MoveTowards(Vector3 destination, float deltaTime)
    {
        if (destination == transform.position)
        {
            return;
        }

        Vector3 val = destination - transform.position;
        float mag = val.magnitude;

        Vector3 dir = val.normalized;
        Vector3 accel = dir * acceleration * deltaTime;

        moveVector += accel;

        transform.position += dir * (moveVector.magnitude);
    }

    void BrakeDistanceBehaviour(float mag, float deltaTime)
    {
        float alphaSpeed = currentSpeed / maxSpeed;
        float alphaDistance = mag / brakeDistance;
    }

    void Accelerate(float mag, float deltaTime)
    {
        currentSpeed += currentSpeed + acceleration * deltaTime <= maxSpeed ? currentSpeed * acceleration * deltaTime : maxSpeed;

        if (currentSpeed > mag)
        {
            currentSpeed = mag;
        }
        else if (currentSpeed < minSpeed * deltaTime)
        {
            currentSpeed = minSpeed;
        }
    }

    void Deccelerate()
    {

    }

    #endregion

    #region static functions
    
    #endregion
}

