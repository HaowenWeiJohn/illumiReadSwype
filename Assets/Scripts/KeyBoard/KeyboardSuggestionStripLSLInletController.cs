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


    public List<string> DecodeSuggestionsFromLSL(float[] suggestionsLVT)
    {
        List<string> suggestionsList = new List<string>();
        int nextDataIndex = 0;

        int overflowFlag = (int)suggestionsLVT[0];

        nextDataIndex += 1;

        if (overflowFlag == 1)
        {
            Debug.Log("Overflow");
            return suggestionsList;
        }

        int numSuggestions = (int)suggestionsLVT[1];
        nextDataIndex += 1;

        string[] suggestions = new string[numSuggestions];


        for (int i = 0; i < numSuggestions; i++)
        {
            int suggestionIndex = (int)suggestionsLVT[nextDataIndex];
            nextDataIndex += 1;

            int wordLength = (int)suggestionsLVT[nextDataIndex];
            nextDataIndex += 1;

            string word = "";

            for (int j = 0; j < wordLength; j++)
            {
                int value = (int)suggestionsLVT[nextDataIndex];
                KeyParams.Keys thisKey = KeyParams.IDKeys[value];
                word += KeyParams.KeysString[thisKey];
                nextDataIndex += 1;
            }
            suggestionsList.Add(word);
        }

        return suggestionsList;

    }



}
