using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : PatrollingEnemy, IAnimatable
{

    
    private bool _isHit = false;
   

    // Start is called before the first frame update
    void Start()
    {
        SetUpComponents();
        SetUpBehaviour();
    }

    protected override void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeath += KillEnemy;
        hpSys.OnHit += HitEnemy;
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
                if (currentState == EnemyEnums.State.IDLE_AT_PATROL_POINT)
                {
                   if(!_isIdle) StayIdleAtPatrolPoint();
                }
                if(currentState==EnemyEnums.State.ALWAYS_IDLE)
                {
                    if(!_isIdle) StartCoroutine(StayIdleCor());
                }
                if(currentState == EnemyEnums.State.IDLE_AFTER_HIT)
                {
                    if (!_isIdle) StayIdleAfterHit();
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void StayIdleAfterHit()
    {
        StartCoroutine(StayIdleCor());
        StartCoroutine(WaitSomeTimeAndDoSmth(GetAnimationLength("Idle"), ResumeActions));
    }
    private void KillEnemy()
    {
        _isHit = true;
        StopCurrentActions();
        PlayAnimation("Death");
        StartCoroutine(WaitSomeTimeAndDoSmth(GetAnimationLength("Death"), () => _isAlive = false));
    }

    private void HitEnemy()
    {
        StopCurrentActions();
        states.Push(currentState);
        _isHit = true;
        PlayAnimation("Hit");
        StartCoroutine(WaitSomeTimeAndDoSmth(GetAnimationLength("Hit"), () =>
        {
            states.Push(EnemyEnums.State.IDLE_AFTER_HIT);
            _isHit = false;
            ResumeActions();
        }));
    }

    protected override void StopCurrentActions()
    {
        base.StopCurrentActions();
        _isIdle = false;
    }

    protected override void ResumeActions()
    {
        base.ResumeActions();
        _isIdle = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable tmp = collision.gameObject.GetComponentInParent<IDamagable>();
        IPushable tmp2 = collision.gameObject.GetComponentInParent<IPushable>();
        
        if (tmp != null)
        {
            tmp.TakeDamage(collisionDmg);
            
        }
        if (tmp2 != null) tmp2.Push(gameObject);
    }
}
