using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSuggestionStripLSLInletController : LSLInletInterface
{
    // Start is called before the first frame update
    void Start()
    {
        
        streamName = Presets.KeyboardSuggestionStripLSLStreamName;
        StartContinousResolver();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PullSuggestionsInfo()
    {
        if (streamActivated)
        {
            pullSample();
            clearBuffer();
        }
    }

    //public void ReturnStringList()
    //{
    //    // return the string list
    //    PullSuggestionsInfo();
        
    //    if(frameTimestamp != 0) 
    //    {
            
    //    }

    //}




}
