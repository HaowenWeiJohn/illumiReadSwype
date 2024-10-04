using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardClickIntroductionStateController : StateController
{




    public KeyboardClickIntroductionStateGUIController keyboardClickIntroductionStateGUIController;


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
        keyboardClickIntroductionStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardClickIntroductionStateGUIController.DisableSelf();
        base.exitState();

    }


}
