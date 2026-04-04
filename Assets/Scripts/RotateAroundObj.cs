using UnityEngine;
// Code from 
public class RotateAroundObj : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject verticalPivot; // The object to rotate vertically
    public GameObject PivotObj; // The object to rotate around horizontally
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
        if(targetObj == null)
        {
            return;
        }
        // Calculate direction from sourceObj's position, not transform's
        Vector3 targetDirection = targetObj.transform.position - sourceObj.transform.position;

        float horizontalAngle = Vector3.SignedAngle(
            sourceObj.transform.forward,
            targetDirection,
            Vector3.up
        );

        bool facingHorizontal = Mathf.Abs(horizontalAngle) <= 0.5f;

        if (!facingHorizontal)
        {
            float hStep = Mathf.Min(rotationSpeed * Time.deltaTime, Mathf.Abs(horizontalAngle));
            float hDirection = Mathf.Sign(horizontalAngle);
            transform.RotateAround(PivotObj.transform.position, Vector3.up, hDirection * hStep);
        }

        // Vertical rotation (X axis, up/down)
        if (verticalPivot != null)
        {
            // Flatten both vectors to get a pure vertical angle
            Vector3 localTarget = verticalPivot.transform.InverseTransformDirection(targetDirection);
            float verticalAngle = Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;

            bool facingVertical = Mathf.Abs(verticalAngle) <= 0.5f;

            if (!facingVertical)
            {
                float vStep = Mathf.Min(rotationSpeed * Time.deltaTime, Mathf.Abs(verticalAngle));
                float vDirection = Mathf.Sign(verticalAngle);
                verticalPivot.transform.Rotate(Vector3.right, -vDirection * vStep, Space.Self);

                Vector3 clampedEuler = verticalPivot.transform.localEulerAngles;
                float xAngle = clampedEuler.x > 180f ? clampedEuler.x - 360f : clampedEuler.x;
                clampedEuler.x = Mathf.Clamp(xAngle, -60f, 60f);
                verticalPivot.transform.localEulerAngles = clampedEuler;
            }

            isFacingTarget = facingHorizontal && facingVertical;
        }
        else
        {
            isFacingTarget = facingHorizontal;
        }
        
        // Use sourceObj's forward vector for the angle calculation
        // float angleToTarget = Vector3.SignedAngle(sourceObj.transform.forward, targetDirection, Vector3.up);

        // Stops rotation when sourceObj is facing the targetObj
        // if (Mathf.Abs(angleToTarget) <= 0.5f)
        // {
        //     isFacingTarget = true;
        //     Debug.Log("Facing target");
        //     return;
        // }

        

        if(isFacingTarget)
        {
            // Debug.Log("Facing target");
        }

        // float step = Mathf.Min(rotationSpeed * Time.deltaTime, Mathf.Abs(angleToTarget));
        // // Changes the direction of rotation based on the sign of the angle to the target
        // float direction = Mathf.Sign(angleToTarget);

        // transform.RotateAround(
        //     PivotObj.transform.position,
        //     Vector3.up,
        //     direction * step
        // );
    }
}
