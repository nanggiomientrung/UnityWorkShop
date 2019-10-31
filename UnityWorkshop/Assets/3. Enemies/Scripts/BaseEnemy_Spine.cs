using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy_Spine : BaseEnemy
{
    [SerializeField] protected string moveAnim; // tên animation cho di chuyển
    [SerializeField] protected string idleAnim; // tên animation cho idle
    [SerializeField] protected string attackAnim; // tên animation cho động tác đánh
    [SerializeField] protected string fallAnim; // tên animation cho động tác ngã
    [SerializeField] protected string standUpAnim; // tên animation cho động tác đứng dậy
    [SerializeField] protected string deadAnim; // tên animation cho động tác bị chết

    [SerializeField] protected string attackEvent;
    [SerializeField] protected SkeletonAnimation data_ske;

    protected override void Start()
    {
        base.Start();
        data_ske.state.Complete += OnAnimationComplete;
        //data_ske.state.Event += HandleSpineEvent;
        data_ske.AnimationState.Event += HandleAnimEvent;
    }

    protected override void Update()
    {
        base.Update();
        transform.position += moveStep * Time.deltaTime; // di chuyển enemy


        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }


    }


    // anim with spine
    protected void SetAnimation(string animationName, bool isLoop)
    {
        if (animationName == idleAnim)
        {
            data_ske.Skeleton.SetToSetupPose();
        }

        if (animationName == moveAnim)
        {
            data_ske.timeScale = moveMultiplier;
            data_ske.state.SetAnimation(0, animationName, isLoop);
        }
        else
        {
            data_ske.timeScale = 1;
            data_ske.state.SetAnimation(0, animationName, isLoop);
        }

    }

    // action after finishing anim
    protected virtual void OnAnimationComplete(Spine.TrackEntry entry)
    {
        switch (currentAction)
        {
            case EnemyAction.Attack:
                // cho về idle để có thể tính toán lại action tiếp
                SetIdle();

                break;
            case EnemyAction.Hit:
                // cho về idle để có thể tính toán lại action tiếp
                SetIdle();
                // chắc chắn enemy ko thể màu bị đánh khi hết anim được
                materialBlock.SetColor("_Black", Color.black);
                meshRenderer.SetPropertyBlock(materialBlock);
                break;
            case EnemyAction.StandUp:
                // cho về idle để có thể tính toán lại action tiếp
                SetIdle();

                materialBlock.SetColor("_Black", Color.black);
                meshRenderer.SetPropertyBlock(materialBlock);
                break;
            case EnemyAction.Fall:
                // cho đứng dậy
                StandUp();

                materialBlock.SetColor("_Black", Color.black);
                meshRenderer.SetPropertyBlock(materialBlock);
                break;
            default:
                break;
        }

        // despawn enemy sau khi enemy chết
        if (string.CompareOrdinal(entry.animation.name, deadAnim) == 0)
        {
            DespawnOnDead();
        }
    }

    protected virtual void HandleAnimEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.CompareOrdinal(attackEvent, e.data.name) == 0)
        {
            StartCoroutine(EnableAttackBox());
        }
    }

    [SerializeField] private Enemy_AttackBox attackBox;
    IEnumerator EnableAttackBox()
    {
        attackBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackBox.gameObject.SetActive(false);
    }

    #region ACTION
    protected Vector3 moveStep = new Vector3(0f, 0f, 0f);
    [SerializeField] private float moveSpeed;
    [SerializeField] protected float moveMultiplier = 2f;
    protected virtual void MoveLeft()
    {
        currentAction = EnemyAction.MoveLeft;
        TurnLeft();
        moveStep.x = -moveSpeed * moveMultiplier;
        SetAnimation(moveAnim, true);
    }

    protected virtual void MoveRight()
    {
        currentAction = EnemyAction.MoveRight;
        TurnRight();
        moveStep.x = moveSpeed * moveMultiplier;
        SetAnimation(moveAnim, true);
    }
    protected virtual void SetIdle() // stop moving, cần gọi khi có OnAnimationComplete thì hay hơn
    {
        if (currentAction == EnemyAction.Idle) return;
        currentAction = EnemyAction.Idle;
        moveStep.x = 0f;
        data_ske.state.ClearTrack(0);
        SetAnimation(idleAnim, true);
    }

    protected virtual void Attack()
    {
        currentAction = EnemyAction.Attack;
        moveStep.x = 0f;
        data_ske.state.ClearTrack(0);
        SetAnimation(attackAnim, false);
    }

    protected virtual void StandUp()
    {
        currentAction = EnemyAction.StandUp;
        moveStep.x = 0f;
        //data_ske.state.ClearTrack(0);
        SetAnimation(standUpAnim, false);
    }

    protected virtual void DespawnOnDead()
    {
        SimplePool.Despawn(gameObject);

        //if (DeadEffect != null)
        //{
        //    temp = SimplePool.Spawn(DeadEffect);
        //    temp.transform.position = transform.position;
        //    temp.transform.parent = EffectContainer.Instance.transform;
        //}
        //if (Sound_OnDead != null)
        //    PlaySound(Sound_OnDead);
        //SimplePool.Despawn(gameObject);
    }

    protected void TurnLeft()
    {
        transform.localScale = Vector3.one;
    }

    protected Vector3 rightScale = new Vector3(-1f, 1f, 1f);// vector scale khi enemy quay sang phải
    protected void TurnRight()
    {
        transform.localScale = rightScale;
    }
    #endregion
}