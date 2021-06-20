using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : PatrollingEnemy, IAnimatable
{
    Stack<EnemyEnums.State> states = new Stack<EnemyEnums.State>();

    [SerializeField]
    private float _attackRangeX;
    [SerializeField]
    private float _attackRangeY;
    [SerializeField]
    private Transform _attackTransform;

    public LayerMask playerCoreLayer;

    public event Action<string, bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;

    private bool _isAlive = true;
    private bool _isHit = false;
    private bool _isIdle = false;
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
        hpSys = GetComponent<HealthSystem>();
        _detection = GetComponentInChildren<PlayerDetection>();
        _detection.OnPlayerDetected = SetPlayerInRange;
        _detection.OnPlayerLeft = SetPlayerNotInRange;
        hpSys.OnHit += HitEnemy;
    }
    protected override void SetUpBehaviour()
    {
        base.SetUpBehaviour();
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
                if(currentState== EnemyEnums.State.ATTACKING)
                {
                    Attack();
                }
                if(currentState==EnemyEnums.State.IDLE_AFTER_HIT)
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
        PlayAnimation("Idle");
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
    private void Attack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        PlayAnimation("Attack");
        StartCoroutine(WaitSomeTimeAndDoSmth(GetAnimationLength("Attack"), () => 
        { 
            PlayAnimation("Idle"); StartCoroutine(WaitSomeTimeAndDoSmth(_attackCooldown, () => { _isAttacking = false;  })); 
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

    private void HitEnemy()
    {
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

    public void StartCheckingForPlayerCol()
    {
        _isCheckingForPlayerCol = true;
        StartCoroutine(WaitSomeTimeAndDoSmth(GetAnimationLength("Attack"), StopCheckingForPlayerCol));
        currentCor = StartCoroutine(CheckForPlayerColliderCor());
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




    private void StopCurrentActions()
    {
        StopAllCoroutines();

        _isIdle = false;
        _isCheckingForPlayerCol = false;
        _isAttacking = false;
    }
    private void ResumeActions()
    {
        currentState = states.Pop();
        _isAttacking = false;
        _isIdle = false;
    }


    IEnumerator WaitSomeTimeAndDoSmth(float timeToWait, Action functionToPerform)
    {
        yield return new WaitForSeconds(timeToWait);
        functionToPerform();
    }



    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name, canBePlayedOver);
    }
    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_attackTransform.position, new Vector3(_attackRangeX, _attackRangeY));
    }
}
