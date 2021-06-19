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

    private bool _isAlive = true;
    private bool _isHit = false;
    private bool _isIdle = false;
    private bool _isCheckingForPlayerCol;
    private bool _isAttacking;
    private Coroutine currentCor = null;

    private PlayerDetection _detection;

    private Collider2D _hitCollider;

    // Start is called before the first frame update
    void Start()
    {
        SetUpBehaviour();
        SetUpComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isHit || !_isAttacking)
            {
                if (currentState == PatrolState.PATROLLING)
                {
                    PlayAnimation("Move");
                    MoveToPatrolPoint();
                }
                if (currentState == PatrolState.IDLE_AT_PATROL_POINT || currentState == PatrolState.ALWAYS_IDLE)
                {
                   currentCor=StartCoroutine(IdleTimerCor(idleCycles));
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
        if (currentState == PatrolState.IDLE_AT_PATROL_POINT)
        {
            currentState = PatrolState.PATROLLING;
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
        _isAttacking = true;
        PlayAnimation("Attack");
    }
    public void StartCheckingForPlayerCol()
    {
        _isCheckingForPlayerCol = true;
        StartCoroutine(CheckForPlayerColliderCor());
    }
    public void StopCheckingForPlaterCol()
    {
        _isCheckingForPlayerCol = false;
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

    }
    protected override void SetPlayerInRange()
    {
        StopCurrentActions();
        Attack();
    }
    protected override void SetPlayerNotInRange()
    {
        
    }
    private void StopCurrentActions()
    {
        if (currentCor != null) StopCoroutine(currentCor);
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
