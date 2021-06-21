using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : PatrollingEnemy, IAnimatable
{

    
    
   

    // Start is called before the first frame update
    void Start()
    {
        SetUpComponents();
        SetUpBehaviour();
    }

    protected override void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeath += Kill;
        hpSys.OnHit += Hit;
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
    }
    private void StayIdleAfterHit()
    {
        StartCoroutine(StayIdleCor());
        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Idle"), ResumeActions));
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
}
