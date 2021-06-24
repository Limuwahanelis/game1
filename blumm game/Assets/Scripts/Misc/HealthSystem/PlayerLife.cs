using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public class PlayerLife : MonoBehaviour,IAnimatable
{
    public GameObject emptyHeartSprite;
    private Image image;

    public event Action<string, bool> OnPlayAnimation;
    public event Func<string, float> OnGetAnimationLength;
    public event Func<float> OnGetAnimationRemainingTime;
    public event Action<string> OnOverPlayAnimation;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void RemoveHeart()
    {
        image.enabled = false;
        emptyHeartSprite.SetActive(true);

    }

    public void RestoreHeart()
    {
        image.enabled = true;
        PlayAnimation("Restore");
    }

    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        OnPlayAnimation?.Invoke(name, canBePlayedOver);
    }

    public float GetAnimationLength(string name)
    {
        return (float)OnGetAnimationLength(name);
    }
    public void RestoreHeartAnimationLogic()
    {
        emptyHeartSprite.SetActive(false);
    }
    IEnumerator WaitForAnimationToEnd()
    {
        yield return new WaitForSeconds(GetAnimationLength("Restore"));
    }
}
