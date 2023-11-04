using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardDewellTimeIntroductionStateController : StateController
{




    public KeyboardDewellTimeIntroductionStateGUIController keyboardDewellTimeIntroductionStateGUIController;


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
        keyboardDewellTimeIntroductionStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardDewellTimeIntroductionStateGUIController.DisableSelf();
        base.exitState();

    }


}
