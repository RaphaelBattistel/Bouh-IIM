using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public float speed = 2f;
    public float chaseSpeed = 4f;
    public float viewDistance = 5f;
    public Vector2 visionSize = new Vector2(2f, 1f);

    private Vector3 targetPoint;
    private SpriteRenderer sr;
    private float baseY;
    private bool goingBack = false;
    private float normalSpeed;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        targetPoint = pointB.position;
        baseY = transform.position.y;
        normalSpeed = speed;
    }

    void Update()
    {
        if (!IsInZone(transform.position))
        {
            goingBack = true;
            GoBackToZone();
            return;
        }

        if (goingBack)
        {
            goingBack = false;
            targetPoint = GetClosestPoint().position;
        }

        if (CanSeePlayer() && IsInZone(player.position))
        {
            speed = chaseSpeed;
            Chase();
        }
        else
        {
            speed = normalSpeed;
            Patrol();
        }
    }

    void Patrol()
    {
        Vector3 target = new Vector3(targetPoint.x, baseY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        sr.flipX = target.x < transform.position.x;

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            targetPoint = targetPoint == pointA.position ? pointB.position : pointA.position;
        }
    }

    void Chase()
    {
        Vector3 playerPos = new Vector3(player.position.x, baseY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        sr.flipX = player.position.x < transform.position.x;
    }

    void GoBackToZone()
    {
        Transform close = GetClosestPoint();
        Vector3 backPos = new Vector3(close.position.x, baseY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, backPos, speed * Time.deltaTime);
        sr.flipX = backPos.x < transform.position.x;
    }

    bool IsInZone(Vector3 pos)
    {
        float left = Mathf.Min(pointA.position.x, pointB.position.x);
        float right = Mathf.Max(pointA.position.x, pointB.position.x);
        return pos.x >= left && pos.x <= right;
    }

    Transform GetClosestPoint()
    {
        float distA = Vector3.Distance(transform.position, pointA.position);
        float distB = Vector3.Distance(transform.position, pointB.position);
        return distA < distB ? pointA : pointB;
    }

    bool CanSeePlayer()
    {
        Vector2 dir = sr.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.BoxCast(origin, visionSize, 0f, dir, viewDistance, LayerMask.GetMask("Player"));
        return hit.collider != null && hit.collider.transform == player;
    }
}
