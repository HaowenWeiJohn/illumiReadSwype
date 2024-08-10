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


    public GameObject PaintCursor;
    public ExperimentManager experimentManager;
    private int StateEndIndex =0;

    private int currentWordIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        StateEndIndex = experimentManager.GazePinchTrialCount;
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
        targetWordText.text = experimentManager.targetwords[experimentManager.configuration];
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
        if(currentWordIndex>=StateEndIndex)
        {
            exitState();
        }
        else if (outputText==experimentManager.targetwords[experimentManager.configuration] || Input.GetKeyDown(Presets.NextStateKey))
        {
            experimentManager.configuration += 1;
            currentWordIndex += 1;
            if(currentWordIndex<StateEndIndex)
            {
                targetWordText.text =  experimentManager.targetwords[experimentManager.configuration];
            }
            keyboardClickStateGUIController.keyboardManager.ClearOutputFieldText();
        }
        
        if(Input.GetKeyDown(Presets.InterruptKey))
        {
            base.currentState = Presets.State.InterruptState;
        }
    }

}
