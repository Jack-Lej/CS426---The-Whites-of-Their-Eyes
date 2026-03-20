using UnityEngine;
// Code from 
public class RotateAroundObj : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject PivotObj; // The object to rotate around
    // Rotates around until facing the target object
    public GameObject targetObj; // The object to face towards
    public GameObject sourceObj; // The object that is rotating, used for calculating the angle to the target object
    private bool isFacingTarget = false;
    // public float fieldOfView = 0.5f;

    public bool IsFacingTarget
    {
        get { return isFacingTarget; }
    }

    public GameObject TargetObj
    {
        get { return targetObj; }
        set { targetObj = value; }
    }

    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    // Update is called once per frame
    internal void Update()
    {
        // Calculate direction from sourceObj's position, not transform's
        Vector3 targetDirection = targetObj.transform.position - sourceObj.transform.position;
        
        // Use sourceObj's forward vector for the angle calculation
        float angleToTarget = Vector3.SignedAngle(sourceObj.transform.forward, targetDirection, Vector3.up);

        // Stops rotation when sourceObj is facing the targetObj
        if (Mathf.Abs(angleToTarget) <= 0.5f)
        {
            isFacingTarget = true;
            // Debug.Log("Facing target");
            return;
        }
        else
        {
            isFacingTarget = false;
        }

        float step = Mathf.Min(rotationSpeed * Time.deltaTime, Mathf.Abs(angleToTarget));
        // Changes the direction of rotation based on the sign of the angle to the target
        float direction = Mathf.Sign(angleToTarget);

        transform.RotateAround(
            PivotObj.transform.position,
            Vector3.up,
            direction * step
        );
    }
}
