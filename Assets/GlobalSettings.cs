using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

public class GlobalSettings : MonoBehaviour
{
    // awake function called before start
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        VarjoMixedReality.StartRender();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
