using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardDewellTimeStateController : StateController
{




    public KeyboardDewellTimeStateGUIController keyboardDewellTimeStateGUIController;

    public GameObject KeyBoard;

    public string targetWord;

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
        keyboardDewellTimeStateGUIController.EnableSelf();
        KeyBoard.SetActive(true);
        base.enterState();

    }

    public override void exitState()
    {
        keyboardDewellTimeStateGUIController.DisableSelf();
        KeyBoard.SetActive(false);
        base.exitState();

    }

    public override void stateShift()
    {
        string outputText = keyboardDewellTimeStateGUIController.keyboardManager.outputField.text;
        if (outputText==targetWord)
        {
            exitState();
        }
        base.stateShift();
    }


}
