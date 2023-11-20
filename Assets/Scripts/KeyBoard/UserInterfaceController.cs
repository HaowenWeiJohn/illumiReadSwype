using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController: MonoBehaviour
{
    // Start is called before the first frame update


    GameManager gameManager;

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




    public virtual void EnableSelf()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableSelf()
    {
        gameObject.SetActive(false);
    }





}
