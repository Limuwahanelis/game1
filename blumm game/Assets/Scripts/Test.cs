using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
public class Test : MonoBehaviour
{
    public int testInt;
    public List<int> testList = new List<int>();
    public int size;
#if UNITY_EDITOR
    public AnimatorController controlf;
    //public int testIntEd;
#endif
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
