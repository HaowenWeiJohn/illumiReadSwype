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

using Leap.Unity;
using Leap.Unity.Examples;
using UnityEngine;
using UnityEngine.UI;

namespace Keyboard
{
    public class Key : MonoBehaviour
    {
        [SerializeField] PinchDetector pinchDetector;
        [SerializeField] protected KeyChannel keyChannel;
        [SerializeField] protected KeyboardManager keyboard;
        protected Button button;

        public BoxCollider boxCollider;

        [Header("Additional Key Settings")]
        public bool hasGazeThisFrame;
        public KeyParams.Keys key;
        public bool selected = false;



        [Header("DwellTime Settings")]
        public float keyDwellTimeCounter = 0;


        protected virtual void Update()
        {
            switch (keyboard.interactionMode)
            {
                case Presets.InteractionMode.DwellTime:
                    KeyDwellTimeCallback();
                    break;
                case Presets.InteractionMode.ButtonClick:
                    KeyButtonClickCallback();
                    break;
                case Presets.InteractionMode.IllumiReadSwype:
                    KeyIllumiReadSwypeCallback();
                    break;
                    //case Presets.InteractionMode.FreeSwitch:
                    //    KeyFreeSwitchCallback();
                    //    break;

            }

            ClearGaze();

        }





        protected virtual void Awake()
        {

            // get box collider
            boxCollider = GetComponent<BoxCollider>();
            // set box collider size equal to the button size
            boxCollider.size = new Vector3(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height, 0.001f);


            button = GetComponent<Button>();
            button.onClick.AddListener(OnPress);

            keyboard.onKeyboardModeChanged.AddListener(UpdateKey);
            keyChannel.onFirstKeyPress.AddListener(UpdateKey);
            keyChannel.OnKeyColorsChanged += ChangeKeyColors; 
            keyChannel.OnKeysStateChange += ChangeKeyState;
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(OnPress);
            keyboard.onKeyboardModeChanged.RemoveListener(UpdateKey);
            keyChannel.onFirstKeyPress.RemoveListener(UpdateKey);
            keyChannel.OnKeyColorsChanged -= ChangeKeyColors;
            keyChannel.OnKeysStateChange -= ChangeKeyState;
        }

        protected virtual void OnPress()
        {
            keyboard.DeactivateShift();
            //keyboard.DeactivateSuggesitonKeys();
        }

        protected virtual void UpdateKey()
        {
            // empty method for override in child classes
        }
        
        protected void ChangeKeyColors(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = normalColor;
            cb.highlightedColor = highlightedColor;
            cb.pressedColor = pressedColor;
            cb.selectedColor = selectedColor;
            button.colors = cb;
        }

        protected void ChangeKeyState(bool enabled)
        {
            button.interactable = enabled;
        }


        public void HasGaze()
        {
            hasGazeThisFrame = true;
        }
        

        public void ClearGaze()
        {
            hasGazeThisFrame = false;
        }



        public void KeyDwellTimeCallback()
        {
            if (hasGazeThisFrame)
            {
                if (!selected)
                {
                    // highlight key
                    // first time one this key
                    // play the audio clip
                    InvokeButtonSelected();
                    selected = true;
                }
                

                keyDwellTimeCounter += Time.deltaTime;


                Color selectedColor = Color.Lerp(KeyParams.KeyNormalColor, KeyParams.KeySelectedColor, keyDwellTimeCounter / KeyParams.KeyboardDwellActivateTime);

                ChangeKeyColors(
                    KeyParams.KeyNormalColor,
                    KeyParams.KeyHighlightedColor,
                    KeyParams.KeyPressedColor,
                    selectedColor
                    );

                if (keyDwellTimeCounter >= KeyParams.KeyboardDwellActivateTime)
                {
                    // reset the counter
                    keyDwellTimeCounter = 0;
                    // reset the color
                    InvokeButtonOnClick();
                    ChangeKeyColors(KeyParams.KeyNormalColor, KeyParams.KeyHighlightedColor, KeyParams.KeyPressedColor, KeyParams.KeyNormalColor);
                    // evoke key stroke
                    Debug.Log(key);
                    PlayKeyEnterAudioClip();
                }
                else
                {
                    PlayKeyHoverAudioClip();
                }


            }
            else
            {
                keyDwellTimeCounter = 0;
                ChangeKeyColors(KeyParams.KeyNormalColor, KeyParams.KeyHighlightedColor, KeyParams.KeyPressedColor, KeyParams.KeySelectedColor);
                InvokeButtonNormal();
                selected = false;
            }
        }



        public void KeyButtonClickCallback()
        {

            if (hasGazeThisFrame)
            {
                if (!selected)
                {
                    // first time one this key

                    // highlight key
                    InvokeButtonSelected();
                    selected = true;
                    // play the audio clip
                    PlayKeyHoverAudioClip();
                }


                if (Input.GetKeyDown(Presets.UserInputButton1) || pinchDetector.DidStartPinch) // evoke button press 
                {
                    // evoke key stroke
                    InvokeButtonOnClick();
                    // play audio
                    PlayKeyEnterAudioClip();
                }
            }
            else
            {
                // set color to regular color
                InvokeButtonNormal();
                selected = false;
            }
        }



        public void KeyIllumiReadSwypeCallback()
        {

            if (hasGazeThisFrame)
            {
                if (!selected)
                {
                    // highlight key
                    // first time one this key
                    InvokeButtonSelected();
                    selected = true;
                    // play the audio clip
                    PlayKeyHoverAudioClip();


                }
                // still allow the user to press the button
                if (Input.GetKeyDown(Presets.UserInputButton1) || pinchDetector.DidStartPinch)
                    //if (Input.GetKeyDown(Presets.UserInputButton1))
                {
                    // evoke key stroke
                    InvokeButtonOnClick();
                    PlayKeyEnterAudioClip();
                }
                if (keyDwellTimeCounter <= KeyParams.KeyboardDwellActivateTime)
                {
                    keyDwellTimeCounter += Time.deltaTime;


                    Color selectedColor = Color.Lerp(KeyParams.KeyNormalColor, KeyParams.KeySelectedColor, keyDwellTimeCounter / KeyParams.KeyboardDwellActivateTime);

                    ChangeKeyColors(
                        KeyParams.KeyNormalColor,
                        KeyParams.KeyHighlightedColor,
                        KeyParams.KeyPressedColor,
                        selectedColor
                        );


                }
                else
                {
                    // do nothing, keep it selected
                }


            }
            else
            {
                keyDwellTimeCounter = 0;
                ChangeKeyColors(KeyParams.KeyNormalColor, KeyParams.KeyHighlightedColor, KeyParams.KeyPressedColor, KeyParams.KeySelectedColor);
                InvokeButtonNormal();
                selected = false;
            }

        }





        public void InvokeButtonOnClick()
        {
            button.onClick.Invoke();
        }

        public void InvokeButtonSelected()
        {
            button.Select();
        }


        public void InvokeButtonNormal()
        {
            // set regular button
            button.OnDeselect(null);
        }


        public void PlayKeyHoverAudioClip() {
            AudioSource.PlayClipAtPoint(keyboard.keyHoverAudioClip, transform.position); 
        }

        public void PlayKeyEnterAudioClip()
        {
            AudioSource.PlayClipAtPoint(keyboard.keyEnterAudioClip, transform.position);
        }













    }
}