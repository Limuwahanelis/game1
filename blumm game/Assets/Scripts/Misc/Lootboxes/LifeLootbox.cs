using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLootbox : Lootbox
{
    public HealthBar playerHealthBar;
    public GameObject playerPickupLife;
    protected override void GiveItems()
    {
        playerPickupLife.SetActive(true);
        StartCoroutine(WaitForHeartAnimationToEnd());
        
    }
    IEnumerator WaitForHeartAnimationToEnd()
    {
        yield return new WaitForSeconds(playerPickupLife.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        playerHealthBar.IncreaseMaxHealth();
    }

    private void OnValidate()
    {
        amountOfitems = 1;
    }
}
