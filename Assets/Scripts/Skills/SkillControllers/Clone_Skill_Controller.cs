using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    [SerializeField] private float colorLoosingSpeed;
    private Animator anim;
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;
    private bool canDuplicateClone;

    private float chanceToDuplicate;
    private int facingDir = 1;

    

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _newTransform, float cloneDuration, bool canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate, Player _player)
    {
        if (canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = cloneDuration;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if (canDuplicateClone)
                {
                    if (Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
