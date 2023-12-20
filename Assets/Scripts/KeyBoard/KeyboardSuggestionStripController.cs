using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardSuggestionStripController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject SuggestionStrips;

    public SuggestionStripController suggestionStripController1;
    public SuggestionStripController suggestionStripController2;
    public SuggestionStripController suggestionStripController3;

    public Presets.InteractionMode interactionMode = Presets.InteractionMode.ButtonClick;

    public List<string> suggestionsList = new List<string>();


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearAllSuggestions()
    {
        suggestionStripController1.ClearSuggestionText();
        suggestionStripController2.ClearSuggestionText();
        suggestionStripController3.ClearSuggestionText();
    }
    
    public void SetSuggestionStrips(List<string> suggestionsList)
    {

        suggestionStripController1.SetSuggestionText(suggestionsList[1]);
        suggestionStripController2.SetSuggestionText(suggestionsList[2]);
        suggestionStripController3.SetSuggestionText(suggestionsList[3]);
    }

    public int GetSuggestionWithGazeIndex()
    {
        if (suggestionStripController1.hasGazeThisFrame)
        {
            return 1;
        }
        else if (suggestionStripController2.hasGazeThisFrame)
        {
            return 2;
        }
        else if (suggestionStripController3.hasGazeThisFrame)
        {
            return 3;
        }
        else
        {
            return 0;
        }   
    }

    public void DisableSuggestionStrips()
    {
        SuggestionStrips.SetActive(false);
    }

    public void EnableSuggestionStrips()
    {
        SuggestionStrips.SetActive(true);
    }


}
