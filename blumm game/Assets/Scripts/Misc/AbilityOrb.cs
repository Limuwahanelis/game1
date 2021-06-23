using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AbilityOrb : MonoBehaviour,IInteractable,IAnimatable
{

    public AbilityManager.PlayerAbilites unlockedAbility;
    private PlayerDetection _detection;

    public event Action<string, bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Func<float> OnGetAnimationRemainingTime;
    public event Action<string> OnOverPlayAnimation;

    public UnityEvent<AbilityManager.PlayerAbilites> OnOrbPickedup;
    // Start is called before the first frame update
    void Start()
    {
        _detection = GetComponent<PlayerDetection>();
        _detection.OnPlayerDetected = Interact;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        PlayAnimation("Pickup");
        StartCoroutine(WaitForAnimationToEnd(GetAnimationLength("Pickup")));
        
    }

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name,canBePlayedOver);
    }


    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }
    IEnumerator WaitForAnimationToEnd(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        //Abilities.UnlockAbility(unlockedAbility);
        OnOrbPickedup?.Invoke(unlockedAbility);
    }
    
}
