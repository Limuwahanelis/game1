using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLootbox : Lootbox
{
    public CoinsPool coinsPool;


    protected override void GiveItems()
    {
        StartCoroutine(SpawnCor());
    }

    IEnumerator SpawnCor()
    {
        for (int i = 0; i < amountOfitems; i++)
        {
            Coin tmp = coinsPool.GetItem();
            tmp.transform.position = GetSpawnPoint();
            IInteractable tmpi = tmp.GetComponent<IInteractable>();
            yield return new WaitForSeconds(0.15f);
            tmpi.Interact();
        }
    }
}
