using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletController : MonoBehaviour
{
    [SerializeField] protected BaseBullet bulletPrefab;
    private BaseBullet currentBullet;
    protected int currentBulletQuantity;
    protected void InitBullet(Vector3 shootPosition, Vector3 shootDirection, float bulletVelocity, float bulletDamage)
    {
        currentBullet = SimplePool.Spawn(bulletPrefab, shootPosition, Quaternion.identity);
        currentBullet.BulletController(this);
        currentBullet.transform.SetParent(transform);
        currentBullet.SetBulletVelocity(Vector3.Normalize(shootDirection) * bulletVelocity);
        currentBullet.SetBulletDamage(bulletDamage);
        currentBulletQuantity++;
    }
    
    public virtual void OnBulletDespawn()
    {
        currentBulletQuantity--;
        if (currentBulletQuantity <= 0)
        {
            SimplePool.Despawn(gameObject);
        }
    }
}
