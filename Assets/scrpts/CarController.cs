using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Input Source (UDP)")]
    public HandUDPReceiver input;

    [Header("Speed Settings")]
    public float baseSpeed = 10f;
    public float maxSpeed = 30f;
    public float accelerationRate = 5f;
    public float decelerationRate = 8f;
    public float naturalSlowdown = 2f;

    [Header("Lane Settings")]
    public float laneWidth = 4f;
    public int totalLanes = 3;
    public float laneChangeSpeed = 8f;

    [Header("Steering")]
    public float maxSteeringAngle = 25f;
    public float steeringSpeed = 6f;
    public float steeringReturnSpeed = 5f;

    private float currentSpeed;
    private int currentLane = 1;
    private int targetLane = 1;

    private float currentX;
    private float targetX;
    private bool isChangingLane;

    private float currentSteeringAngle;

    public float laneInputCooldown = 0.25f;
    public float laneTimer = 0f;

    void Start()
    {
        currentSpeed = baseSpeed;

        currentX = GetLaneXPosition(currentLane);
        targetX = currentX;

        Vector3 pos = transform.position;
        pos.x = currentX;
        transform.position = pos;
    }

    void Update()
    {

        if (input == null) return;

        HandleAcceleration();
        HandleLaneInput();
        MoveForward();
        MoveLane();
        HandleSteering();

        laneTimer -= Time.deltaTime;
    }

    void HandleAcceleration()
    {
        float throttle = input.throttle;
        float brake = input.brake;

        if (throttle > 0.5f)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
        }
        else if (brake > 0.5f)
        {
            currentSpeed -= decelerationRate * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, naturalSlowdown * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
    }

    void HandleLaneInput()
    {
        if (isChangingLane) return;
        if (laneTimer > 0f) return;

        float steer = input.steer;

        // threshold prevents jitter
        if (steer < -0.5f && currentLane > 0)
        {
            targetLane = currentLane - 1;
            BeginLaneChange();
        }
        else if (steer > 0.5f && currentLane < totalLanes - 1)
        {
            targetLane = currentLane + 1;
            BeginLaneChange();
        }
    }

    void BeginLaneChange()
    {
        targetX = GetLaneXPosition(targetLane);
        isChangingLane = true;
        laneTimer = laneInputCooldown;
    }

    void MoveForward()
    {
        transform.position += -Vector3.forward * currentSpeed * Time.deltaTime;
    }

    void MoveLane()
    {
        currentX = Mathf.Lerp(currentX, targetX, laneChangeSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = currentX;
        transform.position = pos;

        if (isChangingLane && Mathf.Abs(currentX - targetX) < 0.05f)
        {
            currentX = targetX;
            currentLane = targetLane;
            isChangingLane = false;
        }
    }

    void HandleSteering()
    {
        float targetSteering = 0f;

        if (isChangingLane)
        {
            float direction = targetX - currentX;
            float intensity = Mathf.Clamp01(Mathf.Abs(direction) / laneWidth);
            targetSteering = Mathf.Sign(direction) * maxSteeringAngle * intensity;
        }

        float speed = isChangingLane ? steeringSpeed : steeringReturnSpeed;

        currentSteeringAngle = Mathf.Lerp(
            currentSteeringAngle,
            targetSteering,
            speed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, currentSteeringAngle, 0f);
    }

    float GetLaneXPosition(int laneIndex)
    {
        float totalWidth = (totalLanes - 1) * laneWidth;
        return totalWidth / 2f - laneIndex * laneWidth;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    for (int i = 0; i < totalLanes; i++)
    //    {
    //        float x = GetLaneXPosition(i);
    //        Gizmos.DrawLine(new Vector3(x, 0, -50), new Vector3(x, 0, 50));
    //    }
    //}
}