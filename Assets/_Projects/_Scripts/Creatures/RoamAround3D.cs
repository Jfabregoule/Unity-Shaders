using UnityEngine;

public class RoamAround3D : MonoBehaviour
{
    [Header("Boundaries (Relative)")]
    public Vector3 boundaryRange = new Vector3(5f, 5f, 5f);

    [Header("Movement Parameters")]
    public float speed = 2f;
    public float changeDirectionTime = 2f;
    public float smoothingFactor = 0.2f;

    private Vector3 initialPosition;
    private Vector3 currentDirection;
    private Vector3 velocity;
    private float changeDirectionTimer;

    void Start()
    {
        initialPosition = transform.position;

        ChangeDirection();
    }

    void Update()
    {
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            ChangeDirection();
            changeDirectionTimer = changeDirectionTime;
        }

        velocity = Vector3.Lerp(velocity, currentDirection, smoothingFactor * Time.deltaTime);
        transform.position += velocity * speed * Time.deltaTime;

        ClampPosition();
    }

    private void ChangeDirection()
    {
        // Generate a random direction
        currentDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    private void ClampPosition()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, initialPosition.x - boundaryRange.x, initialPosition.x + boundaryRange.x),
            Mathf.Clamp(transform.position.y, initialPosition.y - boundaryRange.y, initialPosition.y + boundaryRange.y),
            Mathf.Clamp(transform.position.z, initialPosition.z - boundaryRange.z, initialPosition.z + boundaryRange.z)
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                initialPosition,
                boundaryRange * 2f
            );
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                transform.position,
                boundaryRange * 2f
            );
        }
    }
}
