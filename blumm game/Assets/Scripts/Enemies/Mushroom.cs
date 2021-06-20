using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : PatrollingEnemy, IAnimatable
{

    public event Action<string,bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;

    private bool _isAlive = true;
    private bool _isHit = false;
    private bool _isIdle = false;
   

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
                if (currentState == EnemyEnums.State.IDLE_AT_PATROL_POINT || currentState == EnemyEnums.State.ALWAYS_IDLE)
                {
                   StartCoroutine(IdleTimerCor(idleCycles));
                }
                if(currentState == EnemyEnums.State.IDLE_AFTER_HIT)
                {
                    StayIdleAfterHit();
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator IdleTimerCor(int numbeOfIdleCycles)
    {
        if (_isIdle) yield break;
        else _isIdle = true;
        if(numbeOfIdleCycles>0) PlayAnimation("Idle");
        yield return new WaitForSeconds(numbeOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
        if (currentState == EnemyEnums.State.IDLE_AT_PATROL_POINT)
        {
            currentState = EnemyEnums.State.PATROLLING;
            RotateEnemyTowardsNextPatrolPoint();
        }
    }
    private void StayIdleAfterHit()
    {
        if (_isIdle) return;
        _isIdle = true;
        PlayAnimation("Idle");
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
        Debug.Log("hit");
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


    //IEnumerator EnemyHitCor()
    //{
    //    PlayAnimation("Hit");
    //    yield return new WaitForSeconds(GetAnimationLength("Hit"));
    //    hpSys.isInvincible = false;
    //    if (currentState== EnemyEnums.State.PATROLLING)
    //    {
    //        PlayAnimation("Idle");
    //        yield return new WaitForSeconds(GetAnimationLength("Idle"));
    //    }
    //    _isHit = false;
    //}

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name, canBePlayedOver);
    }

    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }


    IEnumerator WaitSomeTimeAndDoSmth(float timeToWait, Action functionToPerform)
    {
        yield return new WaitForSeconds(timeToWait);
        functionToPerform();
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
