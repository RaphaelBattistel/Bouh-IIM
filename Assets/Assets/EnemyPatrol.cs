using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private float savespeed;
    public float viewDistance = 5f;
    public Vector2 viewBoxSize = new Vector2(2f, 1f);
    public Transform player;

    private Vector3 currentTarget;
    private SpriteRenderer spriteRenderer;
    private bool returningToZone = false;
    private float fixedY;
    private Collider2D coll2d;

    void Start()
    {
        savespeed = speed   ;
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll2d = GetComponent<Collider2D>();
        currentTarget = pointB.position;
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (!IsInsideLimits(transform.position))
        {
            returningToZone = true;
            ReturnToZone();
            return;
        }

        if (returningToZone)
        {
            returningToZone = false;
            currentTarget = ClosestPatrolPoint().position;
        }

        if (CanSeePlayer() && IsInsideLimits(player.position))
        {
            speed=4f;
            ChasePlayer();
        }
        else
        {
            Patrol();
            speed = savespeed;
        }
    }

    void Patrol()
    {
        Vector3 targetPos = new Vector3(currentTarget.x, fixedY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        spriteRenderer.flipX = (targetPos.x < transform.position.x);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position;
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, fixedY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        spriteRenderer.flipX = (player.position.x < transform.position.x);
    }

    void ReturnToZone()
    {
        Transform closestPoint = ClosestPatrolPoint();
        Vector3 targetPos = new Vector3(closestPoint.position.x, fixedY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        spriteRenderer.flipX = (targetPos.x < transform.position.x);
    }

    bool IsInsideLimits(Vector3 pos)
    {
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        return pos.x >= minX && pos.x <= maxX;
    }

    Transform ClosestPatrolPoint()
    {
        float distA = Vector3.Distance(transform.position, pointA.position);
        float distB = Vector3.Distance(transform.position, pointB.position);
        return distA < distB ? pointA : pointB;
    }

    bool CanSeePlayer()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.BoxCast(origin, viewBoxSize, 0f, direction, viewDistance, LayerMask.GetMask("Player"));

        return hit.collider != null && hit.collider.transform == player;
    }

}
