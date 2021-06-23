using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAnimatable
{
    public enum Cause
    {
        NONE,
        ENEMY,
        COLLISION,
        OVERRIDE,
        ATTACK,
        JUMP,
        DEATH
    }
    public Cause NoControlCause = Cause.NONE;

    public GameObject mainBody;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    public HealthSystem playerHealth;

    public PhysicsMaterial2D noFrictionMat;
    public PhysicsMaterial2D normalMat;

    [HideInInspector]
    public bool isAlive = true;
    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool isMovableByPlayer = true;
    [HideInInspector]
    public bool isOnGround;
    [HideInInspector]
    public bool isFalling;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool isPushedBack;
    [HideInInspector]
    public bool isHit;
    [HideInInspector]
    public bool isNearWall;

    public bool canDoubleJump;

    public bool checkForLastPush = false;
    public bool performedLastPush = false;

    public event Action<string,bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;
    public event Func<float> OnGetAnimationRemainingTime;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<HealthSystem>();
        playerHealth.OnHit +=playerCombat.PlayerHit;
        playerHealth.OnPush+= playerMovement.CollidedWithEnemy;
        playerHealth.OnDeath = playerCombat.KillPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (isMovableByPlayer)
            {
                if (isOnGround)
                {
                    canDoubleJump = true;
                    playerMovement.ChangePhysicsMaterial(normalMat);
                    if (isMoving) PlayAnimation("Walk");
                    else PlayAnimation("Idle");
                    if (isAttacking)
                    {
                        TakeControlFromPlayer(Cause.ATTACK);
                        PlayAnimation("Attack");
                        StartCoroutine(WaitAndExecuteFunction(GetAnimationLength("Attack"), () =>
                         {
                             isAttacking = !isAttacking;
                             ReturnControlToPlayer(Cause.ATTACK);
                         }));
                    }

                }
            }
            if (!isOnGround)
            {
                
                if (isFalling)
                {
                    if (!isPushedBack) PlayAnimation("Fall");
                }

            }


            if (isNearWall) playerMovement.ChangePhysicsMaterial(noFrictionMat);
        }



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
    }

    public void StopAllActions()
    {
        TakeControlFromPlayer(Cause.DEATH);
        StopAllCoroutines();
        playerMovement.StopAllCoroutines();
        playerCombat.StopAllCoroutines();
        playerHealth.StopAllCoroutines();


        isMoving = false;
        isJumping = false;
        isMovableByPlayer = false;
        isFalling = false;
        isAttacking = false;
        isPushedBack = false;
        isHit = false;

        
    }
    public IEnumerator WaitAndExecuteFunction(float timeToWait, Action functionToExecute)
    {
        yield return new WaitForSeconds(timeToWait);
        functionToExecute();
    }

    public IEnumerator WaitForPlayerToLandOnGroundAfterPush()
    {
        PlayAnimation("Hit");
        if (playerCombat.invincibilityCor != null) playerCombat.StopCoroutine(playerCombat.invincibilityCor);
        if(playerHealth.currentHP.value<=0)
        {
            playerCombat.StopAllCoroutines();
        }
        //playerCollisions.SetEnemyCollisions(false);
        playerHealth.isPushable = false;
        playerHealth.isInvincible = true;
        while (isOnGround)
        {
            yield return null;
        }
        while(!isOnGround)
        {
            yield return null;
        }
        playerHealth.isPushable = true;
        playerCombat.StartCoroutine(playerCombat.NotPushableCor());
        //playerCollisions.SetEnemyCollisions(true);
        playerHealth.isInvincible = false;
        playerCombat.invincibilityCor = playerCombat.StartCoroutine(playerCombat.InvincibilityCor());
        
        if(playerHealth.currentHP.value<=0)
        {
            playerCombat.KillPlayer();
            yield break;
        }
        playerMovement.StopPlayerOnXAxis();
        ReturnControlToPlayer(Cause.COLLISION);
        isPushedBack = false;
    }
    public float GetAnimationLength(string name)
    {
        return (float)(OnGetAnimationLength?.Invoke(name));
    }

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name,canBePlayedOver);
    }

    public void OverPlayAnimation(string name)
    {
        OnOverPlayAnimation?.Invoke(name);
    }
}
