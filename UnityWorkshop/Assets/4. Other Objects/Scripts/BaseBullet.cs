using UnityEngine;
using DG.Tweening;

public class BaseBullet : MonoBehaviour
{
    protected float bulletDamage;
    [SerializeField] protected string hitTag;
    private BulletController bulletController;
    Tween moveTween;

    public void SetBulletDamage(float BulletDamage)
    {
        bulletDamage = BulletDamage;
    }

    public void BulletController(BulletController BulletController)
    {
        bulletController = BulletController;
    }

    public void SetBulletVelocity(Vector3 BulletVelocity)
    {
        if (bulletController != null)
        {
            float rotateAmount = Vector3.SignedAngle(Vector3.right,BulletVelocity, Vector3.forward);
            transform.localEulerAngles = new Vector3(0,0,rotateAmount);
        }
            
        moveTween = transform.DOLocalMove(BulletVelocity, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Ground"))
        {
            if (bulletController != null) bulletController.OnBulletDespawn();
            if (moveTween != null) moveTween.Kill();
            SimplePool.Despawn(gameObject);
        }
        else if (coll.CompareTag(hitTag))
        {
            IActor health = coll.gameObject.GetComponent<IActor>();
            if(health != null)
            {
                health.TakeDamage(bulletDamage);
            }
            if (bulletController != null) bulletController.OnBulletDespawn();
            if (moveTween != null) moveTween.Kill();
            SimplePool.Despawn(gameObject);
        }
    }

    private Vector3 lastPosition;
    private void OnEnable()
    {
        lastPosition = transform.position;
    }
    private void Update()
    {
        if (bulletController != null) return;
        if (lastPosition != transform.position)
        {
            transform.localEulerAngles = new Vector3(0, 0, Vector3.SignedAngle(transform.localScale.x == 1 ? Vector3.right : Vector3.left, transform.position - lastPosition, Vector3.forward));
        }
        lastPosition = transform.position;
    }
}
