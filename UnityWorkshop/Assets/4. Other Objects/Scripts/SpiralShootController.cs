using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpiralShootController : BulletController
{
    private Vector3 rotateSpeed = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    Tween rotateTween;
    public void ShootAsSpiral(int NumberBulletPerWave, float SpiralVelocity, float SpiralRotateSpeed, float BulletDamage)
    {
        for (int i = 0; i < NumberBulletPerWave; i++)
        {
            direction.x = Mathf.Cos(2 * Mathf.PI / NumberBulletPerWave * i);
            direction.y = Mathf.Sin(2 * Mathf.PI / NumberBulletPerWave * i);
            InitBullet(transform.position, direction, SpiralVelocity, BulletDamage);
        }
        rotateSpeed.z = SpiralRotateSpeed;
        rotateTween = transform.DORotate(rotateSpeed, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    public override void OnBulletDespawn()
    {
        currentBulletQuantity--;
        if (currentBulletQuantity <= 0)
        {
            rotateTween.Kill();
            SimplePool.Despawn(gameObject);
        }
    }
}
