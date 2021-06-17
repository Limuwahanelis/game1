using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IAnimatable
{
    public GameObject mainBody;
    public PlayerMovement playerMovement;

    public bool isMoving;

    public event Action<string> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;



    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) PlayAnimation("Walk");
        else PlayAnimation("Idle");
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
