using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    private string _currentAnimation;
    private Animator _anim;
    private IAnimatable _objectToAnimate;
    public AnimatorController _animController;
    public List<string> stateNames = new List<string>();
    private void Start()
    {
        _objectToAnimate = GetComponent<IAnimatable>();
       _objectToAnimate.OnPlayAnimation += PlayAnimation;
        _objectToAnimate.OnGetAnimationLength += GetStateLength;
       _anim = GetComponent<Animator>();
        
    }
    private void OnValidate()
    {
        stateNames.Clear();
        for (int i = 0; i < _animController.layers[0].stateMachine.states.Length; i++)
        {
            stateNames.Add(_animController.layers[0].stateMachine.states[i].state.name);
        }
        
    }
    public void PlayAnimation(string name)
    {
        
        AnimatorState clipToPlay=null;
        for (int i = 0; i < _animController.layers[0].stateMachine.states.Length; i++)
        {
            if (_animController.layers[0].stateMachine.states[i].state.name == name)
            {
                clipToPlay = _animController.layers[0].stateMachine.states[i].state;
            }
        }
        
        if (clipToPlay == null)
        {
            Debug.LogError("There is no state with name: " + name);
            return ;
        }
        if (_currentAnimation == clipToPlay.name) return;
        _anim.Play(clipToPlay.nameHash);
        _currentAnimation = clipToPlay.name;
        return ;
    }
    private void OnDestroy()
    {
        _objectToAnimate.OnPlayAnimation -= PlayAnimation;
        _objectToAnimate.OnGetAnimationLength -= GetStateLength;
    }
    public float GetStateLength(string name)
    {
        float clipDuration = 0;
        for (int i = 0; i < _animController.layers[0].stateMachine.states.Length; i++)
        {
            if (_animController.layers[0].stateMachine.states[i].state.name == name)
            {
                clipDuration = _animController.layers[0].stateMachine.states[i].state.motion.averageDuration;
            }
        }
        return clipDuration;
    }
}
