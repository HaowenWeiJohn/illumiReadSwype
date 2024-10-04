using Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardFreeSwitchStateGUIController : StateGUIController
{

    public KeyboardManager keyboardManager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }


    public override void EnableSelf()
    {
        base.EnableSelf();
        keyboardManager.SetKeyboardInteractionMode(Presets.InteractionMode.FreeSwitch);
        keyboardManager.ClearOutputFieldText();
        keyboardManager.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        keyboardManager.DisableSelf();
    }
}
