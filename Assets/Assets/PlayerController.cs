using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using A1_24_25;

public class PlayerController : MonoBehaviour
{
    public float jumpower;
    public float speed;
    public float fallingSpeed;
    public float fallingDirection;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask wallLayer;
    public Transform groundCheck;
    public SpriteRenderer skin;
    float horizontal;
    public bool walltriger;
    public bool wallcheck;
    public bool walltoggle;
    public float stamina;
    public float staminaLeft;
    public EnemyController EnemyController;
    private CameraController CameraController;
    public float maxHp;
    public float currentHp;
    private bool isdead;
    public bool canMove = true;
    public Transform lastChekpoint;
    public GameObject staminaBar;
    public GameObject hearth1;
    public GameObject hearth2;
    public GameObject hearth3;

    void Start()
    {
        currentHp = maxHp;
        staminaLeft = stamina;
        isdead = false;
        lastChekpoint.position = transform.position;
        CameraController = FindObjectOfType<CameraController>();
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
    bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(.5f, .1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    void bump()
    {
        if (Physics2D.OverlapCapsule(groundCheck.position, new Vector2(.5f, .1f), CapsuleDirection2D.Horizontal, 0, enemyLayer) && !isdead)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpower);
            staminaLeft = stamina;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            CameraController.ShakeCamera(0.3f, 2f);
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