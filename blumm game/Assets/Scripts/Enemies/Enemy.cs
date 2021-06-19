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

    protected virtual void SetPlayerInRange() {}
    protected virtual void SetPlayerNotInRange() { }
    protected virtual void SetUpComponents() { }
}