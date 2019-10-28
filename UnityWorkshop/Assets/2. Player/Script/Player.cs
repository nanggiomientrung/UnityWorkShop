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
    private float currentHealth;
    

    // thông số nhân vật
    [SerializeField] private float playerDamage;

    private void OnEnable()
    {
        currentHealth = playerHealth;
    }

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
        if (isPlayerOnGround == false) return;
        // cần cơ chế ko cho nhảy khi đang ở trên không
        animator.SetTrigger("Jump");
        rigidBody.AddForce(jumpForce);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        //DirectShoot();
        ParabolShoot();
        //SpiralShoot();
        //Shoot(isMovingRightDirection ? rightShootDirection : leftShootDirection,
        //    isMovingRightDirection ? Vector3.zero : new Vector3(0, 0, 180), bulletInitialVelocity, bulletPrefab);
        //Shoot(isMovingRightDirection ? rightShootDirection_Parabol : leftShootDirection_Parabol,
        //    isMovingRightDirection ? Vector3.zero : new Vector3(0, 0, 180), bulletInitialVelocity, bulletPrefab_Gravity);
    }

    private Vector3 leftScale = new Vector3(-1f, 1f, 1f); // để lật player lại cho quay sang phía trái
    
    private void Move(bool isRight)
    {
        // xoay player tùy theo di chuyển trái hay phải
        transform.localScale = isRight ? Vector3.one : leftScale;// if(isRight) transform.localScale = vector3.one else transform.localscale = leftscale
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

    public void TakeDamage(float xDamage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= xDamage;
        if (playerHealth <= 0) OnDeath();
    }

    public void OnDeath()
    {
        GameManager.instance.PlayerDeath();
    }

    private bool isPlayerOnGround = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CheckGround"))
        {
            Debug.Log("Tiep dat");
            isPlayerOnGround = true;
        }


        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<IActor>().TakeDamage(100);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckGround"))
        {
            Debug.Log("Nhay");
            isPlayerOnGround = false;
        }
    }

    #region BULLET SHOOTING
    // đạn
    [SerializeField] private BaseBullet bulletPrefab;
    [SerializeField] private Vector2 leftShootDirection;
    [SerializeField] private Vector2 rightShootDirection;
    [SerializeField] private float bulletInitialVelocity;
    [SerializeField] private Transform shootTransform;
    private BaseBullet currentBullet;
    private void DirectShoot()
    {
        currentBullet = SimplePool.Spawn(bulletPrefab, shootTransform.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(leftShootDirection) * bulletInitialVelocity;
        if (isMovingRightDirection)
        {
            //currentBullet = SimplePool.Spawn(bulletPrefab, shootTransform.position, Quaternion.identity);
            //currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(rightShootDirection) * bulletInitialVelocity;
            currentBullet.transform.localScale = Vector3.one;
        }
        else
        {
            //currentBullet = SimplePool.Spawn(bulletPrefab, shootTransform.position, Quaternion.identity);
            //currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(leftShootDirection) * bulletInitialVelocity;
            currentBullet.transform.localScale = leftScale;
        }
        currentBullet.SetBulletDamage(playerDamage);
    }

    [SerializeField] private BaseBullet bulletPrefab_Gravity;
    [SerializeField] private Vector2 leftShootDirection_Parabol;
    [SerializeField] private Vector2 rightShootDirection_Parabol;
    private void ParabolShoot()
    {
        if (isMovingRightDirection)
        {
            currentBullet = SimplePool.Spawn(bulletPrefab_Gravity, shootTransform.position, Quaternion.identity);
            currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(rightShootDirection_Parabol) * bulletInitialVelocity;
            currentBullet.transform.localScale = Vector3.one;
            //currentBullet.transform.localEulerAngles
        }
        else
        {
            currentBullet = SimplePool.Spawn(bulletPrefab_Gravity, shootTransform.position, Quaternion.identity);
            currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(leftShootDirection_Parabol) * bulletInitialVelocity;
            currentBullet.transform.localScale = leftScale;
        }
        currentBullet.SetBulletDamage(playerDamage);
    }

    private void Shoot(Vector3 ShootDirection, Vector3 RotateAngle, float Velocity, BaseBullet BulletPrefab)
    {
        currentBullet = SimplePool.Spawn(BulletPrefab, shootTransform.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(ShootDirection) * Velocity;
        currentBullet.transform.localEulerAngles = RotateAngle;
        currentBullet.SetBulletDamage(playerDamage);
    }

    [SerializeField] private SpiralShootController bulletControllerPrefab;
    private SpiralShootController currentControllerPrefab;
    private int NumberBulletPerWave = 5;
    private float SpiralVelocity = 5;
    private float SpiralRotateSpeed = 30;
    private void SpiralShoot()
    {
        currentControllerPrefab = SimplePool.Spawn(bulletControllerPrefab, shootTransform.position, Quaternion.identity);
        currentControllerPrefab.ShootAsSpiral(NumberBulletPerWave, SpiralVelocity, SpiralRotateSpeed, playerDamage);
    }
    #endregion
}
