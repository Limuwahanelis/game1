using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPool : CustomPool<Coin>
{
    public GameObject coinPrefab;
    public PlayerCoinManager coinMan;
    private void Start()
    {
        Coin[] coins = GetComponentsInChildren<Coin>(true);
        items = new List<Coin>(coins);
        for(int i=0;i<items.Count;i++)
        {
            items[i].GetComponent<PartOfPool>().poolPos = transform.position;
        }

    }
    public override Coin GetItem()
    {
        for(int i=0;i<items.Count;i++)
        {
            Coin toReturn = items[i];
            PartOfPool item = toReturn.GetComponent<PartOfPool>();
            if(item.isFree)
            {
                toReturn.gameObject.SetActive(true);
                coinMan.RegisterNewCoin(toReturn);
                item.isFree = false;
                return toReturn;
            }
        }
        GameObject tmp= Instantiate(coinPrefab,transform);
        SetUpNewCoin(ref tmp);
        return tmp.GetComponent<Coin>();
    }

    private void SetUpNewCoin(ref GameObject coin)
    {
        coin.SetActive(true);
        coin.GetComponent<PartOfPool>().isFree = false;
        coin.GetComponent<PartOfPool>().poolPos = transform.position;

        Add(coin.GetComponent<Coin>());
        coinMan.RegisterNewCoin(coin.GetComponent<Coin>());
    }

}
