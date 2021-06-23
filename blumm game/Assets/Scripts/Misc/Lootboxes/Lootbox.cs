using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Lootbox : MonoBehaviour,IAnimatable,IInteractable
{

    public event Action<string, bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Func<float> OnGetAnimationRemainingTime;
    public event Action<string> OnOverPlayAnimation;

    public event Action<int> OnOpenBox;
    public Transform SpawnTransform;
    public int amountOfitems;

    private bool _isPlayerNear = false;

    //private bool _isOpened
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(_isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.F)) Interact();
        }
    }


    public float GetAnimationLength(string name)
    {
        throw new NotImplementedException();
    }

    public void Interact()
    {
        GiveItems();
        PlayAnimation("Open");
    }

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name,canBePlayedOver);
    }


    protected abstract void GiveItems();

    protected Vector3 GetSpawnPoint()
    {
        if (amountOfitems == 1) return SpawnTransform.position;
        return new Vector3(SpawnTransform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), SpawnTransform.position.y + UnityEngine.Random.Range(0, 1f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isPlayerNear = false;
    }
}
