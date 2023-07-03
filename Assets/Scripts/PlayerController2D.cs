using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    //movement & jumping
    private float moveHorizontal;
    private float speed = 8f;
    private float jumpPower = 26f;
    private bool isFacingRight = true;
    private int extraJumps;

    //dashing
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    //wall slide
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    //wall jumping
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 26f);

    //moving platform
    public bool isOnPlatform;
    public Rigidbody2D platformRigidBody;

    //audiomanager
    private AudioManager audioManager;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform sprite;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        extraJumps = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            if (extraJumps <= 0)
                extraJumps = 2;

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
        }

        if(Input.GetButtonDown("Jump") && extraJumps > 0 && !IsGrounded())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
            extraJumps--;
        }

        if(Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0f)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            audioManager.PlaySFX(audioManager.dash);
            StartCoroutine(Dash());
        }

        //rotate the cube on the Z Axis
        if (!IsGrounded() || !IsWalled())
        {
            sprite.Rotate(Vector3.back, 452f * Time.deltaTime);
        }

        //make sure the cube lands flat on the ground or wall
        if (IsGrounded() || IsWalled())
        {
            Vector3 rotation = sprite.rotation.eulerAngles;
            rotation.z = Mathf.Round(rotation.z / 90) * 90;
            sprite.rotation = Quaternion.Euler(rotation);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        if (!isWallJumping && isOnPlatform)
        {
            rigidBody.velocity = new Vector2(moveHorizontal * speed + platformRigidBody.velocity.x, rigidBody.velocity.y);
        }
        else if (!isWallJumping && !isOnPlatform)
        {
            rigidBody.velocity = new Vector2(moveHorizontal * speed, rigidBody.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(1.1f, 0.2f), 0, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 1.1f), 0, wallLayer);
    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && moveHorizontal != 0f)
        {
            isWallSliding = true;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rigidBody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJump), wallJumpingDuration);
        }
    }

    private void StopWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        //facing left or right if pressing the left or right key respectively
        if(isFacingRight && moveHorizontal < 0f || !isFacingRight && moveHorizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        trailRenderer.emitting = false;
        rigidBody.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Reset()
    {
        isFacingRight = true;
    }
}