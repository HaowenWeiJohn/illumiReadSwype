using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardIllumiReadSwypeStateController : StateController
{




    public KeyboardIllumiReadSwypeStateGUIController keyboardIllumiReadSwypeStateGUIController;

    public GameObject KeyBoard;

    public GameObject PaintCursor;


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
        KeyBoard.SetActive(true);
        PaintCursor.SetActive(true);
        base.enterState();

    }

    public override void exitState()
    {
        keyboardIllumiReadSwypeStateGUIController.DisableSelf();
        KeyBoard.SetActive(false);
        PaintCursor.SetActive(false);
        base.exitState();

    }


}
