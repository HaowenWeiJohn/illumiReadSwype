using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Varjo.XR;
using Leap.Unity;

public class StartStateController : StateController
{




    public StartStateGUIController startStateGUIController;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        stateShift();
    }


    public override void enterState()
    {
        startStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        startStateGUIController.DisableSelf();
        base.exitState();

    }

    public override void stateShift()
    {
        if(VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            // base.stateShift();
            exitState();
        }
        
    }


}
