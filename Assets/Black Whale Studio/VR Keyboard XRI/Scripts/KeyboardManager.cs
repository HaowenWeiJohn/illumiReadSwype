/*
 * Copyright (c) 2023 Black Whale Studio. All rights reserved.
 *
 * This software is the intellectual property of Black Whale Studio. Direct use, copying, or distribution of this code in its original or only slightly modified form is strictly prohibited. Significant modifications or derivations are required for any use.
 *
 * If this code is intended to be used in a commercial setting, you must contact Black Whale Studio for explicit permission.
 *
 * For the full licensing terms and conditions, visit:
 * https://blackwhale.dev/
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT ANY WARRANTIES OR CONDITIONS.
 *
 * For questions or to join our community, please visit our Discord: https://discord.gg/55gtTryfWw
 */

using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VarjoExample;

namespace Keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        [Header("Keyboard Setup")]
        [SerializeField] private KeyChannel keyChannel;
        [SerializeField] private Button spacebarButton;
        [SerializeField] private Button speechButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button switchButton;
        [SerializeField] private string switchToNumbers = "Numbers";
        [SerializeField] private string switchToLetter = "Letters";

        private TextMeshProUGUI switchButtonText;

        [Header("Keyboards")]
        [SerializeField] private GameObject suggestionStrips;
        [SerializeField] private GameObject lettersKeyboard;
        [SerializeField] private GameObject numbersKeyboard;
        [SerializeField] private GameObject specialCharactersKeyboard;

        [Header("Shift/Caps Lock Button")] 
        [SerializeField] internal bool autoCapsAtStart = true;
        [SerializeField] private Button shiftButton;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite activeSprite;
 
        [Header("Switch Number/Special Button")]
        [SerializeField] private Button switchNumberSpecialButton;
        [SerializeField] private string numbersString = "Numbers";
        [SerializeField] private string specialString = "Special";

        private TextMeshProUGUI switchNumSpecButtonText;
        
        [Header("Keyboard Button Colors")]
        [SerializeField] private Color normalColor = Color.black;
        [SerializeField] private Color highlightedColor = Color.yellow;
        [SerializeField] private Color pressedColor = Color.red;
        [SerializeField] private Color selectedColor = Color.blue;
        
        [Header("Output Field Settings")]
        [SerializeField] private TMP_InputField outputField;
        [SerializeField] private Button enterButton;
        [SerializeField] private int maxCharacters = 15;
        [SerializeField] private int minCharacters = 3;

        private bool suggestionAnchorActivated = false;
        private int suggestionAnchorStartPosition = 0;
        private int suggestionAnchorEndPosition = 0;



        [Header("Suggestion Keys")]
        [SerializeField] List<SuggestionKey> suggestionKeys = new List<SuggestionKey>();

        //[SerializeField] private SuggestionKey SuggestionKey1;
        //[SerializeField] private SuggestionKey SuggestionKey2;
        //[SerializeField] private SuggestionKey SuggestionKey3;

        [Header("Interaction Mode")]
        public Presets.InteractionMode interactionMode;
        public KeyParams.KeyboardState keyboardState = KeyParams.KeyboardState.TypeState;


        private ColorBlock shiftButtonColors;
        private bool isFirstKeyPress = true;
        private bool keyHasBeenPressed;
        private bool shiftActive;
        private bool capsLockActive;
        private bool specialCharactersActive;
        private float lastShiftClickTime;
        private float shiftDoubleClickDelay = 0.5f;

        public UnityEvent onKeyboardModeChanged;

        private void Start() // changed awake to start
        {
            // set color
            normalColor = KeyParams.KeyNormalColor;
            highlightedColor = KeyParams.KeyHighlightedColor;
            pressedColor = KeyParams.KeyPressedColor;
            selectedColor = KeyParams.KeySelectedColor;

            // Set the colors of the shift button. It should be capitalized at the start
            shiftButtonColors = shiftButton.colors;
            
            CheckTextLength();

            speechButton.interactable = false;
            
            numbersKeyboard.SetActive(false);
            specialCharactersKeyboard.SetActive(false);
            lettersKeyboard.SetActive(true);

            spacebarButton.onClick.AddListener(OnSpacePress);
            deleteButton.onClick.AddListener(OnDeletePress);
            switchButton.onClick.AddListener(OnSwitchPress);
            shiftButton.onClick.AddListener(OnShiftPress);
            switchNumberSpecialButton.onClick.AddListener(SwitchBetweenNumbersAndSpecialCharacters);
            switchButtonText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            switchNumSpecButtonText = switchNumberSpecialButton.GetComponentInChildren<TextMeshProUGUI>();
            keyChannel.RaiseKeyColorsChangedEvent(normalColor, highlightedColor, pressedColor, selectedColor);
            
            switchNumberSpecialButton.gameObject.SetActive(false);
            numbersKeyboard.SetActive(false);
            specialCharactersKeyboard.SetActive(false);

            if (!autoCapsAtStart) return;
            ActivateShift();
            UpdateShiftButtonAppearance();

            DisableSelf();
        }

        private void OnDestroy()
        {
            spacebarButton.onClick.RemoveListener(OnSpacePress);
            deleteButton.onClick.RemoveListener(OnDeletePress);
            switchButton.onClick.RemoveListener(OnSwitchPress);
            shiftButton.onClick.RemoveListener(OnShiftPress);
            switchNumberSpecialButton.onClick.RemoveListener(SwitchBetweenNumbersAndSpecialCharacters);
        }

        private void OnEnable()
        {
            keyChannel.OnLetterKeyPressed += LetterKeyPress;
            keyChannel.OnSuggestionKeyPressed += SuggestionKeyPress;
            keyChannel.OnSuggestionsReceived += SuggestionsReceived;

            // keyboard state
            keyChannel.OnKeyboardStateChange += SetKeyboardState;

        }

        private void OnDisable()
        {
            keyChannel.OnLetterKeyPressed -= LetterKeyPress;
            keyChannel.OnSuggestionKeyPressed -= SuggestionKeyPress;
            keyChannel.OnSuggestionsReceived -= SuggestionsReceived;

            // keyboard state
            keyChannel.OnKeyboardStateChange -= SetKeyboardState;

        }

        private void LetterKeyPress(string key)
        {


            keyHasBeenPressed = true;
            bool wasShiftActive = shiftActive;
            DeactivateShift();

            string textToInsert;
            if (wasShiftActive || capsLockActive)
            {
                textToInsert = key.ToUpper();
            }
            else
            {
                textToInsert = key.ToLower();
            }

            // check current state 
            if (keyboardState == KeyParams.KeyboardState.SelectSuggestionState)
            {
                // auto add space
                textToInsert = " " + textToInsert;
                // back to regular state
                keyChannel.RaiseKeyboardStateChangeEvent(KeyParams.KeyboardState.TypeState);
                DeactivateSuggesitonKeys();
            }


            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            outputField.text = outputField.text.Remove(startPos, endPos - startPos);
            outputField.text = outputField.text.Insert(startPos, textToInsert);

            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + textToInsert.Length;

            if (isFirstKeyPress)
            {
                isFirstKeyPress = false;
                keyChannel.onFirstKeyPress.Invoke();
            }
    
            CheckTextLength();
        }

        private void SuggestionKeyPress(string suggestion)
        {
            Debug.Log(suggestion);

            if (keyboardState == KeyParams.KeyboardState.SelectSuggestionState)
            {

                // replace the holding suggestion
                int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
                int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

                outputField.text = outputField.text.Remove(suggestionAnchorStartPosition, suggestionAnchorEndPosition - suggestionAnchorStartPosition);
                outputField.text = outputField.text.Insert(suggestionAnchorStartPosition, suggestion);

                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = suggestionAnchorStartPosition + suggestion.Length;


                keyChannel.RaiseKeyboardStateChangeEvent(KeyParams.KeyboardState.TypeState);
                DeactivateSuggesitonKeys();
            }
            else
            {
                Debug.Log("Not in Suggestion Selection State, but could select Suggestions. Report Bug");
            }

            

            // remove the last word

            // put in the selected suggestion

            // deactivate all the sugggestion




        }

        private void OnSpacePress()
        {

            if (keyboardState == KeyParams.KeyboardState.SelectSuggestionState)
            {
                // auto add space
                keyChannel.RaiseKeyboardStateChangeEvent(KeyParams.KeyboardState.TypeState);
                DeactivateSuggesitonKeys();
            }

            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            outputField.text = outputField.text.Remove(startPos, endPos - startPos);
            outputField.text = outputField.text.Insert(startPos, " ");

            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + 1;
            
            CheckTextLength();
        }

        private void OnDeletePress()
        {

            if (keyboardState == KeyParams.KeyboardState.SelectSuggestionState)
            {
                // remove the holdingg suggesiton
                outputField.text = outputField.text.Remove(suggestionAnchorStartPosition, suggestionAnchorEndPosition - suggestionAnchorStartPosition);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = suggestionAnchorStartPosition;
                // deactivate keyboard suggestions
                keyChannel.RaiseKeyboardStateChangeEvent(KeyParams.KeyboardState.TypeState);
                DeactivateSuggesitonKeys();
            }




            if (string.IsNullOrEmpty(outputField.text)) return;
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            if (endPos > startPos)
            {
                outputField.text = outputField.text.Remove(startPos, endPos - startPos);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos;
            }
            else if (startPos > 0)
            {
                outputField.text = outputField.text.Remove(startPos - 1, 1);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos - 1;
            }
            
            CheckTextLength();
        }

        private void CheckTextLength()
        {
            //// TODO: refactor this method

            //int currentLength = outputField.text.Length;

            //// Raise event to enable or disable keys based on the text length
            //bool keysEnabled = currentLength < maxCharacters;
            //keyChannel.RaiseKeysStateChangeEvent(keysEnabled);

            //// Disables or enables the enter button based on the text length
            //enterButton.interactable = currentLength >= minCharacters;

            //// Always enable the delete button, regardless of the text length
            //deleteButton.interactable = true;
            
            //// Disable shift/caps lock if maximum text length is reached
            //if (currentLength != maxCharacters) return;
            //DeactivateShift();
            //capsLockActive = false;
            //UpdateShiftButtonAppearance();
        }

        private void SetKeyboardState(KeyParams.KeyboardState newKeyboardState)
        {
            keyboardState = newKeyboardState;
        }
        

        private void OnSwitchPress()
        {

            // if selecting suggestions
            if (keyboardState == KeyParams.KeyboardState.SelectSuggestionState)
            {
                keyChannel.RaiseKeyboardStateChangeEvent(KeyParams.KeyboardState.TypeState);
                DeactivateSuggesitonKeys();
            }

            if (lettersKeyboard.activeSelf)
            {
                lettersKeyboard.SetActive(false);
                numbersKeyboard.SetActive(true);
                specialCharactersKeyboard.SetActive(false);
                switchNumberSpecialButton.gameObject.SetActive(true);

                // Set buttons' text
                switchButtonText.text = switchToNumbers;
                switchNumSpecButtonText.text = specialString;
            }
            else
            {
                lettersKeyboard.SetActive(true);
                numbersKeyboard.SetActive(false);
                specialCharactersKeyboard.SetActive(false);
                switchNumberSpecialButton.gameObject.SetActive(false);

                // Set buttons' text
                switchButtonText.text = switchToLetter;
                switchNumSpecButtonText.text = specialString;
            }
            DeactivateShift();
            onKeyboardModeChanged?.Invoke();
        }


        private void OnShiftPress()
        {
            if (capsLockActive)
            {
                // If Caps Lock is active, deactivate it
                capsLockActive = false;
                shiftActive = false;
            }
            else switch (shiftActive)
            {
                case true when !keyHasBeenPressed && Time.time - lastShiftClickTime < shiftDoubleClickDelay:
                    // If Shift is active, a key has not been pressed, and Shift button was double clicked, activate Caps Lock
                    capsLockActive = true;
                    shiftActive = false;
                    break;
                case true when !keyHasBeenPressed:
                    // If Shift is active, a key has not been pressed, deactivate Shift
                    shiftActive = false;
                    break;
                case false:
                    // If Shift is not active and Shift button was clicked once, activate Shift
                    shiftActive = true;
                    break;
            }

            lastShiftClickTime = Time.time;
            UpdateShiftButtonAppearance();
            onKeyboardModeChanged?.Invoke();
        }

        private void ActivateShift()
        {
            if (!capsLockActive) shiftActive = true;

            UpdateShiftButtonAppearance();
            onKeyboardModeChanged?.Invoke();
        }

        public void DeactivateShift() // TODO: also deactivate the switing options
        {
            if (shiftActive && !capsLockActive && keyHasBeenPressed)
            {
                shiftActive = false;
                UpdateShiftButtonAppearance();
                onKeyboardModeChanged?.Invoke();
            }

            keyHasBeenPressed = false;
        }

        public bool IsShiftActive() => shiftActive;

        public bool IsCapsLockActive() => capsLockActive;

        private void SwitchBetweenNumbersAndSpecialCharacters()
        {
            if (lettersKeyboard.activeSelf) return;

            // Switch between numbers and special characters keyboard
            bool isNumbersKeyboardActive = numbersKeyboard.activeSelf;
            numbersKeyboard.SetActive(!isNumbersKeyboardActive);
            specialCharactersKeyboard.SetActive(isNumbersKeyboardActive);

            switchNumSpecButtonText.text = switchNumSpecButtonText.text == specialString ? numbersString : specialString;

            onKeyboardModeChanged?.Invoke();
        }

        private void UpdateShiftButtonAppearance()
        {
            if (capsLockActive)
            {
                shiftButtonColors.normalColor = highlightedColor;
                buttonImage.sprite = activeSprite;
            }
            else if(shiftActive)
            {
                shiftButtonColors.normalColor = highlightedColor;
                buttonImage.sprite = defaultSprite;
            }
            else
            {
                shiftButtonColors.normalColor = normalColor;
                buttonImage.sprite = defaultSprite;
            }

            shiftButton.colors = shiftButtonColors;
        }


        public void EnableSelf()
        {
            gameObject.SetActive(true);
            //keyChannel.RaiseKeysStateChangeEvent(true);
        }

        public void DisableSelf()
        {
            gameObject.SetActive(false);
            //keyChannel.RaiseKeysStateChangeEvent(false);
        }


        public void SuggestionsReceived(List<string> suggestionList)
        {

            // start with lower case
            for (int i = 0; i < suggestionList.Count; i++)
            {
                suggestionList[i] = suggestionList[i].ToLower();
            }

            // if capital we turn Upper
            if (capsLockActive)
            {
                // turn the suggestion list into Caps
                // Loop through the list and convert each string to uppercase
                for (int i = 0; i < suggestionList.Count; i++)
                {
                    suggestionList[i] = suggestionList[i].ToUpper();
                }
            }

            if (shiftActive)
            {
                for (int i = 0; i < suggestionList.Count; i++)
                {
                    suggestionList[i] = char.ToUpper(suggestionList[i][0]) + suggestionList[i].Substring(1);
                }
            }



            // activate suggestion strips, suggestion received
            ActivateSuggesitonKeys(); // set interactiable
            // update suggestion list with 1:-1
            UpdateSuggestionKeys(suggestionList.GetRange(1, suggestionKeys.Count));
            
            // put the first suggestion on the top
            string suggestionToInsert = suggestionList[0];
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);


            outputField.text = outputField.text.Remove(startPos, endPos - startPos);

            // when index is not 0 and left is not space
            if (startPos!=0)
            {
                if(outputField.text.Substring(startPos-1, 1) != " ") 
                {
                // insert a space after the previous word
                outputField.text = outputField.text.Insert(startPos, " ");
                startPos += 1;
                }
            }


            //if (startPos!=) // if there is no space and not the first word
            //{
            //    // insert a space 
            //    outputField.text = outputField.text.Insert(startPos, " ");
            //    startPos += 1;
            //}


            // insert the first suggestion
            outputField.text = outputField.text.Insert(startPos, suggestionToInsert);

            // set the anchor position and suggestion anchor position
            suggestionAnchorActivated = true;
            suggestionAnchorStartPosition = startPos;
            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + suggestionToInsert.Length;
            suggestionAnchorEndPosition = outputField.selectionAnchorPosition;


            // transfer to select suggestion state
            keyChannel.RaiseKeyboardStateChangeEvent(KeyParams.KeyboardState.SelectSuggestionState);
            keyHasBeenPressed = true;
            DeactivateShift();


        }


        public void ActivateSuggesitonKeys()
        {

            foreach (SuggestionKey suggestionKey in suggestionKeys)
            {
                suggestionKey.ChangeSuggestionKeyState(true);
            }

        }


        public void UpdateSuggestionKeys(List<string> suggestionList)
        {
            for (int i = 0; i < suggestionKeys.Count; i++)
            {
                suggestionKeys[i].SetSuggestionText(suggestionList[i]);
            }
        }


        public void DeactivateSuggesitonKeys()
        {
            
            foreach(SuggestionKey suggestionKey in suggestionKeys)
            {
                suggestionKey.ChangeSuggestionKeyState(false);
                suggestionKey.ClearSuggesitonText();
            }

        }
        public void EnableSuggestionStrips()
        {
            suggestionStrips.SetActive(true);
            ActivateSuggesitonKeys();
        }


        public void DisableSuggestionStrips()
        {
            suggestionStrips.SetActive(false);
            DeactivateSuggesitonKeys();
        }



        public void SetKeyboardInteractionMode(Presets.InteractionMode mode)
        {
            interactionMode = mode;

            // set suggestions strips
            if(interactionMode == Presets.InteractionMode.None)
            {
                DisableSuggestionStrips();
            }
            else if (interactionMode == Presets.InteractionMode.ButtonClick)
            {
                DisableSuggestionStrips();
            }
            else if (interactionMode == Presets.InteractionMode.DwellTime)
            {
                DisableSuggestionStrips();
            }
            else if (interactionMode == Presets.InteractionMode.IllumiReadSwype)
            {
                EnableSuggestionStrips();
                DeactivateSuggesitonKeys();
            }
            else if (interactionMode == Presets.InteractionMode.FreeSwitch)
            {
                EnableSuggestionStrips();
            }


        }

        public void ClearOutputFieldText()
        {
            outputField.text = string.Empty;
            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = 0;
        }



    }
}