using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardFreeSwitchState : StateController
{




    public KeyboardFreeSwitchStateGUIController keyboardFreeSwitchStateGUIController;


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
        keyboardFreeSwitchStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardFreeSwitchStateGUIController.DisableSelf();
        base.exitState();

    }


}
