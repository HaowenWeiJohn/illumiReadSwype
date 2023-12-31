using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Keyboard
{
    public class SuggestionKey : Key
    {
        [SerializeField] private string suggestionString;
        private TextMeshProUGUI buttonText;

        protected override void Awake()
        {
            base.Awake();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            //InitializeKey();
        }

        //private void InitializeKey() => buttonText.text = keyboard.autoCapsAtStart ? character.ToUpper() : character;

        protected override void OnPress()
        {
            base.OnPress();
            keyChannel.RaiseSuggestionKeyPressedEvent(suggestionString);

            //keychannel.raisekeypressedevent(character);
        }

        protected override void UpdateKey()
        {
            //if (keyboard.IsShiftActive() || keyboard.IsCapsLockActive())
            //{
            //    buttonText.text = character.ToUpper();
            //}
            //else
            //{
            //    buttonText.text = character.ToLower();
            //}
        }

        //protected void ChangeKeyColors(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor)
        //{
        //    base.ChangeKeyColors( normalColor,  highlightedColor,  pressedColor,  selectedColor);
        //}


        public void ChangeSuggestionKeyState(bool state)
        {
            ChangeKeyState(state);
        }



        public void SetSuggestionText(string suggestionString)
        {
            this.suggestionString = suggestionString;
            buttonText.text = suggestionString;
        }

        public void ClearSuggesitonText()
        {
            suggestionString = string.Empty;
            buttonText.text = string.Empty;
        }

        

    }
}