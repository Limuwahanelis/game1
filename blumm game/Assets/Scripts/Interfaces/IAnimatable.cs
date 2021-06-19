using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IAnimatable
{
    public event Action<string,bool> OnPlayAnimation;
    public event Func<string,float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;
    void PlayAnimation(string name,bool canBePlayedOver=true);
    float GetAnimationLength(string name);
}
