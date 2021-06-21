using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : PatrollingEnemy
{
    

    [SerializeField]
    private float _attackRangeX;
    [SerializeField]
    private float _attackRangeY;
    [SerializeField]
    private Transform _attackTransform;

    public LayerMask playerCoreLayer;

    
    private bool _isCheckingForPlayerCol;
    private bool _isAttacking;

    [SerializeField]
    private float _attackCooldown;

    private PlayerDetection _detection;

    private Collider2D _hitCollider;

    // Start is called before the first frame update
    void Start()
    {
        SetUpBehaviour();
        SetUpComponents();
    }
    protected override void SetUpComponents()
    {
        base.SetUpComponents();
        _detection = GetComponentInChildren<PlayerDetection>();
        _detection.OnPlayerDetected = SetPlayerInRange;
        _detection.OnPlayerLeft = SetPlayerNotInRange;
        hpSys.OnHit += Hit;
        hpSys.OnDeath = Kill;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isHit)
            {
                if (currentState == EnemyEnums.State.PATROLLING)
                {
                    PlayAnimation("Move");
                    MoveToPatrolPoint();
                }
                if ( currentState == EnemyEnums.State.ALWAYS_IDLE)
                {
                    if (!_isIdle)  StartCoroutine(StayIdleCor());
                }
                if(currentState == EnemyEnums.State.IDLE_AT_PATROL_POINT)
                {
                    if (!_isIdle)  StayIdleAtPatrolPoint();
                }
                if (currentState == EnemyEnums.State.ATTACKING)
                {
                    Attack();
                }
                if(currentState==EnemyEnums.State.IDLE_AFTER_HIT)
                {
                    if (!_isIdle) StayIdleAfterHit();

                }
            }
        }
    }



    private void StayIdleAfterHit()
    {
        StartCoroutine(StayIdleCor());
        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Idle"), ResumeActions));
    }

    private void Attack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        PlayAnimation("Attack");
        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Attack"), () => 
        { 
            PlayAnimation("Idle"); StartCoroutine(WaitAndExecuteFunction(_attackCooldown, () => { _isAttacking = false;  })); 
        }));
    }
    
    IEnumerator CheckForPlayerColliderCor()
    {
        while(_isCheckingForPlayerCol)
        {
            _hitCollider = Physics2D.OverlapBox(_attackTransform.position, new Vector2(_attackRangeX, _attackRangeY), 0, playerCoreLayer);
            if (_hitCollider != null)
            {
                IDamagable tmp = _hitCollider.GetComponentInParent<IDamagable>();
                if (tmp != null)
                {
                    tmp.TakeDamage(dmg);
                }
            }
            yield return null;
        }
        yield return null;
    }
    public void StartCheckingForPlayerCol()
    {
        _isCheckingForPlayerCol = true;
        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Attack"), StopCheckingForPlayerCol));
        StartCoroutine(CheckForPlayerColliderCor());
    }
    public void StopCheckingForPlayerCol()
    {
        _isCheckingForPlayerCol = false;
    }


    protected override void SetPlayerInRange()
    {
        states.Push(currentState);
        currentState = EnemyEnums.State.ATTACKING;
        StopCurrentActions();
        Attack();
    }
    protected override void SetPlayerNotInRange()
    {
        ResumeActions();
    }


    protected override void StopCurrentActions()
    {
        base.StopCurrentActions();
        _isIdle = false;
        _isCheckingForPlayerCol = false;
        _isAttacking = false;
    }

    protected override void ResumeActions()
    {
        base.ResumeActions();
        _isAttacking = false;
        _isIdle = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_attackTransform.position, new Vector3(_attackRangeX, _attackRangeY));
    }
}
