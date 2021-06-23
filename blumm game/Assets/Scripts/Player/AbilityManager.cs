using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AbilityManager : MonoBehaviour
{
    public enum PlayerAbilites
    {
        DOUBLEJUMP
    }
    [SerializeField]
    private bool[] unlockedAbilities= new bool[Enum.GetNames(typeof(PlayerAbilites)).Length];
    private void Start()
    {
        unlockedAbilities = new bool[Enum.GetNames(typeof(PlayerAbilites)).Length];
    }
    public void UnlockAbility(PlayerAbilites ability)
    {
        unlockedAbilities[(int)ability] = true;
    }
    public bool CheckIfAbilityIsUnloked(PlayerAbilites ability)
    {
        return unlockedAbilities[(int)ability];
    }
}
