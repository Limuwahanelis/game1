using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : PatrollingEnemy, IAnimatable
{
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

    private EnemyStates.State _previousState;

    private bool _isAlive = true;
    private bool _isHit = false;
    private bool _isIdle = false;
    private bool _isCheckingForPlayerCol;
    private bool _isAttacking;

    [SerializeField]
    private float _attackCooldown;
    private Coroutine currentCor = null;

    private PlayerDetection _detection;

    private Collider2D _hitCollider;

    // Start is called before the first frame update
    void Start()
    {
        
        SetUpBehaviour();
        SetUpComponents();
    }

    protected override void SetUpBehaviour()
    {
        base.SetUpBehaviour();
        _previousState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isHit)
            {
                if (currentState == EnemyStates.State.PATROLLING)
                {
                    PlayAnimation("Move");
                    MoveToPatrolPoint();
                }
                if (currentState == EnemyStates.State.IDLE_AT_PATROL_POINT || currentState == EnemyStates.State.ALWAYS_IDLE)
                {
                    currentCor = StartCoroutine(IdleTimerCor(idleCycles));
                }
                if(currentState== EnemyStates.State.ATTACKING)
                {
                    Attack();
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_attackTransform.position, new Vector3(_attackRangeX, _attackRangeY));
    }
    IEnumerator IdleTimerCor(int numbeOfIdleCycles)
    {
        if (_isIdle) yield break;
        else _isIdle = true;
        PlayAnimation("Idle");
        yield return new WaitForSeconds(numbeOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
        if (currentState == EnemyStates.State.IDLE_AT_PATROL_POINT)
        {
            currentState = EnemyStates.State.PATROLLING;
            RotateEnemyTowardsNextPatrolPoint();
        }
    }
    protected override void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
        _detection = GetComponentInChildren<PlayerDetection>();
        _detection.OnPlayerDetected = SetPlayerInRange;
        _detection.OnPlayerLeft = SetPlayerNotInRange;
    }

    private void Attack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        PlayAnimation("Attack");
        StartCoroutine(WaitSomeTimeAndDoSmth(GetAnimationLength("Attack"), () => 
        { 
            PlayAnimation("Idle"); StartCoroutine(WaitSomeTimeAndDoSmth(_attackCooldown, () => { _isAttacking = false; })); 
        }));
    }
    
    public void StartCheckingForPlayerCol()
    {
        _isCheckingForPlayerCol = true;
        StartCoroutine( WaitSomeTimeAndDoSmth(GetAnimationLength("Attack"), StopCheckingForPlayerCol));
        StartCoroutine(CheckForPlayerColliderCor());
    }
    public void StopCheckingForPlayerCol()
    {
        _isCheckingForPlayerCol = false;
        //StartCoroutine(WaitSomeTimeAndDoSmth(_attackCooldown, StopAttacking));

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

    IEnumerator WaitSomeTimeAndDoSmth(float timeToWait,Action functionToPerform)
    {
        yield return new WaitForSeconds(timeToWait);
        functionToPerform();
    }
    protected override void SetPlayerInRange()
    {
        StopCurrentActions();
        Attack();
    }
    protected override void SetPlayerNotInRange()
    {
        ResumeActions();
    }
    private void StopCurrentActions()
    {
        if (currentCor != null) StopCoroutine(currentCor);
        _previousState = currentState;
        _isIdle = false;
        currentState = EnemyStates.State.ATTACKING;
    }

    private void ResumeActions()
    {
        currentState = _previousState;
        _isAttacking = false;
    }
    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name,canBePlayedOver);
    }

    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }
}
