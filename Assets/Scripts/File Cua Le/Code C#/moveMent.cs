//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class moveMent : MonoBehaviour
//{
//    public CharacterController2D controller;
//    public Animator animator;
//    public Rigidbody2D rb;  
//    private AudioManager audioManager;
//    public float runSpeed = 40f;

//    float horizontalMove = 0f;
//    bool jump = false;
//    bool crouch = false;
//    private bool isDashing;
//    private bool canDash = true;
//    [SerializeField] private float dashPower = 20f;
//    [SerializeField] private float dashingTime = 0.2f;
//    [SerializeField] private float dashingCoolDown = 0.5f;
//    // Update is called once per frame

//    private void Awake()
//    {
//        audioManager = FindAnyObjectByType<AudioManager>();
//    }
//    void Update()
//    {
//        if (isDashing)
//        {
//            return;
//        }
//        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
//        animator.SetFloat("Speed",Mathf.Abs(horizontalMove));

//        if (Input.GetKeyDown("w"))
//        {
//            canDash = false;
//            jump = true;
//            animator.SetBool("isJumping", true);
//            audioManager.PlayJumpSound();
//        }
//        if (Input.GetButtonDown("Crouch"))
//        {
//            crouch = true;
//        }
//        else if (Input.GetButtonUp("Crouch"))
//        {
//            crouch = false;
//        }
//        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash)
//        {
//            StartCoroutine(Dashing()); 
//        }
//    }
//    public void OnLanding()
//    {
//        animator.SetBool("isJumping", false);
//        canDash = true;
//    }
//    public void OnCrouching(bool isCrouching)
//    {
//        animator.SetBool("isCrouching",isCrouching);
//    }
//    public IEnumerator Dashing()
//    {
//        animator.SetTrigger("isRolling");
//        canDash = false;
//        isDashing = true;
//        float originalGravity = rb.gravityScale;
//        rb.gravityScale = 0f;
//        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
//        yield return new WaitForSeconds(dashingTime);
//        isDashing = false;
//        rb.gravityScale = originalGravity;
//        yield return new WaitForSeconds(dashingCoolDown);
//        canDash = true;
//    }
//    void FixedUpdate()
//    {
//        // Move our character
//        if (isDashing)
//        {
//            return;
//        }
//        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
//        jump = false;
//    }
//}
