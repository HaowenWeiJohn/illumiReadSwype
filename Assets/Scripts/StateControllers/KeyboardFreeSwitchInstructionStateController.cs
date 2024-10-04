using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardFreeSwitchInstructionStateController : StateController
{




    public KeyboardFreeSwitchInstructionStateGUIController keyboardFreeSwitchInstructionStateGUIController;


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
        keyboardFreeSwitchInstructionStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardFreeSwitchInstructionStateGUIController.DisableSelf();
        base.exitState();

    }


}
