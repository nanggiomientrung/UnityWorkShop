using DG.Tweening;
using UnityEngine;

public class Enemy_Frog : BaseEnemy
{
    protected override void OnEnable()
    {
        base.OnEnable();
        currentAction = EnemyAction.Attack;
        Jump();
    }

    private float initPosX = 9;
    public void SetInitPosX(float InitPosX)
    {
        initPosX = InitPosX;
    }


    private void Jump()
    {
        LeftJump();
    }

    private void LeftJump()
    {
        transform.DOMoveX(initPosX - 5, 1).SetEase(Ease.Linear).OnComplete(FlipEnemy);
        transform.DOMoveY(0, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    private void RightJump()
    {
        transform.DOMoveX(initPosX, 1).SetEase(Ease.Linear).OnComplete(FlipEnemy);
        transform.DOMoveY(0, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    Vector3 leftScale = new Vector3(-1, 1, 1);
    private void FlipEnemy()
    {
        if (transform.localScale.x == 1)
        {
            transform.localScale = leftScale;
            RightJump();
            Shoot();
        }
        else
        {
            transform.localScale = Vector3.one;
            LeftJump();
            Shoot();
        }
    }

    // mỗi khi con ếch tiếp đất thì nó sẽ lại bắn đạn ra xung quanh
    private Vector3 spawnOffset = new Vector3(0, 0.25f, 0f);
    [SerializeField] private BaseBullet bulletPrefab_Gravity;
    private BaseBullet currentBullet;
    [SerializeField] private Vector3[] bulletShootDirection_Parabol;
    [SerializeField] private float bulletInitialVelocity;
    private void Shoot()
    {
        for (int i = 0; i < bulletShootDirection_Parabol.Length; i++)
        {
            currentBullet = SimplePool.Spawn(bulletPrefab_Gravity, transform.position + spawnOffset, Quaternion.identity);
            currentBullet.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(bulletShootDirection_Parabol[i]) * bulletInitialVelocity;
            currentBullet.transform.localScale = bulletShootDirection_Parabol[i].x > 0 ? Vector3.one : leftScale;
            currentBullet.transform.localEulerAngles = new Vector3(0, 0, Vector3.SignedAngle(currentBullet.transform.localScale.x == 1 ? Vector3.right : Vector3.left, bulletShootDirection_Parabol[i], Vector3.forward));
            currentBullet.SetBulletDamage(enemyDamage);
        }
    }
}
