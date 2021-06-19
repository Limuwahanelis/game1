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
        set.Remove(this);
        OnPickUpEvent?.Invoke();
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        set.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        throw new NotImplementedException();
    }

    public float GetAnimationLength(string name)
    {
        throw new NotImplementedException();
    }
}
