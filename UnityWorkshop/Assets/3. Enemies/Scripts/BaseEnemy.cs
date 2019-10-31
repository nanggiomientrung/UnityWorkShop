using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IActor
{
    [SerializeField] private float enemyHealth; // máu của enemy
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private int killedScore; // điểm khi giết dc enemy này
    private float currentHealth;
    [SerializeField]protected float enemyDamage;

    // biến cho đổi màu khi bị đánh
    [SerializeField] private bool isFrameByFrame = true; // animation của enemy là frame by frame hay Spine (Skeleton)
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected MaterialPropertyBlock materialBlock;
    [SerializeField]protected MeshRenderer meshRenderer;
    private Color hitColor = Color.white;
    private Color normalColor = Color.black;
    [SerializeField] private float hitDuration;
    private float timer;

    // trạng thái của enemy
    protected EnemyAction currentAction;

    protected virtual void Start()
    {
        materialBlock = new MaterialPropertyBlock();
    }

    protected virtual void OnEnable()
    {
        currentHealth = enemyHealth;

        currentAction = EnemyAction.Idle;

        // set màu về ban đầu 
        if(isFrameByFrame)
        {
            hitColor = Color.red;
            normalColor = Color.white;
            try
            {
                normalColor.g = 1f;
                normalColor.b = 1f;
                spriteRenderer.color = normalColor;
            }
            catch { }
        }
        else
        {
            hitColor = Color.white;
            normalColor = Color.black;
            try
            {
                normalColor.a = 1f;
                materialBlock.SetColor("_Color", Color.white);
                materialBlock.SetColor("_Black", normalColor);
                meshRenderer.SetPropertyBlock(materialBlock);
            }
            catch { }
        }
    }

    protected virtual void Update()
    {
        // đổi màu khi bị đánh
        if (currentAction == EnemyAction.Hit)
        {
            timer += Time.deltaTime;
            if(isFrameByFrame)
            {
                if (timer <= hitDuration / 2)
                {
                    hitColor.g = 1 - timer / hitDuration*2;
                    hitColor.b = 1 - timer / hitDuration*2;
                    spriteRenderer.color = hitColor;
                }
                else
                {
                    if (timer > hitDuration)
                    {
                        timer = hitDuration;
                        currentAction = EnemyAction.Idle;
                    }

                    hitColor.g = timer / hitDuration * 2 - 1;
                    hitColor.b = timer / hitDuration * 2 - 1;
                    spriteRenderer.color = hitColor;
                }
            }
            else
            {
                if (timer <= hitDuration / 2)
                {
                    // materialBlock.SetColor("_Color", Color.black); // với shader này thì ko dùng _Color mà dùng _Black
                    hitColor.a = timer / hitDuration;
                    materialBlock.SetColor("_Black", hitColor);
                    meshRenderer.SetPropertyBlock(materialBlock);
                }
                else
                {
                    if (timer > hitDuration)
                    {
                        timer = hitDuration;
                        currentAction = EnemyAction.Idle;
                    }

                    normalColor.a = timer / hitDuration;
                    materialBlock.SetColor("_Black", normalColor);
                    meshRenderer.SetPropertyBlock(materialBlock);
                }
            }
        }
    }

    public void OnDeath()
    {
        currentAction = EnemyAction.Dead;
        GameManager.instance.IncreaseScore(killedScore);
        GameObject.Destroy(gameObject); // dùng SimplePool.Despawn nếu như sinh quái liên tục theo di chuyển của người chơi
    }

    public void TakeDamage(float Damage)
    {
        Debug.LogError("Take Damage");
        timer = 0; // set cho update màu
        currentAction = EnemyAction.Hit;
        currentHealth -= Damage;
        if(currentHealth <=0)
        {
            OnDeath();
        }
    }
}

public enum EnemyAction
{
    OnSpawning, // đang spawn, ko action j
    Idle,
    MoveLeft,
    MoveRight,
    Attack,
    Hit,
    Dead,
    SpecialAction_1,
    SpecialAction_2,
    Fall,
    StandUp
}