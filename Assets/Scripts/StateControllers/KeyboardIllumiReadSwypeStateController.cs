using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardIllumiReadSwypeStateController : StateController
{




    public KeyboardIllumiReadSwypeStateGUIController keyboardIllumiReadSwypeStateGUIController;


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
        keyboardIllumiReadSwypeStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        keyboardIllumiReadSwypeStateGUIController.DisableSelf();
        base.exitState();

    }


}
