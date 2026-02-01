using UnityEngine;

/// <summary>
/// SimpleTrafficMove - Makes any object move forward at a set speed
/// No wheels, no complexity - just movement!
/// </summary>
public class SimpleTrafficMove : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float speed = 15f;

    private void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
