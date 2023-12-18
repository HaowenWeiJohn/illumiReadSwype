using Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDewellTimeStateGUIController : StateGUIController
{
    // Start is called before the first frame update

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
        keyboardManager.SetKeyboardInteractionMode(Presets.InteractionMode.DwellTime);
        keyboardManager.ClearOutputFieldText();
        keyboardManager.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        keyboardManager.DisableSelf();
    }

}
