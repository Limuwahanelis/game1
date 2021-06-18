using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public class PlayerLife : MonoBehaviour
{
    public Sprite fullHealth;
    public Sprite emptyHealth;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        
    }

    public void ChangeHeart(bool toEmpty)
    {
        if (toEmpty) image.sprite = emptyHealth;
        else image.sprite = fullHealth;
    }
}
