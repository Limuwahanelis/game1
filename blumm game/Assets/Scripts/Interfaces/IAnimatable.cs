using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IAnimatable
{
    public event Action<string> OnPlayAnimation;
    public event Func<string,float> OnGetAnimationLength;
    //public delegate void AnimationDelegate(string name);

    void PlayAnimation(string name);
    float GetAnimationLength(string name);
}
