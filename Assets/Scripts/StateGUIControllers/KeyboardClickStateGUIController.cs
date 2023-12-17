using Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardClickStateGUIController : StateGUIController
{

    public KeyboardManager KeyboardManager;

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
        KeyboardManager.setKeyboardInteractionMode(Presets.InteractionMode.ButtonClick);
        KeyboardManager.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        KeyboardManager.DisableSelf();
    }
}
