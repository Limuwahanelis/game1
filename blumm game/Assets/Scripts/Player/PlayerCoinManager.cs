using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinManager : MonoBehaviour
{
    public IntReference numberOfCoins;
    public CoinLootboxCustomSet lootboxes;
    public CoinCustomSet coins;
    public Sprite[] numberSprites;
    public Image tensNum;
    public Image onesNum;

    // Start is called before the first frame update
    void Start()
    {
        onesNum.sprite = numberSprites[numberOfCoins.value % 10];
        tensNum.sprite = numberSprites[numberOfCoins.value / 10];
        for (int i=0;i<coins.items.Count;i++)
        {
            coins.items[i].OnPickUpEvent = CollectCoin;
        }
        for(int i=0;i<lootboxes.items.Count;i++)
        {
            lootboxes.items[i].OnOpenBox += OpenLootbox;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CollectCoin()
    {
        numberOfCoins.value += 1;
        UpdateDisplay();
    }

    void OpenLootbox(int amountOfCoinsInLootbox)
    {
        numberOfCoins.value += amountOfCoinsInLootbox;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        onesNum.sprite = numberSprites[numberOfCoins.value % 10];
        tensNum.sprite = numberSprites[numberOfCoins.value / 10];
    }

    public void RegisterNewCoin(Coin coin)
    {
        coin.OnPickUpEvent = CollectCoin;
    }
}
