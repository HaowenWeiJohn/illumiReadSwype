using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SuggestionStripController : MonoBehaviour
{
    // Start is called before the first frame update

    public Image suggestionImage;
    public TextMeshProUGUI suggestionText;

    public bool hasGazeThisFrame = false;
    public bool selected = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SuggestionCallback();
        ClearGaze();
    }


    public void SetSuggestionText(string text)
    {
        suggestionText.text = text;
    }

    public void ClearSuggestionText()
    {
        suggestionText.text = "";
    }

    public void HasGaze()
    {
        hasGazeThisFrame = true;
    }

    public void ClearGaze()
    {
        hasGazeThisFrame = false;
    }


    public void SuggestionCallback()
    {

        if (hasGazeThisFrame)
        {
            if (!selected)
            {
                // first time one this key
            }
            selected = true;
            suggestionImage.color = KeyParams.KeyActiveColor;

            // watch the input


        }
        else
        {
            selected = false;
            // set color to regular color
            suggestionImage.color = KeyParams.KeyInactiveColor;

        }
    }

}
