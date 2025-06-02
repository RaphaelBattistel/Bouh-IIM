using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;


public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Stats Stat { get; private set; }

    [Header("JUMP")]
    [SerializeField] private float jumpower;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float fallingDirection;
    public bool canJump = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    [Header("MOVE")]
    public bool canMove = true;
    public float speed;
    float horizontal;

    [Header("WALL")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool walltriger;
    public bool wallcheck;
    [SerializeField] private bool walltoggle;

    [Header("STAMINA")]
    [SerializeField] private float stamina;
    [SerializeField] private float staminaLeft;
    [SerializeField] private GameObject staminaBar;

    [Header("LIFE")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    private bool isdead;
    public bool canBeHurted = true;
    [SerializeField] private GameObject hearth1;
    [SerializeField] private GameObject hearth2;
    [SerializeField] private GameObject hearth3;
    [SerializeField] private Transform lastChekpoint;

    [Header("ENEMY")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private EnemyController EnemyController;

    [Header("POSSESS")]
    public bool isPossessing = false;
    public Transform possessionTarget;

    private SpriteRenderer skin;
    private Rigidbody2D rb;
    private Collider2D myCollider;
    private Animator anim;





    void Start()
    {
        skin = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        currentHp = maxHp;
        staminaLeft = stamina;
        isdead = false;
        lastChekpoint.position = transform.position;
        canBeHurted = true;
        isPossessing = false;
    }
    public void movement(InputAction.CallbackContext context)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rb.linearVelocity = new Vector2(1 * speed, rb.linearVelocity.y);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(-1 * speed, rb.linearVelocity.y);
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, fallingSpeed * rb.linearVelocity.y);
        if (walltriger == true && staminaLeft > 0)
        {
            staminaLeft -= 0.25f;
            wallcheck = true;
        }
        else
        {
            wallcheck = false;
        }
        bump();
        IsFalling();
        if (currentHp != 0)
        {
            if (currentHp == 1)
            {
                hearth1.SetActive(true);
                hearth2.SetActive(false);
                hearth3.SetActive(false);
            }
            else if (currentHp == 2)
            {
                hearth1.SetActive(true);
                hearth2.SetActive(true);
                hearth3.SetActive(false);
            }
            else if (currentHp == 3)
            {
                hearth1.SetActive(true);
                hearth2.SetActive(true);
                hearth3.SetActive(true);
            }
        }
        if (staminaLeft > 0)
        {
            staminaBar.transform.localScale = new Vector2(staminaLeft / stamina, staminaBar.transform.localScale.y);
        }
        isControlling();

        animCheck();
    }


    private void animCheck()
    {
        anim.SetFloat("VelocityX", MathF.Abs(rb.linearVelocity.x));
        anim.SetFloat("VelocityY", rb.linearVelocity.y);
        anim.SetBool("Grounded", IsGrounded());
        anim.SetBool("Wall", wallcheck);
    }

    public void wallmode(InputAction.CallbackContext context)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            walltriger = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            walltriger = false;
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        if (canMove = true)
        {
            horizontal = context.ReadValue<Vector2>().x;

            if (horizontal < 0)
            {
                skin.flipX = true;
            }
            else if (horizontal > 0)
            {
                skin.flipX = false;
            }
        }
    }
    public void jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !isdead)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpower);

        }
    }
    public bool IsGrounded()
    {
        if (canJump)
        {
            return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(.5f, .1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        }
        else
        {
            return false;
        }
    }
    void bump()
    {
        if (Physics2D.OverlapCapsule(groundCheck.position, new Vector2(.5f, .1f), CapsuleDirection2D.Horizontal, 0, enemyLayer) && !isdead)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpower);
            staminaLeft = stamina;
        }
    }
    public void isControlling()
    {
        if (isPossessing)
        {
            Color currentColor = skin.color;
            currentColor.a = 0;
            skin.color = currentColor;
            myCollider.isTrigger = true;
            transform.position = possessionTarget.position;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        else
        {
            Color currentColor = skin.color;
            currentColor.a = 1;
            skin.color = currentColor;
            myCollider.isTrigger = false;
        }
    }
    void IsFalling()
    {
        fallingDirection = Input.GetAxis("Vertical");

        if (fallingDirection < 0)
        {
            fallingSpeed = 0.5f;
        }
        else
        {
            fallingSpeed = 1f;
        }
    }

    public void dead()
    {
        transform.position = lastChekpoint.position;
        currentHp = maxHp;
        staminaLeft = stamina;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && canBeHurted)
        {
            anim.SetTrigger("Hurt");
            canMove = false;
            if (currentHp != 1)
            {
                if (skin.flipX == true)
                {
                    horizontal = 5;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + 5);
                }
                else if (skin.flipX == false)
                {
                    horizontal = -5;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + 5);
                }
                currentHp -= 1;
                StartCoroutine(delay1(0.25f));
                StartCoroutine(delay3(0.25f));
            }
            else
            {
                dead();
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            GameManager.instance.Win();
            canMove = false;
        }
    }
    IEnumerator delay1(float time)
    {
        yield return new WaitForSeconds(time);
        if (horizontal > 0)
        {
            horizontal = 2.5f;
        }
        else if (horizontal < 0)
        {
            horizontal = -2.5f;
        }
    }


    IEnumerator delay3(float time)
    {
        yield return new WaitForSeconds(time);
        if (horizontal > 0)
        {
            horizontal = 0;
            canMove = true;
        }
        else if (horizontal < 0)
        {
            horizontal = 0;
        }
    }
    public void updateCheckpoint()
    {
        lastChekpoint.position = transform.position;
    }
    
    
}