using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy, IAnimatable
{
    public enum State
    {
        ALWAYS_IDLE,
        PATROLLING,
        IDLE_AT_PATROL_POINT
    }
    private State currentState;
    private HealthSystem _healthSystem;

    public int idleCycles;
    public List<Transform> patrolPoints = new List<Transform>();
    private List<Vector3> _patrolpositions = new List<Vector3>();

    public event Action<string,bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

    private bool _isAlive = true;
    private bool _isHit = false;
    private bool _isIdle = false;
    private int _patrolPointIndex = 0;


    private Coroutine currentCor = null;

    // Start is called before the first frame update
    void Start()
    {
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeathEvent += KillEnemy;
        hpSys.OnHitEvent += HitEnemy;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            _patrolpositions.Add(patrolPoints[i].position);
        }
        if (patrolPoints.Count < 2)
        {
            currentState = State.ALWAYS_IDLE;
        }
        else
        {
            currentState = State.PATROLLING;
            RotateEnemyTowardsNextPatrolPoint();
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isHit)
            {
                if (currentState == State.PATROLLING) MoveToPatrolPoint();
                if (currentState == State.IDLE_AT_PATROL_POINT|| currentState==State.ALWAYS_IDLE) StartCoroutine(IdleTimerCor(idleCycles));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void MoveToPatrolPoint()
    {
        PlayAnimation("Move");
        transform.position = Vector3.MoveTowards(transform.position, _patrolpositions[_patrolPointIndex], speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - _patrolpositions[_patrolPointIndex].x) < 0.1)
        {
            if (_patrolPointIndex + 1 > _patrolpositions.Count - 1) _patrolPointIndex = 0;
            else _patrolPointIndex++;
            currentState = State.IDLE_AT_PATROL_POINT;
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
    private void RotateEnemyTowardsNextPatrolPoint()
    {
        float direction;

        if (_patrolpositions[_patrolPointIndex].x < transform.position.x) direction=-1;
        else direction=1;

        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }
    private void StopCurrentActions()
    {
       if(currentCor!=null) StopCoroutine(currentCor);
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
