using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardIllumiReadSwypeIntroductionStateController : StateController
{




    public KeyboardIllumiReadSwypeIntroductionStateGUIController keyboardIllumiReadSwypeIntroductionStateGUIController;


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
        keyboardIllumiReadSwypeIntroductionStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardIllumiReadSwypeIntroductionStateGUIController.DisableSelf();
        base.exitState();

    }


}
