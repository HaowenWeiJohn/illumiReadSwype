using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController: MonoBehaviour
{
    // Start is called before the first frame update


    public GameManager gameManager;

    public KeyboardController keyBoardController;
    public KeyboardInputFieldCanvasController keyboardInputFieldCanvasController;





    void Start()
    {
        
    }


    void Update()
    {
        




    }

    

    public void UpdateKeyInput(KeyParams.Keys key)
    {
        keyboardInputFieldCanvasController.UpdateKeyInput(key);
    }




    public void EnableSelf()
    {
        keyBoardController.ResetKeyboard();
        keyboardInputFieldCanvasController.ResetInputField();
        gameObject.SetActive(true);
    }

    public void DisableSelf()
    {
        keyBoardController.ResetKeyboard();
        keyboardInputFieldCanvasController.ResetInputField();
        gameObject.SetActive(false);
    }


    public void setKeyboardInteractionMode(Presets.InteractionMode interactionMode)
    {
        keyBoardController.interactionMode = interactionMode;
    }


    


}
