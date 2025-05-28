using UnityEngine;

public class Possession : MonoBehaviour
{

    [SerializeField] private bool possessing = false;

    [SerializeField] private GameObject emission;

    [SerializeField] private PlayerControllers playcont;


    [SerializeField] private float speedMove;

    [SerializeField] private LayerMask detectWall;
    [SerializeField] private float wallDist = 1f;

    [SerializeField] private bool grounded = false;
    [SerializeField] private bool wasGrounded = false;

    [SerializeField] private bool isBrocken = false;
    [SerializeField] private float maxFall = 2;
    private float lastY;
    private float fallSpeed;

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
        playcont = GetComponent<PlayerControllers>();


        _caps2d.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleasePossession();
        }

        fallSpeed = lastY - transform.position.y;
        lastY = transform.position.y;

        if (possessing == true)
        {
            playcont.canJump = false ;
            playcont.canMove = false ;

            if (_objects == OBJECTS.BOX || _objects == OBJECTS.VASE)
            {
                Move();
                if(_objects == OBJECTS.BOX)
                {
                    Debug.Log("box");
                }
                else if (_objects == OBJECTS.VASE)
                {
                    if (!wasGrounded && grounded)
                    {
                        // Atterrissage détecté, on vérifie la vitesse de chute
                        if (fallSpeed > maxFall && !isBrocken)
                        {
                            isBrocken = true;
                            _caps2d.enabled = true;
                            Debug.Log("Le vase est cassé (sans Rigidbody2D)");
                            
                        }
                        else
                        {
                            Debug.Log("pas cassé");
                        }
                    }
                    wasGrounded = grounded;
                }
            }
            else if (_objects == OBJECTS.TV) 
            {
                Debug.Log("TVS");
                _caps2d.enabled = true ;
            }

        }

    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true ;
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

    public void StartPossession(PlayerControllers player)
    {
        possessing = true;
        playcont = player;

        if (playcont != null)
        {
            playcont.canJump = false;
            playcont.canMove = false;
        }

        if (_caps2d != null)
            _caps2d.enabled = true;

        Debug.Log("Possession démarrée");
    }

    private void ReleasePossession()
    {
        possessing = false;

        if (playcont != null)
        {
            playcont.canJump = true;
            playcont.canMove = true;
        }

        if (_caps2d != null)
        {
            _caps2d.enabled = false;
        }

        Debug.Log("Possession terminée.");
    }
}
