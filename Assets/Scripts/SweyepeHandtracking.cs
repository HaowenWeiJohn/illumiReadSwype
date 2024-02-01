using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweyepeHandtracking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPostDetected()
    {
        Debug.Log("Detected thumb up!");
    }
}
