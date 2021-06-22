using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HealthSystem))]
public abstract class Enemy : MonoBehaviour,IAnimatable
{
    protected HealthSystem hpSys;
    public float speed;
    public int dmg;
    public int collisionDmg;

    public GameObject mainCollider;

    protected bool _isAlive = true;
    protected bool _isIdle = false;
    protected bool _isHit = false;

    public event Action<string, bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;
    public event Func<float> OnGetAnimationRemainingTime;

    protected EnemyEnums.State currentState;
    protected Stack<EnemyEnums.State> states = new Stack<EnemyEnums.State>();
    protected virtual void SetPlayerInRange() {}
    protected virtual void SetPlayerNotInRange() {}
    protected virtual void SetUpComponents()
    {
        hpSys = GetComponent<HealthSystem>();
    }


    protected virtual void StopCurrentActions()
    {
        StopAllCoroutines();
    }
    protected virtual void ResumeActions()
    {
        currentState = states.Pop();
    }

    protected virtual void Kill()
    {
        StopCurrentActions();
        mainCollider.SetActive(false);
        _isAlive = false;
        currentState = EnemyEnums.State.DEAD;
        PlayAnimation("Death");
        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Death"), () => Destroy(gameObject)));
    }

    protected virtual void Hit()
    {
        StopCurrentActions();
        states.Push(currentState);
        _isHit = true;
        PlayAnimation("Hit");
        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Hit"), () =>
        {
            states.Push(EnemyEnums.State.IDLE_AFTER_HIT);
            _isHit = false;
            ResumeActions();
        }));
    }

    protected IEnumerator StayIdleCor(int numberOfIdleCycles = 1)
    {
        _isIdle = true;
        if(numberOfIdleCycles>0)PlayAnimation("Idle");
        yield return new WaitForSeconds(numberOfIdleCycles * GetAnimationLength("Idle"));
        _isIdle = false;
    }
    protected IEnumerator WaitAndExecuteFunction(float timeToWait, Action functionToPerform)
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

    protected float GetCurrentAnimationRemainingLength()
    {
        return (float)OnGetAnimationRemainingTime?.Invoke();
    }
}