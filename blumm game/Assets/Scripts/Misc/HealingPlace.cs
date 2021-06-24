using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlace : MonoBehaviour
{
    public IntReference playerCurrentHP;
    public IntReference playerMaxHP;
    private bool _isHealing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerCurrentHP.value < playerMaxHP.value) StartCoroutine(HealPlayerCor());
    }

    IEnumerator HealPlayerCor()
    {
        if (_isHealing) yield break;
        _isHealing = true;
        playerCurrentHP.value += 1;
        yield return new WaitForSeconds(1f);
        _isHealing = false;
    }
}
