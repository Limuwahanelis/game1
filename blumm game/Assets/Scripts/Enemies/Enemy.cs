using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HealthSystem))]
public abstract class Enemy : MonoBehaviour
{
    protected HealthSystem hpSys;
    public float speed;
    public int dmg;
    public int collisionDmg;

    protected EnemyEnums.State currentState;
    protected Stack<EnemyEnums.State> states = new Stack<EnemyEnums.State>();
    protected virtual void SetPlayerInRange() {}
    protected virtual void SetPlayerNotInRange() { }
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
}