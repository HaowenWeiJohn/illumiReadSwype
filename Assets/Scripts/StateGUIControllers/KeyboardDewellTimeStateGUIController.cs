using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDewellTimeStateGUIController : StateGUIController
{
    // Start is called before the first frame update

    public UserInterfaceController userInterfaceController;

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
        userInterfaceController.setKeyboardInteractionMode(Presets.InteractionMode.DwellTime);
        userInterfaceController.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        userInterfaceController.DisableSelf();
    }

}
