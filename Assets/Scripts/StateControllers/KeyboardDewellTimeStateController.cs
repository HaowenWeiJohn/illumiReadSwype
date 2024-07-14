using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardDewellTimeStateController : StateController
{




    public KeyboardDewellTimeStateGUIController keyboardDewellTimeStateGUIController;

    public GameObject KeyBoard;

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
        keyboardDewellTimeStateGUIController.EnableSelf();
        KeyBoard.SetActive(true);
        base.enterState();

    }

    public override void exitState()
    {
        keyboardDewellTimeStateGUIController.DisableSelf();
        KeyBoard.SetActive(false);
        base.exitState();

    }


}
