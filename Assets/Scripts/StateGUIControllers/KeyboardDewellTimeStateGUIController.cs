using Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDewellTimeStateGUIController : StateGUIController
{
    // Start is called before the first frame update

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
        KeyboardManager.setKeyboardInteractionMode(Presets.InteractionMode.DwellTime);
        KeyboardManager.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        KeyboardManager.DisableSelf();
    }

}
