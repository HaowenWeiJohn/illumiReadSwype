using Keyboard;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionStripKey : Key
{



    [SerializeField] private string suggestionText;
    private TextMeshProUGUI buttonText;

    

    protected override void Update()
    {
        base.Update();
    }

    protected override void Awake()
    {
        base.Awake(); // set the initial key color
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        ClearAndDeactivateSuggestionStrip();
    }



    public void SetAndActivateSuggestionStrip(string s)
    {
        suggestionText = s;
        buttonText.text = s;
        button.interactable = true;
    }

    public void ClearAndDeactivateSuggestionStrip()
    {
        suggestionText = "";
        buttonText.text = "";
        button.interactable = false;
    }

    protected override void OnPress()
    {
        base.OnPress();
        keyChannel.RaiseSuggestionKeyPressedEvent(suggestionText);
    }


    


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }



}
