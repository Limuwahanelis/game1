using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System;
[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{

    private string _currentAnimation;
    private Animator _anim;
    private IAnimatable _objectToAnimate;
    public AnimatorController _animController;
    public List<string> stateNames = new List<string>();
    private float _animLength;
    private bool _overPlayAnimationEnded=true;
    private bool _timerStarted;
    private Coroutine _currentTimer;
    private bool _animationEnded;
    private void Start()
    {
        _objectToAnimate = GetComponent<IAnimatable>();
       _objectToAnimate.OnPlayAnimation += PlayAnimation;
        _objectToAnimate.OnGetAnimationLength += GetStateLength;
        _objectToAnimate.OnOverPlayAnimation+= OverPlayAnimation;
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
    public void PlayAnimation(string name,bool canBePlayedOver=true)
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
        if (!canBePlayedOver)
        {

            _overPlayAnimationEnded = false;
            _animLength = clipToPlay.motion.averageDuration;
            _currentTimer=StartCoroutine(TimerCor(_animLength,SetOverPlayAnimAsEnded));
            _anim.Play(clipToPlay.nameHash);
            _currentAnimation = clipToPlay.name;
        }
        
        if (_overPlayAnimationEnded)
        {
            _animLength = clipToPlay.motion.averageDuration;
            StartCoroutine(TimerCor( _animLength, SetNormalAnimAsEneded));
            _anim.Play(clipToPlay.nameHash);
            _currentAnimation = clipToPlay.name;
        }
    }

    public void OverPlayAnimation(string name)
    {
        AnimatorState clipToPlay = null;
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
            return;
        }
        //if (_currentAnimation == clipToPlay.name) return;
        if(_currentTimer!=null) StopCoroutine(_currentTimer);
        _overPlayAnimationEnded = true;
        _timerStarted = false;

        _anim.Play(clipToPlay.nameHash);
        _currentAnimation = clipToPlay.name;
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

    IEnumerator TimerCor(float time,Action functionToPerform)
    {
        //if (_timerStarted) yield break;
        //else _timerStarted = true;
        yield return new WaitForSeconds(time);
        functionToPerform();
    }

    private void SetOverPlayAnimAsEnded()
    {
        _overPlayAnimationEnded = true;
        _timerStarted = false;
    }
    private void SetNormalAnimAsEneded()
    {
        _currentAnimation = null;
    }

}
