using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController: MonoBehaviour
{
    // Start is called before the first frame update


    public GameManager gameManager;

    [Header("Keyboard Components")]
    public KeyboardController keyBoardController;
    public KeyboardInputFieldCanvasController keyboardInputFieldCanvasController;
    public KeyboardSuggestionStripController keyboardSuggestionStripController;

    [Header("Keyboard Interaction Mode")]
    public Presets.InteractionMode interactionMode = Presets.InteractionMode.ButtonClick;


    [Header("LSL Inlet Interface")]
    public KeyboardSuggestionStripLSLInletController keyboardSuggestionStripLSLInletController;

    [Header("Input String List")]
    public List<string> inputWordList = new List<string>();


    [Header("Swype Suggestion String List")]
    public List<string> suggestionsWordList = new List<string>();

    [Header("Swyping State")]
    public Presets.illumiReadSwypeState illumiReadSwypeState= Presets.illumiReadSwypeState.Idle;




    void Start()
    {

    }


    void Update()
    {

        switch (interactionMode)
        {
            case Presets.InteractionMode.DwellTime:
                KeyboardDwellTimeCallback();
                break;
            case Presets.InteractionMode.ButtonClick:
                KeyboardButtonClickCallback();
                break;
            case Presets.InteractionMode.IllumiReadSwype:
                KeyboardIllumiReadSwypeCallback();
                break;
            case Presets.InteractionMode.FreeSwitch:
                KeyboardFreeSwitchCallback();
                break;
        }

        // temp clear all the input
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inputWordList.Clear();
            keyboardInputFieldCanvasController.SetUserInputWithWordList(inputWordList);
        }



    }

    

    public void UpdateKeyInput(KeyParams.Keys key)
    {
        keyboardInputFieldCanvasController.UpdateKeyInput(key);
    }




    public void EnableSelf()
    {
        keyBoardController.ResetKeyboard();
        keyboardInputFieldCanvasController.ResetInputField();
        gameObject.SetActive(true);
    }

    public void DisableSelf()
    {
        keyBoardController.ResetKeyboard();
        keyboardInputFieldCanvasController.ResetInputField();
        gameObject.SetActive(false);
    }


    public void setKeyboardInteractionMode(Presets.InteractionMode interactionMode)
    {
        this.interactionMode = interactionMode;
        keyBoardController.interactionMode = interactionMode;
        keyboardSuggestionStripController.interactionMode = interactionMode;
    }



    // callback functions
    public void KeyboardDwellTimeCallback()
    {
        // do nothing
    }

    public void KeyboardButtonClickCallback()
    {
        // do nothing
    }

    public void KeyboardIllumiReadSwypeCallback()
    {

        // Idle State
        if (illumiReadSwypeState == Presets.illumiReadSwypeState.Idle)
        {
            if (Input.GetKeyDown(Presets.UserInputButton2))
            {
                // remove the suggestion list
                suggestionsWordList.Clear();
                // clear the suggestion strip
                keyboardSuggestionStripController.ClearAllSuggestions();

                illumiReadSwypeState = Presets.illumiReadSwypeState.Swyping;
                Debug.Log("Start Swyping");
            }
        }


        // Swyping State
        else if (illumiReadSwypeState == Presets.illumiReadSwypeState.Swyping)
        {
            if (Input.GetKeyUp(Presets.UserInputButton2))
            {
                illumiReadSwypeState = Presets.illumiReadSwypeState.WaitingForSuggestions;
                Debug.Log("Waiting for Suggestions");
            }
        }


        // waiting for suggestions state
        else if (illumiReadSwypeState == Presets.illumiReadSwypeState.WaitingForSuggestions)
        {
            keyboardSuggestionStripLSLInletController.PullSuggestionsInfo();
            if (keyboardSuggestionStripLSLInletController.frameTimestamp != 0)
            {
                // we found the suggestions
                Debug.Log("Update Suggestions");
                suggestionsWordList = DecodeSuggestionsFromLSL(keyboardSuggestionStripLSLInletController.frameDataBuffer);

                if (suggestionsWordList.Count > 0)
                {
                    // update the suggestion strip
                    keyboardSuggestionStripController.ClearAllSuggestions();
                    // Add one to the input field word list
                    inputWordList.Add(suggestionsWordList[0]);
                    // update the suggestion strip using 1:-1
                    keyboardSuggestionStripController.SetSuggestionStrips(suggestionsWordList);
                    // update the input field
                    keyboardInputFieldCanvasController.SetUserInputWithWordList(inputWordList);
                    // enable the selecting suggestions strip
                    keyboardSuggestionStripController.EnableSuggestionStrips();


                    illumiReadSwypeState = Presets.illumiReadSwypeState.SelectingSuggestion;
                }
                else
                {
                    illumiReadSwypeState = Presets.illumiReadSwypeState.Idle;
                }


            }
        }


        // selecting suggestion state
        else if (illumiReadSwypeState == Presets.illumiReadSwypeState.SelectingSuggestion)
        {
            // understand if the suggestion is correct


            if (Input.GetKeyDown(Presets.UserInputButton1)) // select the suggestion
            {
                // get suggestion has gaze, if has gaze we update the input field and go back to idle
                
                // get the suggestion that has gaze on it
                int suggestionIndex = keyboardSuggestionStripController.GetSuggestionWithGazeIndex();
                if(suggestionIndex!=0)
                {
                    AudioSource.PlayClipAtPoint(keyBoardController.KeyEnterAudioClip, transform.position);

                    string word = suggestionsWordList[suggestionIndex];
                    // update the last word in the input field
                    inputWordList[inputWordList.Count - 1] = word;
                    // update the input field
                    keyboardInputFieldCanvasController.SetUserInputWithWordList(inputWordList);
                    // diable the suggestion strip
                    keyboardSuggestionStripController.DisableSuggestionStrips();
                    // go back to idle
                    illumiReadSwypeState = Presets.illumiReadSwypeState.Idle;
                }

            }
            if (Input.GetKeyDown(Presets.UserInputButton2)) // continue swyping
            {
                // start swyping again
                suggestionsWordList.Clear();
                keyboardSuggestionStripController.ClearAllSuggestions();
                // disable the suggestion strip
                keyboardSuggestionStripController.DisableSuggestionStrips();
                // go back to swyping
                illumiReadSwypeState = Presets.illumiReadSwypeState.Swyping;
                

            }
            else if (Input.GetKeyDown(Presets.UserInputButton3)) // return to the regular state
            {
                // clear the suggestion strip
                suggestionsWordList.Clear();
                keyboardSuggestionStripController.ClearAllSuggestions();
                
                // remove the last been putted in the input field
                inputWordList.RemoveAt(inputWordList.Count - 1);
                // update the input field
                keyboardInputFieldCanvasController.SetUserInputWithWordList(inputWordList);
                // disable the suggestion strip
                keyboardSuggestionStripController.DisableSuggestionStrips();
                // go back to idle
                illumiReadSwypeState = Presets.illumiReadSwypeState.Idle;

            }
        }





        //// waiting the suggestion strip update
        //keyboardSuggestionStripLSLInletController.PullSuggestionsInfo();

            //if (keyboardSuggestionStripLSLInletController.frameTimestamp != 0)
            //{
            //    Debug.Log("Update Suggestions");
            //    List<string> suggestionsList = DecodeSuggestionsFromLSL(keyboardSuggestionStripLSLInletController.frameDataBuffer);
            //    if (suggestionsList.Count > 0)
            //    {
            //        keyboardSuggestionStripController.ClearAllSuggestions();
            //        // update the suggestion strip
            //        inputWordList.Add(suggestionsList[0]);
            //        keyboardSuggestionStripController.SetSuggestionStrips(suggestionsList);

            //        keyboardInputFieldCanvasController.SetUserInputWithWordList(inputWordList);

            //    }
            //    else
            //    {
            //        // do nothing, no suggestions detected
            //    }
            //}


    }

    public void KeyboardFreeSwitchCallback()
    {
        // do nothing
    }


    public List<string> DecodeSuggestionsFromLSL(float[] suggestionsLVT)
    {
        List<string> suggestionsList = new List<string>();
        int nextDataIndex = 0;

        int overflowFlag = (int)suggestionsLVT[0];

        nextDataIndex+=1;

        if(overflowFlag == 1)
        {
            Debug.Log("Overflow");
            return suggestionsList;
        }

        int numSuggestions = (int)suggestionsLVT[1];
        nextDataIndex+=1;

        string[] suggestions = new string[numSuggestions];


        for(int i = 0; i < numSuggestions; i++)
        {
            int suggestionIndex = (int)suggestionsLVT[nextDataIndex];
            nextDataIndex+=1;

            int wordLength = (int)suggestionsLVT[nextDataIndex];
            nextDataIndex+=1;

            string word = "";

            for (int j = 0; j < wordLength; j++)
            {
                int value = (int)suggestionsLVT[nextDataIndex];
                KeyParams.Keys thisKey = KeyParams.IDKeys[value];
                word += KeyParams.KeysString[thisKey];
                nextDataIndex+=1;
            }
            suggestionsList.Add(word);
        }

        return suggestionsList;

    }

    public void illumiReadSwypeRefreshWordList()
    {
        keyboardInputFieldCanvasController.SetUserInputWithWordList(inputWordList);
    }


}
