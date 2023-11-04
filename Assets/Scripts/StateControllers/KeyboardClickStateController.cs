using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardClickStateController : StateController
{




    public KeyboardClickStateGUIController keyboardClickStateGUIController;


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
        keyboardClickStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardClickStateGUIController.DisableSelf();
        base.exitState();

    }


}
