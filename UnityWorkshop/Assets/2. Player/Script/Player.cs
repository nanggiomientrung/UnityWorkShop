using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IActor
{
    [SerializeField] private Animator animator;
    [SerializeField] private float playerHealth;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Vector3 moveSpeed;
    private bool isPlayerMoving = false;
    private bool isMovingRightDirection = true;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // JUMP
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            // ATTACK
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            // HURT
            //animator.SetTrigger("Hurt");
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(false);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopMoving(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(true);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopMoving(true);
        }

        if(isPlayerMoving)
        {
            if(isMovingRightDirection)
            {
                transform.position += moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position -= moveSpeed * Time.deltaTime;
            }
        }
    }

    private Vector3 jumpForce = new Vector3(0, 500, 0);
    private void Jump()
    {
        // cần cơ chế ko cho nhảy khi đang ở trên không
        animator.SetTrigger("Jump");
        rigidBody.AddForce(jumpForce);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private Vector3 leftScale = new Vector3(-1f, 1f, 1f); // để lật player lại cho quay sang phía trái
    
    private void Move(bool isRight)
    {
        // xoay player tùy theo di chuyển trái hay phải
        transform.localScale = isRight ? Vector3.one : leftScale;
        // set anim cho player
        animator.SetBool("Move", true);
        isMovingRightDirection = isRight;
        isPlayerMoving = true;
    }

    private void StopMoving(bool isRight)
    {
        // nếu user đang di chuyển cùng chiều Stop
        if (isMovingRightDirection == isRight)
        {
            animator.SetBool("Move", false);
            isPlayerMoving = false;
        }
    }

    public void TakeDamage(float Damage)
    {
        animator.SetTrigger("Hurt");
        playerHealth -= Damage;
        if (playerHealth <= 0) OnDeath();
    }

    public void OnDeath()
    {
        GameManager.instance.PlayerDeath();
    }
}
