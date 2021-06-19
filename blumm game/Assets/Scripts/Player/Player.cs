using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IAnimatable
{
    public enum Cause
    {
        NONE,
        ENEMY,
        OVERRIDE,
        ATTACK,
        JUMP,
    }
    public Cause NoControlCause = Cause.NONE;

    public GameObject mainBody;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    public HealthSystem playerHealth;


    public bool isMoving;
    public bool isJumping;
    public bool isMovableByPlayer = true;
    public bool isOnGround;
    public bool isFalling;
    public bool isAttacking;
    public bool isPushedBack;
    public event Action<string> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

    public GameObject attackEffect;
    


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<HealthSystem>();
        playerHealth.OnHitEvent += playerMovement.CollidedWithEnemy;
        playerHealth.OnDeathEvent += () => { };
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovableByPlayer)
        {
            if (isOnGround)
            {
                if (isMoving) PlayAnimation("Walk");
                else PlayAnimation("Idle");
                if(isAttacking)
                {
                    TakeControlFromPlayer(Cause.ATTACK);
                    PlayAnimation("Attack");
                    StartCoroutine(AttackCor(GetAnimationLength("Attack"), (result => isAttacking = result), isAttacking));
                }
            }
        }
        if (!isOnGround)
        {
            if (isFalling)
            {
                if(!isPushedBack) PlayAnimation("Fall");
            }
                
        }
        //if (isOnGround) ReturnControlToPlayer(Cause.ENEMY);
    }

    public void ReturnControlToPlayer(Cause returnControlCause)
    {
        if (NoControlCause == Cause.NONE) return;
        if (returnControlCause == Cause.OVERRIDE)
        {
            isMovableByPlayer = true;
            NoControlCause = Cause.NONE;
            return;
        }
        if (NoControlCause != returnControlCause) return;
        else
        {
            isMovableByPlayer = true;
        }
        NoControlCause = Cause.NONE;
    }

    public void TakeControlFromPlayer(Cause takeAwayCause)
    {
        isMovableByPlayer = false;
        NoControlCause = takeAwayCause;
        playerMovement.StopPlayer();
        //rb.velocity = new Vector2(0, 0);
    }
    public IEnumerator WaitForAnimationToEnd(float animationLength, Action<bool> myVariableLambda, bool currentValue, Cause noControlReason)
    {
        yield return new WaitForSeconds(animationLength);
        myVariableLambda(!currentValue);
        ReturnControlToPlayer(noControlReason);
    }

    IEnumerator AttackCor(float animationLength, Action<bool> myVariableLambda, bool currentValue)
    {
        yield return new WaitForSeconds(animationLength);
        myVariableLambda(!currentValue);
        ReturnControlToPlayer(Cause.ATTACK);
    }

    public IEnumerator WaitForPlayerToLandOnGroundAfterPush()
    {
        PlayAnimation("Push");
        while (isOnGround)
        {
            yield return null;
        }
        while(!isOnGround)
        {
            yield return null;
        }
        ReturnControlToPlayer(Cause.ENEMY);
        isPushedBack = false;
    }
    public float GetAnimationLength(string name)
    {
        return (float)(OnGetAnimationLength?.Invoke(name));
    }

    public void PlayAnimation(string name)
    {
        OnPlayAnimation?.Invoke(name);
    }

}
