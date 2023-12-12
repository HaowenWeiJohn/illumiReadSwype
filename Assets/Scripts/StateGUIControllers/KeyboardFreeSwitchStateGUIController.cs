using Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardFreeSwitchStateGUIController : StateGUIController
{
    public KeyboardManager keyboardManager;

    // Start is called before the first frame update
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
        //userInterfaceController.setKeyboardInteractionMode(Presets.InteractionMode.ButtonClick);
        keyboardManager.SetKeyboardInteractionMode(Presets.InteractionMode.FreeSwitch);
        keyboardManager.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        keyboardManager.DisableSelf();
    }
}
