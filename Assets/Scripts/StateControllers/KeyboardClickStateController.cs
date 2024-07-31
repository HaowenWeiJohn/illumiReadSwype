using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardClickStateController : StateController
{




    public KeyboardClickStateGUIController keyboardClickStateGUIController;

    public GameObject KeyBoard;

    public TextMeshProUGUI targetWordText;

    // the list should contain the set of target words
    public List<string> targetWordList;

    private int currentWordIndex = 0;


    public GameObject PaintCursor;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // base.Update();
        stateShift();
    }


    public override void enterState()
    {
        keyboardClickStateGUIController.EnableSelf();
        targetWordText.text = targetWordList[0];
        KeyBoard.SetActive(true);
        PaintCursor.SetActive(true);
        base.enterState();

    }

    public override void exitState()
    {
        keyboardClickStateGUIController.DisableSelf();
        targetWordText.text = "";
        KeyBoard.SetActive(false);
        PaintCursor.SetActive(false);
        base.exitState();

    }

    public override void stateShift()
    {
        string outputText = keyboardClickStateGUIController.keyboardManager.outputField.text;
        if(currentWordIndex>=targetWordList.Count)
        {
            exitState();
        }
        else if (outputText==targetWordList[currentWordIndex] || Input.GetKeyDown(Presets.NextStateKey))
        {
            currentWordIndex += 1;
            if(currentWordIndex<targetWordList.Count)
            {
                targetWordText.text = targetWordList[currentWordIndex];
            }
            keyboardClickStateGUIController.keyboardManager.ClearOutputFieldText();
        }
        
        if(Input.GetKeyDown(Presets.InterruptKey))
        {
            base.currentState = Presets.State.InterruptState;
        }
    }

}
