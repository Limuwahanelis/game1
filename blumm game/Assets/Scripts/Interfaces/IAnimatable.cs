using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IAnimatable
{
    public event Action<string,bool> OnPlayAnimation;
    public event Func<string,float> OnGetAnimationLength;

    void PlayAnimation(string name,bool canBePlayedOver=true);
    float GetAnimationLength(string name);
}
