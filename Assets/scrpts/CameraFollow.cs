using UnityEngine;

/// <summary>
/// CameraFollow - Smooth camera that follows the car with customizable offset
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // The car to follow

    [Header("Camera Position")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float followSpeed = 10f;
    
    [Header("Camera Rotation")]
    [SerializeField] private float lookAheadDistance = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    
    //[Header("Lane Change Smoothing")]
    //[SerializeField] private bool followLaneChanges = true;
    //[SerializeField] private float laneFollowSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: No target assigned!");
            return;
        }

        // Calculate desired position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        
        // Smoothly move camera to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Look at point slightly ahead of the car
        Vector3 lookAtPoint = target.position + target.forward * lookAheadDistance;
        
        // Smoothly rotate camera to look at target
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
