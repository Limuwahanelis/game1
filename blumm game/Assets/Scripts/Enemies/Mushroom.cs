using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : PatrollingEnemy, IAnimatable
{

    public event Action<string,bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

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
                if (currentState == State.PATROLLING)
                {
                    PlayAnimation("Move");
                    MoveToPatrolPoint();
                }
                if (currentState == State.IDLE_AT_PATROL_POINT || currentState == State.ALWAYS_IDLE)
                {
                    StartCoroutine(IdleTimerCor(idleCycles));
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

    private void HitEnemy(GameObject dmgSource)
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

    private void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeathEvent += KillEnemy;
        hpSys.OnHitEvent += HitEnemy;
    }
    IEnumerator IdleTimerCor(int numbeOfIdleCycles)
    {
        if(_isIdle) yield break ;
        else _isIdle = true;
        PlayAnimation("Idle");
        yield return new WaitForSeconds(numbeOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
        if (currentState == State.IDLE_AT_PATROL_POINT)
        {
            currentState = State.PATROLLING;
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
        if (currentState==State.PATROLLING)
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
        if (tmp != null)
        {
            tmp.TakeDamage(dmg,gameObject);
        }
    }
    //private void OnTriggerEnter2D(Collision2D collision)
    //{

    //}
}
