using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardIllumiReadSwypeStateController : StateController
{




    public KeyboardIllumiReadSwypeStateGUIController keyboardIllumiReadSwypeStateGUIController;

    public GameObject KeyBoard;
    public TextMeshProUGUI targetWordText;

    private int StateEndIndex =0;

    private int currentWordIndex = 0;

    public ExperimentManager experimentManager;

    public GameObject PaintCursor;


    // Start is called before the first frame update
    void Start()
    {
        // experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        StateEndIndex = experimentManager.SweyepeTrialCount;
    }

    // Update is called once per frame
    void Update()
    {
        // base.Update();
        stateShift();
    }


    public override void enterState()
    {
        keyboardIllumiReadSwypeStateGUIController.EnableSelf();
        targetWordText.text = experimentManager.targetwords[experimentManager.configuration];
        KeyBoard.SetActive(true);
        PaintCursor.SetActive(true);
        base.enterState();

    }

    public override void exitState()
    {
        keyboardIllumiReadSwypeStateGUIController.DisableSelf();
        targetWordText.text = "";
        KeyBoard.SetActive(false);
        PaintCursor.SetActive(false);
        base.exitState();

    }

    public override void stateShift()
    {
        string outputText = keyboardIllumiReadSwypeStateGUIController.keyboardManager.outputField.text;
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
                targetWordText.text = experimentManager.targetwords[experimentManager.configuration];
            }
            keyboardIllumiReadSwypeStateGUIController.keyboardManager.ClearOutputFieldText();
        }
        
        if(Input.GetKeyDown(Presets.InterruptKey))
        {
            base.currentState = Presets.State.InterruptState;
        }
    }


}
