using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeed : MonoBehaviour
{
    public Animator animator;
    public float movementSpeed = 1.0f ;
    public float JumpForce = 1.0f;

    public float jumpTime;　//跳躍的最最大蓄力時間
    public float timeJump; //跳躍當前蓄力時間
    public bool jumpState;
    public int jumpCount;
    public bool grounded;
    public bool isDoublejump;
    public bool _isGrounded;
    public float moveDirection;

    float horizontalMove = 0f;


    private Rigidbody2D _rigidbody;
   
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    public void Jump() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpState = true;
            animator.SetBool("isJump", jumpState);
            _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            timeJump = 0;
        }
        else if (Input.GetKey(KeyCode.Space) && jumpState && jumpCount<=2)
        {
            timeJump += Time.deltaTime;
            if (timeJump < jumpTime)
            {
                _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            jumpState = false;
            animator.SetBool("isJump", jumpState);
            timeJump = 0;
        }
    }
    private bool checkGrounded()
    {
        Vector2 origin = transform.position;

        float radius = 0.2f;

        // detect downwards
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 0.5f;
        LayerMask layerMask = LayerMask.GetMask("Platform");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitRec.collider != null;
    }



    // Update is called once per frame
    void Update()
    {
        _isGrounded = checkGrounded();
        horizontalMove = Input.GetAxis("Horizontal") * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal") * movementSpeed));

        //Vector2 newVelocity;
        //newVelocity.x = horizontalMove;
        //newVelocity.y = _rigidbody.velocity.y;
        //_rigidbody.velocity = newVelocity;

        //float moveDirection = -transform.localScale.x * horizontalMove;

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
        if (Direction < 0) {

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
                else {
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

        Jump();
        //if (Input.GetButton("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f) 
        //{
        //    _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        //}
    }
}
