using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Coin : MonoBehaviour,IAnimatable,IInteractable
{
    public CoinCustomSet set;
    public Action OnPickUpEvent;
    private PlayerDetection detection;
    private PartOfPool pool;

    public event Action<string,bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Action<string> OnOverPlayAnimation;
    public event Func<float> OnGetAnimationRemainingTime;

    private void Start()
    {
        detection = GetComponent<PlayerDetection>();
        pool = GetComponent<PartOfPool>();
        if (detection != null) detection.OnPlayerDetected = Interact;
        //else Interact();
    }

    private void Awake()
    {
        set.Add(this);
    }

    public void Interact()
    {
        PlayAnimation("PickUp");
        StartCoroutine(WaitForAnimationToEnd(GetAnimationLength("PickUp")));
    }



    IEnumerator WaitForAnimationToEnd(float time)
    {
        yield return new WaitForSeconds(time);
        OnPickUpEvent?.Invoke();
        if (pool == null)
        {
            Destroy(gameObject);
            set.Remove(this);
        }
        else
        {
            pool.ReturnToPool();
        }

    }

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name, canBePlayedOver);
    }

    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }

    private void OnDestroy()
    {
        set.Remove(this);
    }
}
