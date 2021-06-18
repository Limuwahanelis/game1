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
    private bool _isAlive = true;
    private int _patrolPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        hpSys = GetComponent<HealthSystem>();
        hpSys.OnDeathEvent += KillEnemy;
        for(int i=0;i<patrolPoints.Count;i++)
        {
            _patrolpositions.Add(patrolPoints[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (patrolPoints.Count > 1) MoveToPatrolPoint();
        }
    }


    public void MoveToPatrolPoint()
    {
        if (!_isIdle)
        {
            PlayAnimation("Move");
            //RaiseOnWalkEvent();
            transform.position = Vector3.MoveTowards(transform.position, _patrolpositions[_patrolPointIndex], speed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - _patrolpositions[_patrolPointIndex].x) < 0.1)
            {
                if (_patrolPointIndex + 1 > _patrolpositions.Count - 1) _patrolPointIndex = 0;
                else _patrolPointIndex++;

                if (idleCycles >0 ) StartCoroutine(IdleTimerCor());
                else
                {
                    if (_patrolpositions[_patrolPointIndex].x < transform.position.x) RotateEnemy(-1);
                    else RotateEnemy(1);
                }

            }
        }
    }
    IEnumerator IdleTimerCor()
    {
        if (_isIdle) yield break;
        else _isIdle = true;
        PlayAnimation("Idle");
        yield return new WaitForSeconds(idleCycles*GetAnimationLength("Idle"));
        if (_patrolpositions[_patrolPointIndex].x < transform.position.x) RotateEnemy(-1);
        else RotateEnemy(1);
        _isIdle = false;
    }

    private void KillEnemy()
    {
        _isIdle = true;
        PlayAnimation("Death");
        StartCoroutine(WaitForAnimationToEnd(GetAnimationLength("Death"), (result) => _isAlive = result, _isAlive));
    }
    private void RotateEnemy(int direction)
    {
        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }


    IEnumerator WaitForAnimationToEnd(float animationLength, Action<bool> myVariableLambda, bool currentValue)
    {
        yield return new WaitForSeconds(animationLength);
        myVariableLambda(!currentValue);
        gameObject.SetActive(false);
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
