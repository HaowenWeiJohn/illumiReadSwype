using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardIllumiReadSwypeStateGUIController : StateGUIController
{

    public UserInterfaceController userInterfaceController;

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
        userInterfaceController.setKeyboardInteractionMode(Presets.InteractionMode.IllumiReadSwype);
        userInterfaceController.EnableSelf();
    }

    public override void DisableSelf()
    {
        base.DisableSelf();
        userInterfaceController.DisableSelf();
    }

}
