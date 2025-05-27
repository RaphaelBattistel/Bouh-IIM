using UnityEngine;

public class Possession : MonoBehaviour
{

    [SerializeField] private bool possessing = false;

    [SerializeField] private GameObject emission;

    [SerializeField] private PlayerControllers playcont;


    [SerializeField] private float speedMove;

    [SerializeField] private LayerMask detectWall;
    [SerializeField] private float wallDist = 1f;

    private CapsuleCollider2D _col2d;

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
        _col2d = emission.GetComponent<CapsuleCollider2D>();
        
        playcont = GetComponent<PlayerControllers>();

        
        _col2d.enabled = false; 
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            possessing = false ;
            if(playcont != null)
            {
                playcont.canJump = true;
                playcont.canMove = true;
            }
            

            if (emission != null) 
            {
                _col2d.enabled = false;
            }
            
        }

        if (possessing == true)
        {
            playcont.canJump = false ;
            playcont.canMove = false ;
            if (_objects == OBJECTS.BOX)
            {
                Debug.Log("box");
                Move();
            }
            else if (_objects == OBJECTS.TV) 
            {
                Debug.Log("TVS");
                _col2d.enabled = true ;
            }
        }

    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shoot")
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shoot")
        {
            possessing = true;
            Debug.Log("il est 1h du zbah, marches un peu!");
        }
    }


    private void Move()
    {
        Vector3 direction = Input.GetAxis("Horizontal") * Vector2.right;
        var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, direction, wallDist, detectWall);

        if (hit.collider != null)
            return;

        Debug.DrawRay(transform.position, direction * wallDist);


        transform.position += Vector3.right * Input.GetAxisRaw("Horizontal") * speedMove * Time.deltaTime;
    }


}
