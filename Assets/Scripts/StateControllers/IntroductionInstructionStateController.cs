using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionInstructionStateController : StateController
{




    public IntroductionInstructionStateGUIController introductionInstructionStateGUIController;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }


    public override void enterState()
    {
        introductionInstructionStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        introductionInstructionStateGUIController.DisableSelf();
        base.exitState();

    }


}
