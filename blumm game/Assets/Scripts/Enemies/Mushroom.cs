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
   


    private Coroutine currentCor = null;

    // Start is called before the first frame update
    void Start()
    {
        SetUpComponents();
        SetUpBehaviour();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isHit)
            {
                if (currentState == PatrolState.PATROLLING)
                {
                    PlayAnimation("Move");
                    MoveToPatrolPoint();
                }
                if (currentState == PatrolState.IDLE_AT_PATROL_POINT || currentState == PatrolState.ALWAYS_IDLE)
                {
                   currentCor= StartCoroutine(IdleTimerCor(idleCycles));
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void KillEnemy()
    {
        _isHit = true;
        StopCurrentActions();
        PlayAnimation("Death");
        StartCoroutine(WaitForAnimationToEnd(GetAnimationLength("Death"), (result) => _isAlive = result, _isAlive));
    }

    private void HitEnemy()
    {
        _isHit = true;
        StopCurrentActions();
        hpSys.isInvincible = true;
        StartCoroutine(EnemyHitCor());
    }
    private void StopCurrentActions()
    {
       if(currentCor!=null) StopCoroutine(currentCor);
    }

    protected override void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeath += KillEnemy;
        hpSys.OnHit += HitEnemy;
    }
    IEnumerator IdleTimerCor(int numbeOfIdleCycles)
    {
        if(_isIdle) yield break ;
        else _isIdle = true;
        PlayAnimation("Idle");
        yield return new WaitForSeconds(numbeOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
        if (currentState == PatrolState.IDLE_AT_PATROL_POINT)
        {
            currentState = PatrolState.PATROLLING;
            RotateEnemyTowardsNextPatrolPoint();
        }
    }
    IEnumerator WaitForAnimationToEnd(float animationLength, Action<bool> myVariableLambda, bool currentValue)
    {
        yield return new WaitForSeconds(animationLength);
        myVariableLambda(!currentValue);
    }

    IEnumerator EnemyHitCor()
    {
        PlayAnimation("Hit");
        yield return new WaitForSeconds(GetAnimationLength("Hit"));
        hpSys.isInvincible = false;
        if (currentState==PatrolState.PATROLLING)
        {
            PlayAnimation("Idle");
            yield return new WaitForSeconds(GetAnimationLength("Idle"));
        }
        _isHit = false;
    }

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name, canBePlayedOver);
    }

    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
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
    //private void OnTriggerEnter2D(Collision2D collision)
    //{

    //}
}
