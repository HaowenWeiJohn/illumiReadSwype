using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackStateController : StateController
{




    public FeedbackStateGUIController feedbackStateGUIController;


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
        feedbackStateGUIController.EnableSelf();
        base.enterState();

    }

    public override void exitState()
    {
        feedbackStateGUIController.DisableSelf();
        base.exitState();

    }


}
