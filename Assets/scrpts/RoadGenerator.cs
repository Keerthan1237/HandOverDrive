using UnityEngine;

/// <summary>
/// RoadGenerator - Creates an infinite scrolling road effect
/// Recycles road segments as the car moves forward
/// </summary>
public class RoadGenerator : MonoBehaviour
{
    [Header("Road Settings")]
    [SerializeField] private GameObject roadSegmentPrefab;
    [SerializeField] private int visibleSegments = 10;
    [SerializeField] private float segmentLength = 20f;
    [SerializeField] private Transform carTransform;

    private Transform[] roadSegments;
    private int currentSegmentIndex = 0;

    private void Start()
    {
        GenerateInitialRoad();
    }

    private void Update()
    {
        if (carTransform == null) return;

        // Check if we need to move road segments
        float carPosition = carTransform.position.z;
        float nextSegmentPosition = roadSegments[currentSegmentIndex].position.z;

        // If car has passed the current segment, move it to the end
        if (carPosition > nextSegmentPosition + segmentLength)
        {
            MoveSegmentToEnd(currentSegmentIndex);
            currentSegmentIndex = (currentSegmentIndex + 1) % visibleSegments;
        }
    }

    /// <summary>
    /// Generates the initial road segments
    /// </summary>
    private void GenerateInitialRoad()
    {
        roadSegments = new Transform[visibleSegments];

        for (int i = 0; i < visibleSegments; i++)
        {
            GameObject segment = Instantiate(roadSegmentPrefab, transform);
            segment.transform.position = new Vector3(0, 0, i * segmentLength);
            roadSegments[i] = segment.transform;
        }
    }

    /// <summary>
    /// Moves a road segment to the end of the road
    /// </summary>
    private void MoveSegmentToEnd(int index)
    {
        // Find the last segment
        int lastIndex = (currentSegmentIndex + visibleSegments - 1) % visibleSegments;
        float lastPosition = roadSegments[lastIndex].position.z;

        // Move this segment to after the last one
        Vector3 newPosition = roadSegments[index].position;
        newPosition.z = lastPosition + segmentLength;
        roadSegments[index].position = newPosition;
    }
}
