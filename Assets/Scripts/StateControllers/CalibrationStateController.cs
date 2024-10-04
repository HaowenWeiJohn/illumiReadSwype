using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationStateController : StateController
{




    public CalibrationStateGUIController calibrationStateGUIController;


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
        calibrationStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        calibrationStateGUIController.DisableSelf();
        base.exitState();

    }


}
