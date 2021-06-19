using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Coin : MonoBehaviour,IInteractable,IAnimatable
{
    public CoinCustomSet set;
    public Action OnPickUpEvent;

    public event Action<string> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;

    public void Interact()
    {
        PlayAnimation("PickUp");
        StartCoroutine(WaitForAnimationToEnd(GetAnimationLength("PickUp")));
    }

    // Start is called before the first frame update
    void Awake()
    {
        set.Add(this);
    }

    private void OnEnable()
    {
        set.Add(this);
    }

    private void OnDisable()
    {
        set.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interact();
    }

    public void PlayAnimation(string name)
    {
        OnPlayAnimation?.Invoke(name);
    }

    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength?.Invoke(name);
    }

    IEnumerator WaitForAnimationToEnd(float time)
    {
        yield return new WaitForSeconds(time);
        OnPickUpEvent?.Invoke();
        set.Remove(this);
        Destroy(gameObject);
    }
}
