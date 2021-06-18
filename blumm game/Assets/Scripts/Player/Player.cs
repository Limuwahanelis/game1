using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IAnimatable
{
    public GameObject mainBody;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;

    public bool isMoving;
    public bool isJumping;
    public bool isMovableByPlayer = true;
    public bool isOnGround;
    public bool isFalling;
    public bool isAttacking;
    public event Action<string> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

    public GameObject attackEffect;
    


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
                if (isJumping)
                {
                    isMovableByPlayer = false;
                    PlayAnimation("Jump");
                    StartCoroutine(WaitForAnimationToEnd(GetAnimationLength("Jump"), (result => isJumping = result), isJumping));
                }
                if(isAttacking)
                {
                    isMovableByPlayer = false;
                    PlayAnimation("Attack");
                    StartCoroutine(AttackCor(GetAnimationLength("Attack"), (result => isAttacking = result), isAttacking));
                }
            }
        }

        if (!isOnGround)
        {
            if (isFalling) PlayAnimation("Fall");
        }
    }

    public float GetAnimationLength(string name)
    {
        return (float)(OnGetAnimationLength?.Invoke(name));
    }

    public void PlayAnimation(string name)
    {
        OnPlayAnimation?.Invoke(name);
    }

    IEnumerator WaitForAnimationToEnd(float animationLength,Action<bool> myVariableLambda,bool currentValue)
    {
        yield return new WaitForSeconds(animationLength);
        myVariableLambda(!currentValue);
        isMovableByPlayer = true;
    }

    IEnumerator AttackCor(float animationLength, Action<bool> myVariableLambda, bool currentValue)
    {
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(animationLength);
        myVariableLambda(!currentValue);
        isMovableByPlayer = true;
        attackEffect.SetActive(false);
    }

}
