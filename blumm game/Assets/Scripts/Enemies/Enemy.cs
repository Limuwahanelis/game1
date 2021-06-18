using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HealthSystem))]
public abstract class Enemy : MonoBehaviour
{
    public Animator anim;
    protected HealthSystem hpSys;
    public float speed;
    public int dmg;

    public virtual void SetPlayerInRange() {}
    public virtual void SetPlayerNotInRange() { }
}