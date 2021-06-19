using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy,IAnimatable
{
    private HealthSystem _healthSystem;

    public int idleCycles;
    public List<Transform> patrolPoints = new List<Transform>();
    private List<Vector3> _patrolpositions = new List<Vector3>();

    public event Action<string> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

    private bool _isIdle;
    private bool _isPatrolling;
    private bool _isAlive = true;
    private bool _isAtPatrolPoint = false;
    private bool _isHit = false;
    private int _patrolPointIndex = 0;


    private Coroutine currentCor = null;

    // Start is called before the first frame update
    void Start()
    {
        if (patrolPoints.Count < 2)
        {
            _isAtPatrolPoint = true;
            _isIdle = true;
        }
        else
        {
            _isPatrolling = true;
        }
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeathEvent += KillEnemy;
        hpSys.OnHitEvent += HitEnemy;
        for(int i=0;i<patrolPoints.Count;i++)
        {
            _patrolpositions.Add(patrolPoints[i].position);
        }
        RotateEnemyTowardsNextPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isHit)
            {
                if (_isPatrolling)
                {
                    MoveToPatrolPoint();
                }
                else
                {
                    if (_isAtPatrolPoint)
                    {
                        if ( !_isIdle) currentCor = StartCoroutine(IdleTimerCor(idleCycles));
                    }
                }
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
            _isPatrolling = false;
            _isAtPatrolPoint = true;
        }
    }
    private void KillEnemy()
    {
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
        _isIdle = false;
        _isPatrolling = false;
        StopCoroutine(currentCor);
    }
    private void ResumeActions()
    {
        _isHit = false;
        if (_isAtPatrolPoint) currentCor = StartCoroutine(IdleTimerCor(idleCycles));
        else _isPatrolling = true;
    }
    IEnumerator IdleTimerCor(int numbeOfIdleCycles)
    {
        _isIdle = true;
        PlayAnimation("Idle");
        yield return new WaitForSeconds(numbeOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
        _isPatrolling = true;
        _isAtPatrolPoint = false;
        RotateEnemyTowardsNextPatrolPoint();
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
        if (!_isAtPatrolPoint)
        {
            PlayAnimation("Idle");
            yield return new WaitForSeconds(GetAnimationLength("Idle"));
        }
        
        ResumeActions();
    }
    public void PlayAnimation(string name)
    {
        OnPlayAnimation?.Invoke(name);
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
