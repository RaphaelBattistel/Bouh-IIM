using UnityEngine;

public class Possession : MonoBehaviour
{

    [SerializeField] private bool possessing = false;

    [SerializeField] private GameObject emission;

    [SerializeField] private PlayerController playcont;


    [SerializeField] private float speedMove;

    [SerializeField] private LayerMask detectWall;
    [SerializeField] private float wallDist = 1f;

    [SerializeField] private bool grounded = false;
    [SerializeField] private bool wasGrounded = false;

    [SerializeField] private bool isBrocken = false;
    [SerializeField] private float maxFall = 2;
    private float lastY;
    private float fallSpeed;
    public Rigidbody2D rb;
    private CapsuleCollider2D _caps2d;
    private Collider2D _col2d; 

    public enum OBJECTS
    {
        NONE,
        BOX,
        TV,
        VASE
    }
    [SerializeField] private OBJECTS _objects;

    void Start()
    {
        if (emission == null)
        {
            return;
        }
        _caps2d = emission.GetComponent<CapsuleCollider2D>();
        

        _col2d = GetComponent<Collider2D>();
        playcont = GetComponent<PlayerController>();


        _caps2d.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&&possessing)
        {
            ReleasePossession();
        }

        fallSpeed = lastY - transform.position.y;
        lastY = transform.position.y;

        if (possessing == true)
        {
            playcont.canJump = false;
            playcont.canMove = false;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;


            if (_objects == OBJECTS.BOX || _objects == OBJECTS.VASE)
            {
                Move();

                if (_objects == OBJECTS.VASE)
                {
                    if (!wasGrounded && grounded)
                    {
                        // Atterrissage d�tect�, on v�rifie la vitesse de chute
                        if (fallSpeed > maxFall && !isBrocken)
                        {
                            isBrocken = true;
                            _caps2d.enabled = true;

                        }
                    }
                    wasGrounded = grounded;
                }
            }
            else if (_objects == OBJECTS.TV)
            {
                _caps2d.enabled = true;
            }

        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }

    }
    
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Shoot")
    //    {
    //        possessing = true;
    //        Debug.Log("il est 1h du zbah, marches un peu!");
    //    }
    //}


    private void Move()
    {
        Vector3 direction = Input.GetAxis("Horizontal") * Vector2.right;
        var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, direction, wallDist, detectWall);

        if (hit.collider != null)
            return;

        Debug.DrawRay(transform.position, direction * wallDist);


        transform.position += Vector3.right * Input.GetAxisRaw("Horizontal") * speedMove * Time.deltaTime;
    }

    public void StartPossession(PlayerController player)
    {
        possessing = true;
        playcont = player;
        playcont.isPossessing = true;
        playcont.possessionTarget = transform;
        playcont.canBeHurted = false;

        if (playcont != null)
        {
            playcont.canJump = false;
            playcont.canMove = false;
        }

        if (_caps2d != null)
            _caps2d.enabled = true;

    }

    private void ReleasePossession()
    {
        possessing = false;
        playcont.canBeHurted = true;
        playcont.isPossessing = false;

        if (playcont != null)
        {
            playcont.canJump = true;
            playcont.canMove = true;
        }

        if (_caps2d != null)
        {
            _caps2d.enabled = false;
        }

    }
}
