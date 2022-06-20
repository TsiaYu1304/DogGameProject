using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeed : MonoBehaviour
{
    public Animator animator;
    public float movementSpeed = 1.0f ;
    public float JumpForce = 1.0f;
    [SerializeField] Transform groundCheckCollider;
    const float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;


    public float fallSpeed; //重力速度
    //public float jumpTime;　//跳躍的最最大蓄力時間
    //public float timeJump; //跳躍當前蓄力時間
    //public bool jumpState;
    public int jumpCount;
    public bool _isGrounded;
    [SerializeField] public float moveDirection;

    private bool isFalling;
    private bool isInputEnabled;

    float horizontalMove = 0f;


    private Rigidbody2D _rigidbody;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        isInputEnabled = true;
        jumpCount = 2;
        animator = gameObject.GetComponent<Animator>();
        _transform = gameObject.GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void JumpControl()
    {//JumpControl
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        else if (jumpCount > 0)
            Jump();
        
        //else if (_isClimp) 
    }

    private void updatePlayerState() {
        _isGrounded = checkGrounded();
        animator.SetBool("isGround", _isGrounded);

        float verticalVelocity = _rigidbody.velocity.y;
        animator.SetBool("isDown", verticalVelocity < 0);

        if (_isGrounded && verticalVelocity == 0)
        {
            animator.SetBool("isJump", false);
            animator.ResetTrigger("isJumpFirst");
            animator.ResetTrigger("isJumpSecond");
            animator.SetBool("isDown", false);

            jumpCount = 2;
            
            //_isSprintable = true;
        }

    }
    public void Jump() {

        Vector2 newVelocity;
        newVelocity.x = _rigidbody.velocity.x;
        newVelocity.y = JumpForce;

        _rigidbody.velocity = newVelocity;
        
        animator.SetBool("isJump",true);

        jumpCount -= 1;
        if (jumpCount == 1)
        {
            animator.SetTrigger("isJumpFirst");
        }
        else if (jumpCount == 0)
        {
            animator.SetTrigger("isJumpSecond");
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    jumpState = true;
        //    animator.SetBool("isJump", jumpState);
        //    _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        //    timeJump = 0;
        //}
        //else if (Input.GetKey(KeyCode.Space) && jumpState && jumpCount <= 2)
        //{
        //    timeJump += Time.deltaTime;
        //    if (timeJump < jumpTime)
        //    {
        //        _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    jumpState = false;
        //    animator.SetBool("isJump", jumpState);
        //    timeJump = 0;
        //}

    }

  
    private bool checkGrounded()
    {
        _isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) {
            _isGrounded = true;
        }
        return _isGrounded;
        //Vector2 origin = _transform.position;

        //float radius = 0.2f;

        //// detect downwards
        //Vector2 direction;
        //direction.x = 0;
        //direction.y = -1;

        //float distance = 0.5f;
        //LayerMask layerMask = LayerMask.GetMask("Platform");

        //RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        //return hitRec.collider != null;
    }


    private void fallControl()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isFalling = true;
            fall();
        }
        else
        {
            isFalling = false;
        }
    }

    private void move() {

        horizontalMove = Input.GetAxis("Horizontal") * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal") * movementSpeed));


        // 無速度且無輸入
        if (Mathf.Abs(horizontalMove) == 0)
        {
            animator.SetBool("isStop", true);
            moveDirection = transform.rotation.y;
        }
        else if (Mathf.Abs(horizontalMove) > 0.5f)
        {
            animator.SetBool("isStop", false);
        }


        //  var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(horizontalMove, 0, 0) * Time.deltaTime * movementSpeed;

        float Direction = -transform.localScale.x * horizontalMove;
        if (Direction < 0)
        {

            if (_isGrounded)
            {
                // turn back animation
                animator.SetTrigger("isRotate");
            }
        }

        if (!Mathf.Approximately(0, horizontalMove))
        {

            if (horizontalMove > 0)
            {

                transform.rotation = Quaternion.Euler(0, 180, 0);
                if (moveDirection != (-1))
                {

                    animator.SetBool("isTurn", true);
                }
                else
                {
                    animator.SetBool("isTurn", false);
                }
            }
            else
            {
                transform.rotation = Quaternion.identity;
                if (moveDirection != 0)
                {

                    animator.SetBool("isTurn", true);
                }
                else
                {
                    animator.SetBool("isTurn", false);
                }
            }

        }
    }
    private void fall()
    {
        Vector2 newVelocity;
        newVelocity.x = _rigidbody.velocity.x;
        newVelocity.y = -fallSpeed;

        _rigidbody.velocity = newVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerState();
        if (isInputEnabled)
        {
            move();
            JumpControl();
            fallControl();
        }

      
        //if (Input.GetButton("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f) 
        //{
        //    _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        //}
    }
}
