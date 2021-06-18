using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public List<PlayerLife> lives = new List<PlayerLife>();
    public IntReference currentHP;
    public float heartDistance;
    public PlayerLife playerLifePrefab;
    private int _previousHealth;
    private void Start()
    {
        _previousHealth = currentHP.value;
        if (lives.Count < currentHP.value)
        {
            for (int i = 0; i < currentHP.value; i++)
            {
                lives.Add(Instantiate(playerLifePrefab, lives.Count == 0 ? transform.position : lives[lives.Count - 1].transform.position + new Vector3(heartDistance, 0f), playerLifePrefab.transform.rotation, transform));
            }
        }
    }

    private void Update()
    {
        if(_previousHealth>currentHP.value)
        {
            ReduceHealth(_previousHealth - currentHP.value);
        }
    }
    public void ReduceHealth(int dmg)
    {
        for(int i=lives.Count-1; i>=0;i--)
        {
            lives[i].ChangeHeart(true);
        }
    }
}
