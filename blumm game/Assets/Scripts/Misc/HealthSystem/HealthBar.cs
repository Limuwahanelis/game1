using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public List<PlayerLife> lives = new List<PlayerLife>();
    public IntReference currentHP;
    public IntReference maxHP;
    
    public GameObject playerLifePrefab;
    public float heartDistance;

    public Vector2 playerLifesPosition;

    private int _previousHealth;
    private void Start()
    {
        
        heartDistance = playerLifePrefab.GetComponent<RectTransform>().rect.width;
        currentHP.value = maxHP.value;
        _previousHealth = currentHP.value;
        for(int i=0;i< maxHP.value;i++)
        {
            lives.Add(Instantiate(playerLifePrefab, GetComponent<RectTransform>()).GetComponent<PlayerLife>());
            lives[i].GetComponent<RectTransform>().anchoredPosition = playerLifesPosition;
            playerLifesPosition.x += heartDistance;
        }
    }

    private void Update()
    {
        
        if(_previousHealth>currentHP.value)
        {
            ReduceHealth(_previousHealth - currentHP.value);
        }
        if(_previousHealth<currentHP.value)
        {
            RestoreHealth();
        }
        _previousHealth = currentHP.value;
    }
    public void ReduceHealth(int dmg)
    {
        for(int i= currentHP.value; i>=currentHP.value;i--)
        {

            lives[i].RemoveHeart();
        }
    }

    public void IncreaseMaxHealth()
    {
        maxHP.value += 1;
        lives.Add(Instantiate(playerLifePrefab, GetComponent<RectTransform>()).GetComponent<PlayerLife>());
        lives[lives.Count-1].GetComponent<RectTransform>().anchoredPosition = playerLifesPosition;
        lives[lives.Count - 1].RemoveHeart();
        playerLifesPosition.x += heartDistance;
    }
    public void RestoreHealth()
    {
        lives[currentHP.value-1].RestoreHeart();
        _previousHealth = currentHP.value;
    }
}
